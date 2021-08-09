using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QNetZ.DDL
{
	public class StationURL : IAnyData
	{
		public StationURL()
		{
			UrlScheme = "";
			Address = "";
			Parameters = new Dictionary<string, int>();
			Valid = false;
		}

		public StationURL(string urlStringText) : this()
		{
			urlString = urlStringText;
		}

		public StationURL(string scheme, string address, IDictionary<string, int> parameters) : this()
		{
			UrlScheme = scheme;
			Address = address;

			if(parameters != null)
			{
				foreach(var key in parameters.Keys)
				{
					Parameters.TryAdd(key, parameters[key]);
				}
			}
		}

		string _urlString;

		public string urlString {
			get
			{
				BuildUrlString();
				return _urlString;
			} 
			set
			{
				_urlString = value;
				ParseStationUrl();
			}
		}

		public bool Valid;

		public string UrlScheme;	// "prudp" or "prudps"
		public string Address;
		public Dictionary<string, int> Parameters;

		void BuildUrlString()
		{
			// "prudp:/address=127.0.0.1;port=5004;sid=15;type=2;RVCID=4660"

			var paramsString = string.Join(";", Parameters.Keys.Select(x => $"{x}={Parameters[x]}"));

			var strSep = paramsString.Length > 0 ? ";" : "";

			_urlString = $"{ UrlScheme }:/address={ Address }{strSep}{ paramsString }";

			Valid = true;
		}

		void ParseStationUrl()
		{
			Valid = false;

			var urlParts = _urlString.Split(":/");
			if (urlParts.Length != 2)
				return;

			UrlScheme = urlParts[0];

			var parameterList = urlParts[1].Split(";");
			foreach(var param in parameterList)
			{
				var key_value = param.Split("=");
				if (key_value.Length != 2)
					return;

				if (key_value[0] == "address")
					Address = key_value[1];
				else
					Parameters.TryAdd(key_value[0], Convert.ToInt32(key_value[1]));
			}

			Valid = true;
		}

		public override string ToString()
		{
			return urlString;
		}

		public void Read(Stream s)
		{
			urlString = Helper.ReadString(s);
		}

		public void Write(Stream s)
		{
			Helper.WriteString(s, urlString);
		}
	}
}
