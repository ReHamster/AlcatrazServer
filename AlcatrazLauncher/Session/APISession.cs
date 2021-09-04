using RestSharp;
using Alcatraz.DTO.Models;
using SharedModels.Helpers;
using AlcatrazLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using RestSharp.Serializers.NewtonsoftJson;
using Newtonsoft.Json;

namespace AlcatrazLauncher.Session
{
	public class RouteAttribute : Attribute
	{
		public string Template { get; set; }
		public RouteAttribute(string template)
		{
			Template = template;
		}
	}

	public class APIMethods
	{
		public APISession Session { get; set; }

		// Выполняет POST запрос с отправкой файлов
		protected void DoAsyncPostFiles(string action, ICollection<string> fileNames, Action<IRestResponse> onComplete)
		{
			TypeInfo typeInfo = this.GetType().GetTypeInfo();
			var apiRouteAttr = typeInfo.GetCustomAttribute<RouteAttribute>();

			if (apiRouteAttr == null)
				throw new NullReferenceException("DoAsyncPOST - cannot determine Route for APIMethods, did you forgot Attribute???");

			var request = new RestRequest(string.Format("{0}/{1}", apiRouteAttr.Template, action), Method.POST);

			request.AlwaysMultipartFormData = true;
	
			foreach (var name in fileNames)
			{
				long length = new System.IO.FileInfo(name).Length;

				request.Files.Add(new FileParameter
				{
					Name = "files",
					Writer = (s) =>
					{
						FileStream stream = File.Open(name, FileMode.Open);
						stream.CopyTo(s);
						stream.Dispose();
					},
					FileName = Path.GetFileName(name),
					ContentType = MimeTypesHelper.GetContentType(name),
					ContentLength = length,
				});
			}

			// before request, authenticate
			if (APISession.Authenticator != null)
				APISession.Authenticator.Authenticate(APISession.WebClient, request);

			ExecuteWithRetryRequestPrompt(() =>
			{
				APISession.WebClient.ExecuteAsync(request, response =>

					Session.EventQueue.Add(() => {
						if (response.ResponseStatus != ResponseStatus.Completed)
						{
							ThrowRequestError(request.Resource, response.ResponseStatus.ToString(), HttpStatusCode.NotFound);
							return;
						}

						if (response.StatusCode == HttpStatusCode.OK)
							onComplete(response);
						else
							ThrowRequestError(request.Resource, response.Content, response.StatusCode);
					})

				);
			});
		}

		// Выполняет POST запрос с параметрами в URL
		protected void DoAsyncPOST(string action, Dictionary<string, string> queryParams, object jsonPayload, Action<IRestResponse> onComplete)
		{ 
			TypeInfo typeInfo = this.GetType().GetTypeInfo();
			var apiRouteAttr = typeInfo.GetCustomAttribute<RouteAttribute>();

			if (apiRouteAttr == null)
				throw new NullReferenceException("DoAsyncPOST - cannot determine Route for APIMethods, did you forgot Attribute???");

			var request = new RestRequest(string.Format("{0}/{1}", apiRouteAttr.Template,  action), Method.POST);
			request.AddJsonBody(jsonPayload);

			if (queryParams != null)
			{ 
				foreach (var v in queryParams)
					request.AddQueryParameter(v.Key, v.Value);
			}

			// before request, authenticate
			if (APISession.Authenticator != null)
				APISession.Authenticator.Authenticate(APISession.WebClient, request);

			ExecuteWithRetryRequestPrompt(() =>
			{
				APISession.WebClient.ExecuteAsync(request, response =>

					Session.EventQueue.Add(() => {
						if (response.ResponseStatus != ResponseStatus.Completed)
						{
							ThrowRequestError(request.Resource, response.ResponseStatus.ToString(), HttpStatusCode.NotFound);
							return;
						}

						if (response.StatusCode == HttpStatusCode.OK)
							onComplete(response);
						else
							ThrowRequestError(request.Resource, response.Content, response.StatusCode);
					})

				);
			});

		}

		// Выполняет POST запрос
		protected void DoAsyncPOST(string action, object jsonPayload, Action<IRestResponse> onComplete)
		{
			DoAsyncPOST(action, null, jsonPayload, onComplete);
		}

		// Для анонимных запросов: обработка ошибок выполняется в вашем onComplete, в том числе при ошибке соединения
		protected void DoAsyncPOST_CustomHandler(string action, Dictionary<string, string> queryParams, object jsonPayload, Action<IRestResponse> onComplete)
		{
			TypeInfo typeInfo = this.GetType().GetTypeInfo();
			var apiRouteAttr = typeInfo.GetCustomAttribute<RouteAttribute>();

			if (apiRouteAttr == null)
				throw new NullReferenceException("DoAsyncPOST - cannot determine Route for APIMethods, did you forgot Attribute???");

			var request = new RestRequest(string.Format("{0}/{1}", apiRouteAttr.Template, action), Method.POST);

			if (queryParams != null)
			{ 
				foreach (var v in queryParams)
					request.AddQueryParameter(v.Key, v.Value);
			}

			request.AddJsonBody(jsonPayload);

			// before request, authenticate
			if(APISession.Authenticator != null)
				APISession.Authenticator.Authenticate(APISession.WebClient, request);

			// добавляем в очередь на обработку
			ExecuteWithRetryRequestPrompt(() =>
			{
				APISession.WebClient.ExecuteAsync(request, response =>
					Session.EventQueue.Add(() => onComplete(response))
				);
			});
		}

		// Для анонимных запросов: обработка ошибок выполняется в вашем onComplete, в том числе при ошибке соединения
		protected void DoAsyncPOST_CustomHandler(string action, object jsonPayload, Action<IRestResponse> onComplete)
		{
			DoAsyncPOST_CustomHandler(action, null, jsonPayload, onComplete);
		}

		// Выполняет GET запрос
		protected void DoAsyncGET(string action, Dictionary<string, string> queryParams, Action<IRestResponse> onComplete)
		{
			TypeInfo typeInfo = this.GetType().GetTypeInfo();
			var apiRouteAttr = typeInfo.GetCustomAttribute<RouteAttribute>();

			if (apiRouteAttr == null)
				throw new NullReferenceException("DoAsyncGET - cannot determine Route for APIMethods, did you forgot Attribute???");

			var request = new RestRequest(string.Format("{0}/{1}", apiRouteAttr.Template, action), Method.GET);

			if (queryParams != null)
			{
				foreach (var v in queryParams)
					request.AddQueryParameter(v.Key, v.Value);
			}

			// before request, authenticate
			if (APISession.Authenticator != null)
				APISession.Authenticator.Authenticate(APISession.WebClient, request);

			ExecuteWithRetryRequestPrompt(() =>
			{
				APISession.WebClient.ExecuteAsync(request, response =>
					Session.EventQueue.Add(() => {
						if (response.ResponseStatus != ResponseStatus.Completed)
						{
							ThrowRequestError(request.Resource, response.ResponseStatus.ToString(), HttpStatusCode.NotFound);
							return;
						}

						if (response.StatusCode == HttpStatusCode.OK)
							onComplete(response);
						else
							ThrowRequestError(request.Resource, response.Content, response.StatusCode);
					})
				);
			});
		}

		protected void DoAsyncGET(string action, Action<IRestResponse> onComplete)
		{
			DoAsyncGET(action, new Dictionary<string, string>(), onComplete);
		}

		protected void ExecuteWithRetryRequestPrompt(Action func)
		{
			bool hasError = false;

			do
			{
				try
				{
					func.Invoke();
				}
				catch (Exception ex)
				{
					// retry?
					hasError = true;

					var result = MessageBox.Show(null, "Connection error: \n" + ex.Message + "\n\nRetry request?", 
						"Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

					if (result == DialogResult.No || result == DialogResult.Cancel)
						break;
				}
			} while (hasError);

		}

		protected void ThrowRequestError(string resource, string errorContent, HttpStatusCode statusCode)
		{
			// если это BadRequest, а BadRequest задаю я в API, то это ErrorDto
			// ошибка для программиста фронтенда
			if (statusCode == HttpStatusCode.BadRequest)
			{
				var errorJson = JsonConvert.DeserializeObject<ErrorModel>(errorContent);

				// may be send error to the USER???
				MessageBox.Show(errorJson.Message, resource);
			}
			else if (statusCode == HttpStatusCode.Unauthorized)
			{
				// если авторизация куда-то подевалась, или мы не имеем права доступа
				MessageBox.Show(statusCode.ToString(), resource);
			}
			else // остальные ошибки это уже по бэкэнду
			{
				MessageBox.Show(errorContent, resource);
			}
		}
	}

	//
	// Частичный класс сессии API, который расширяется через ***Methods.cs
	//
	public partial class APISession
	{
		private static RestClient _client { get; set; }
		public static RestClient WebClient
		{
			get
			{
				if(_client == null)
				{
					var apiServer = ConfigurationManager.AppSettings.Get(Constants.SERVICE_URL_KEY);

					_client = new RestClient("http://" + apiServer);
					_client.UseNewtonsoftJson();
				}

				return _client;
			}
			private set
			{
				_client = value;
			}
		}

		public static RestSharp.Authenticators.IAuthenticator Authenticator {get;set;}

		public IEventQueue EventQueue { get; }

		// constructor
		public APISession(IEventQueue queue)
		{
			EventQueue = queue;
		}
	}
}
