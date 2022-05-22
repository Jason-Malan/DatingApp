using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

#nullable disable

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            this._context = context;
            this._tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<PlatformUserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            PlatformUser user = new PlatformUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
            };

            _context?.PlatformUsers?.Add(user);
            await _context.SaveChangesAsync();

            return new PlatformUserDto { Username = user.UserName, Token = _tokenService.CreateToken(user) };
        }

        [HttpPost("login")]
        public async Task<ActionResult<PlatformUserDto>> Login(LoginDto loginDto)
        {
            PlatformUser user = await _context.PlatformUsers.SingleOrDefaultAsync(user => user.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            using HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);

            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            var usersPhotos = (await _context.Photos.ToListAsync()).FindAll(x => x.PlatformUserId == user.Id);

            return new PlatformUserDto 
            {   
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = usersPhotos.FirstOrDefault(x => x.IsMain.Value == 1)?.Url
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.PlatformUsers.AnyAsync(user => user.UserName == username.ToLower());
        }

    }
}
