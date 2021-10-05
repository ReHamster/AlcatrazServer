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
		uint Register(UserRegisterModel model);
		bool Update(UserModel model);
		bool ChangePassword(uint userId, string newPassword);
		IEnumerable<UserModel> GetAll();
		UserModel GetById(uint id);
		string GenerateJwtToken(UserModel userModel);
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
			var token = GenerateJwtToken(userModel);

			return new AuthenticateResponse(userModel, token);
		}

		public IEnumerable<UserModel> GetAll()
		{
			return _dbContext.Users
				.AsNoTracking()
				.Select(x => new UserModel
				{
					Id = x.Id,
					PlayerNickName = x.PlayerNickName,
					Username = x.Username
				}).ToArray();
		}

		public UserModel GetById(uint id)
		{
			return _dbContext.Users
				.AsNoTracking()
				.Select(x => new UserModel
				{
					Id = x.Id,
					PlayerNickName = x.PlayerNickName,
					Username = x.Username
				})
				.FirstOrDefault(x => x.Id == id);
		}

		private User GetByIdInternal(uint id)
		{
			return _dbContext.Users.FirstOrDefault(x => x.Id == id);
		}

		public uint Register(UserRegisterModel model)
		{
			if (string.IsNullOrWhiteSpace(model.Username))
				return 0;

			if (string.IsNullOrWhiteSpace(model.PlayerNickName))
				return 0;

			if (string.IsNullOrWhiteSpace(model.Password))
				return 0;

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

		public bool Update(UserModel model)
		{
			var user = GetByIdInternal(model.Id);

			if (user == null)
				return false;

			try
			{
				user.Username = model.Username;
				user.PlayerNickName = model.PlayerNickName;
				_dbContext.SaveChanges();
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool ChangePassword(uint userId, string newPassword)
		{
			var user = GetByIdInternal(userId);

			if (user == null)
				return false;

			try
			{
				user.Password = newPassword;
				_dbContext.SaveChanges();
			}
			catch
			{
				return false;
			}

			return true;
		}

		// helper methods

		public string GenerateJwtToken(UserModel user)
		{
			// generate token that is valid for 7 days
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] {
					new Claim("uid", user.Id.ToString()),
					new Claim(ClaimTypes.Name, user.Username), // FIXME: Email?
				}),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
