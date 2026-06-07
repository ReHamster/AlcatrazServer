using Alcatraz.Context;
using Alcatraz.Context.Entities;
using Alcatraz.DTO.Helpers;
using Alcatraz.DTO.Models;
using Alcatraz.GameServices.Helpers;
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
		ResultModel Register(UserRegisterModel model);
		ResultModel Update(UserModel model);
		ResultModel ChangePassword(uint userId, string newPassword);
		ResultModel ResetPassword(uint userId);
		ResultModel Delete(uint userId);

		IEnumerable<UserModel> GetAll();
		UserModel GetById(uint id);

		public IEnumerable<Claim> GetUserClaims(UserModel user);
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
				.SingleOrDefault(x => x.Username == model.Username);

			// return null if user not found
			if (user == null) 
				return null;

			if(!SecurePasswordHasher.Verify($"{user.Id}-{model.Password}", user.Password))
			{
				return null;
			}

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
					Username = x.Username,
					CreatedTime = x.CreatedTime,
					LastUpdateTime = x.LastUpdateTime,
					LastPlayTime = x.LastPlayTime,
					IsAdmin = x.IsAdmin,
					IsPasswordReset = x.IsPasswordReset
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
					Username = x.Username,
					CreatedTime = x.CreatedTime,
					LastUpdateTime = x.LastUpdateTime,
					LastPlayTime = x.LastPlayTime,
					IsAdmin = x.IsAdmin,
					IsPasswordReset = x.IsPasswordReset
				})
				.FirstOrDefault(x => x.Id == id);
		}

		private User GetByIdInternal(uint id)
		{
			return _dbContext.Users.FirstOrDefault(x => x.Id == id);
		}

		public ResultModel Register(UserRegisterModel model)
		{
			if (string.IsNullOrWhiteSpace(model.Username))
				return new ResultModel("Username is incorrect or empty");

			if (string.IsNullOrWhiteSpace(model.PlayerNickName))
				return new ResultModel("PlayerNickName is incorrect or empty");

			if (string.IsNullOrWhiteSpace(model.Password))
				return new ResultModel("Password is incorrect or empty");

			var newUser = new User()
			{
				Username = model.Username,
				PlayerNickName = model.PlayerNickName,
				Password = "tmp",
				CreatedTime = DateTime.Now,
				LastUpdateTime = DateTime.MinValue,
				LastPlayTime = DateTime.MinValue
			};

			if(!_dbContext.Users.Any())
			{
				// Add ADMIN user with starting ID == 1000
				_dbContext.Users.Add(new User()
				{
					Id = 1000,
					Username = "ADMIN",
					PlayerNickName = "ADMIN",
					Password = SecurePasswordHasher.Hash($"1000-{_appSettings.DefaultAdminPassword}"),
					RewardFlags = 0,
					IsAdmin = true,
					IsPasswordReset = false,
					CreatedTime = DateTime.Now,
					LastUpdateTime = DateTime.MinValue,
					LastPlayTime = DateTime.MinValue
				});
				_dbContext.SaveChanges();
			}

			if (_dbContext.Users.Any(x => x.Username == model.Username || x.PlayerNickName == model.PlayerNickName))
				return new ResultModel("User with same name or nickname is already present");

			try
			{
				_dbContext.Users.Add(newUser);
				_dbContext.SaveChanges();
			}
			catch
			{
				return new ResultModel("Unable to add user (internal error)");
			}

			// update password as user Id is acquired
			{
				newUser.Password = SecurePasswordHasher.Hash($"{newUser.Id}-{model.Password}");
				_dbContext.SaveChanges();
			}

			return new ResultModel(newUser.Id);
		}

		public ResultModel Update(UserModel model)
		{
			var user = GetByIdInternal(model.Id);

			if (user == null)
				return new ResultModel("User with that Id was not found");

			if(user.Username == model.Username && user.PlayerNickName == model.PlayerNickName)
			{
				// User name and nickname remains unchanged
				return new ResultModel();
			}

			if (user.Username != model.Username && _dbContext.Users.Any(x => x.Username == model.Username))
			{
				return new ResultModel("User with same name is already present");
			}

			if (user.PlayerNickName != model.PlayerNickName && _dbContext.Users.Any(x => x.PlayerNickName == model.PlayerNickName))
			{
				return new ResultModel("User with same nickname is already present");
			}

			try
			{
				user.LastUpdateTime = DateTime.Now;
				user.Username = model.Username;
				user.PlayerNickName = model.PlayerNickName;

				_dbContext.SaveChanges();
			}
			catch
			{
				return new ResultModel("Unable to update user (internal error)");
			}

			return new ResultModel();
		}

		public ResultModel Delete(uint userId)
		{
			var user = GetByIdInternal(userId);
			if (user == null)
				return new ResultModel("Invalid user");
			if(user.IsAdmin)
			{
				if(_dbContext.Users.Count(x => x.IsAdmin) <= 1)
					return new ResultModel("Cannot delete last user with Admin rights");
			}

			// delete player statistics entirely
			foreach(var board in _dbContext.PlayerStatisticBoards.Where(x => x.PlayerId == userId))
			{
				uint boardId = board.Id;

				_dbContext.Entry(board).State = EntityState.Deleted;
				foreach (var boardValue in _dbContext.PlayerStatisticBoardValues.Where(x => x.PlayerBoardId == boardId))
					_dbContext.Entry(boardValue).State = EntityState.Deleted;
			}

			_dbContext.Entry(user).State = EntityState.Deleted;
			_dbContext.SaveChanges();

			return new ResultModel();
		}

		public ResultModel ChangePassword(uint userId, string newPassword)
		{
			var user = GetByIdInternal(userId);

			if (user == null)
				return new ResultModel("User with that Id was not found");

			try
			{
				user.LastUpdateTime = DateTime.Now;
				user.Password = SecurePasswordHasher.Hash($"{user.Id}-{newPassword}");
				user.IsPasswordReset = false;
				_dbContext.SaveChanges();
			}
			catch
			{
				return new ResultModel("Unable to update user (internal error)");
			}

			return new ResultModel();
		}

		public ResultModel ResetPassword(uint userId)
		{
			var user = GetByIdInternal(userId);

			if (user == null)
				return new ResultModel("User with that Id was not found");

			try
			{
				user.LastUpdateTime = DateTime.Now;
				user.Password = SecurePasswordHasher.Hash($"{user.Id}-{_appSettings.DefaultResetPassword}");
				user.IsPasswordReset = true;
				_dbContext.SaveChanges();
			}
			catch
			{
				return new ResultModel("Unable to update user (internal error)");
			}

			return new ResultModel();
		}

		// helper methods

		public string GenerateJwtToken(UserModel user)
		{
			var claims = GetUserClaims(user);

			// generate token that is valid for 7 days
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public IEnumerable<Claim> GetUserClaims(UserModel user)
		{
			return new[] {
				new Claim("uid", user.Id.ToString()),
				new Claim(ClaimTypes.Role, user.IsAdmin ? "admin" : "user"),
				new Claim(ClaimTypes.Name, user.Username), // FIXME: Email?
			};
		}
	}
}
