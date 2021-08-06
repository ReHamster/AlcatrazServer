﻿using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using System.Collections.Generic;

namespace QuazalWV.Services
{
	[RMCService(RMCProtocolId.SocialNetworksService)]
	public class SocialNetworksService : RMCServiceBase
	{
		[RMCMethod(1)]
		public void PostTwitterMessage(string message)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(2)]
		public void PostFBMessage(string message, string urlText, string urlhref, string urlCaption, string urlDescription)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(3)]
		public void RevokeFBAuthorization()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(4)]
		public void RevokeTwitterAuthorization()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(5)]
		public void LoginFB(string username, string password)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(6)]
		public void LoginTwitter(string username, string password)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(7)]
		public RMCResult HasTwitterAuth()
		{
			return Result(new { a = false });
		}

		[RMCMethod(8)]
		public RMCResult HasFBAuth()
		{
			return Result(new { a = false });
		}

	}
}
