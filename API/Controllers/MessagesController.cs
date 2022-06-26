using API.DTOs;
using API.Extensions;
using API.Interfaces;
using API.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseAPIController
    {
        private readonly IPlatformUserDataManager platformUserDataManager;
        private readonly IMessageDataManager messageDataManager;
        private readonly IPhotoDataManager photoDataManager;
        private readonly IMapper mapper;

        public MessagesController(IPlatformUserDataManager platformUserDataManager,
                                  IMessageDataManager messageDataManager,
                                  IPhotoDataManager photoDataManager,
                                  IMapper mapper)
        {
            this.platformUserDataManager = platformUserDataManager;
            this.messageDataManager = messageDataManager;
            this.photoDataManager = photoDataManager;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot send messages to yourself");

            var sender = await platformUserDataManager.GetUserByUsernameAsync(username);
            var recipient = await platformUserDataManager.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            var senderDto = await platformUserDataManager.GetFrontendUserByUsernameAsync(username);
            var recipientDto = await platformUserDataManager.GetFrontendUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content,
            };

            messageDataManager.AddMessage(message);

            var messageDtoToReturn = mapper.Map<MessageDto>(message);
            messageDtoToReturn.SenderPhotoUrl = senderDto.PhotoUrl!;
            messageDtoToReturn.RecipientPhotoUrl = recipientDto.PhotoUrl!;

            if (await messageDataManager.SaveAllAsync()) return Ok(messageDtoToReturn);

            return BadRequest("Failed to send message");
        }
    }
}
