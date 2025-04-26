using ASAPTaskAPI.Application.Interfaces;
using ASAPTaskAPI.Domain.Entities;
using System.Security.Claims;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using ASAPTaskAPI.Infrastructure.Data;
using ASAPTaskAPI.Infrastructure.Interface;
using System.Threading.Tasks;
using ASAPTaskAPI.Application.Dto;

namespace ASAPTaskAPI.Application.Services
{
  
    public class UserAppService : IUserAppService
    {
        private readonly IConfiguration _config;
        private readonly IRepository<User> _userRepo;

        public UserAppService(IRepository<User> userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<string> Authenticate(LoginDto input)
        {
            var user =await _userRepo.FirstOrDefaultAsync(u => u.Username == input.Username.Trim());
            if (user == null || !BCrypt.Net.BCrypt.Verify(input.Password.Trim(), user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JwtSettings:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, input.Username.Trim()) }),
                Expires = DateTime.UtcNow.AddHours(1),
                Audience = _config["JwtSettings:Audience"], // Set the audience here
                Issuer = _config["JwtSettings:Issuer"],   // Set the issuer here
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task Register(LoginDto input)
        {
            var user = await _userRepo.FirstOrDefaultAsync(u => u.Username == input.Username.Trim());
            if (user != null)
                throw new Exception("User already exists");

            user = new User
            {
                Username = input.Username.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(input.Password.Trim())
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();
        }
    }
}
