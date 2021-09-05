using Alcatraz.DTO.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

// FIXME: вот этот код вообще можно было бы генерить через Client generator какой-нибудь

namespace AlcatrazLauncher.Session
{
	// расширение класса APISession
	public partial class APISession
	{
		private AccountMethods _account;
		public AccountMethods Account
		{
			get
			{
				return _account ?? new AccountMethods { Session = this };
			}
			private set
			{
				_account = value;
			}
		}

		[Route("Users")]
		public class AccountMethods : APIMethods
		{
			public void Authenticate(AuthenticateRequest data, Action<IRestResponse> onComplete, Action<IRestResponse> onFailedResponse)
			{
				DoAsyncPOST_CustomHandler("Authenticate", data, (response) => {

					if (response.ResponseStatus != ResponseStatus.Completed)
					{
						onFailedResponse(response);
						return;
					}

					if (response.StatusCode == HttpStatusCode.OK)
					{
						var loginData = JsonConvert.DeserializeObject<AuthenticateResponse>(response.Content);

						Authenticator = new RestSharp.Authenticators.JwtAuthenticator(loginData.Token);
					}

					onComplete(response);
				});
			}

			public void Register(UserRegisterModel data, Action<IRestResponse> onComplete, Action<IRestResponse> onFailedResponse)
			{
				DoAsyncPOST("Register", data, (response) => {
					if (response.ResponseStatus != ResponseStatus.Completed)
					{
						onFailedResponse(response);
						return;
					}

					onComplete(response);
				});
			}

			public void GetAll(Action<IEnumerable<UserModel>> onComplete)
			{
				DoAsyncGET("GetAll", (response) =>
				{
					var responseJson = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(response.Content);
					onComplete(responseJson);
				});
			}

			public void ChangePassword(ChangePasswordRequest data, Action<IRestResponse> onComplete)
			{
				DoAsyncPOST("ChangePassword", data, (response) => {
					onComplete(response);
				});
			}

			public void UpdateUser(UserModel data, Action<IRestResponse> onComplete)
			{
				DoAsyncPOST("UpdateUser", data, (response) => {
					onComplete(response);
				});
			}

		}
	}
}
