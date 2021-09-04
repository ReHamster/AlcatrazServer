using Alcatraz.Context;
using Alcatraz.Context.Entities;
using Alcatraz.GameServices.Helpers;
using Alcatraz.DTO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Alcatraz.GameServices.Services
{
	public interface IUserService
	{
		AuthenticateResponse Authenticate(AuthenticateRequest model);
		public uint Register([FromBody] UserRegisterModel model);

		IEnumerable<User> GetAll();
		User GetById(int id);
	}

	public class UserService : IUserService
	{
		// users hardcoded for simplicity, store in a db with hashed passwords in production applications
		private readonly AppSettings _appSettings;
		private readonly MainDbContext _dbContext;

		public UserService(IOptions<AppSettings> appSettings, MainDbContext dbContext)
		{
			_appSettings = appSettings.Value;
			_dbContext = dbContext;
		}

		public AuthenticateResponse Authenticate(AuthenticateRequest model)
		{
			var user = _dbContext.Users
				.AsNoTracking()
				.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

			// return null if user not found
			if (user == null) 
				return null;

			var userModel = new UserModel
			{
				Id = user.Id,
				PlayerNickName = user.PlayerNickName,
				Username = user.Username,
			};

			// authentication successful so generate jwt token
			var token = generateJwtToken(userModel);

			return new AuthenticateResponse(userModel, token);
		}

		public IEnumerable<User> GetAll()
		{
			return _dbContext.Users.AsNoTracking().ToArray();
		}

		public User GetById(int id)
		{
			return _dbContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == id);
		}

		public uint Register([FromBody] UserRegisterModel model)
		{
			var newUser = new User()
			{
				Username = model.Username,
				PlayerNickName = model.PlayerNickName,
				Password = model.Password,
			};

			if (_dbContext.Users.Any(x => x.Username == model.Username || x.PlayerNickName == model.PlayerNickName))
				return 0;

			try
			{
				_dbContext.Users.Add(newUser);
				_dbContext.SaveChanges();
			}
			catch
			{
				return 0;
			}

			return newUser.Id;
		}

		// helper methods

		private string generateJwtToken(UserModel user)
		{
			// generate token that is valid for 7 days
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
