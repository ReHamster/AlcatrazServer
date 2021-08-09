using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;
using System.Collections.Generic;

namespace DSFServices.Services
{
	[RMCService(RMCProtocolId.DriverG2WService)]
	public class DriverG2WService : RMCServiceBase
	{
		[RMCMethod(3)]
		public RMCResult UnlockG2W(List<UnlockInputData> unlocksIds, string table)
		{
			UNIMPLEMENTED();

			/*
				{
				  "unlocksIds": [
					{
					  "id": 62,
					  "status": 0
					}
				  ],
				  "table": "vehicles"
				}

				REQ2:

			{
			  "unlocksIds": [
				{
				  "id": 172,
				  "status": 19
				},
				{
				  "id": 168,
				  "status": 19
				},
				{
				  "id": 169,
				  "status": 19
				},
				{
				  "id": 170,
				  "status": 19
				},
				{
				  "id": 171,
				  "status": 19
				}
			  ],
			  "table": "challenges"
			}
			 */

			return Error(0);
		}

		[RMCMethod(4)]
		public RMCResult UploadedReplay(string replayid, string title, string description)
		{
			UNIMPLEMENTED();
			return Error(0);
		}
	}
}
