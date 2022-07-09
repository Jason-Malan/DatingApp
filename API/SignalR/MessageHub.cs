using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHubContext<PresenceHub> presenceHub;
        private readonly PresenceTracker tracker;

        public MessageHub(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<PresenceHub> presenceHub, PresenceTracker tracker)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.presenceHub = presenceHub;
            this.tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            var currentUsername = Context.User.GetUsername();
            var httpContext = Context.GetHttpContext();
            string otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(currentUsername, otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messages 
                = await unitOfWork.MessageRepository.GetMessageThread(currentUsername, otherUser);

            if (unitOfWork.HasChanges()) await unitOfWork.Complete();

            await Clients.Caller.SendAsync("RecieveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new HubException("You cannot send messages to yourself");

            var sender = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await unitOfWork.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) throw new HubException("User not found");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);

            var group = await unitOfWork.MessageRepository.GetGroup(groupName);

            if (group.Connections.Any(x => x.UserName == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            } else
            {
                var connections = await tracker.GetConnectionsForUser(recipient.UserName);
                if (connections != null)
                {
                    await presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived", new { username = sender.UserName, knownAs = sender.KnownAs });
                }
            }

            unitOfWork.MessageRepository.AddMessage(message);

            if (await unitOfWork.Complete()) {
                await Clients.Group(groupName).SendAsync("NewMessage", mapper.Map<MessageDto>(message));
            };
        }

        public async Task<Group> AddToGroup(string groupName)
        {
            var group = await unitOfWork.MessageRepository.GetGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if (group == null)
            {
                group = new Group(groupName);
                unitOfWork.MessageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            if (await unitOfWork.Complete()) return group;

            throw new HubException("Failed to join group.");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await unitOfWork.MessageRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            unitOfWork.MessageRepository.RemoveConnection(connection);

            if (await unitOfWork.Complete()) return group;

            throw new HubException("Failed to remove from group.");
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}
