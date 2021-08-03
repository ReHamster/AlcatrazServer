using System.IO;
using System.Text;

namespace QuazalWV
{
	public class RMCP
    {
        public enum PROTOCOL
        {
            NATTraversalService 			= 3,
            TicketGrantingService 			= 10,	// U
            SecureConnectionService 		= 11,	// U
            NotificationEventManager 		= 14,
            FriendsService 					= 20,	// U
            MatchMakingService 				= 21,
            PersistentStoreService 			= 24,
            AccountManagementService 		= 25,
            UbiAccountManagementService 	= 29,	// U
            NewsService 					= 31,
            UbiNewsService 					= 33,
            PrivilegesService 				= 35,	// U
            TrackingProtocol3Client 		= 36,
            LocalizationService 			= 39,
            MatchMakingProtocolExtClient 	= 50,
            LSPService 						= 81,
            PlayerStatisticsService			= 108,
            RichPresenceService				= 109,
            ClansService					= 110,
            MetaSessionService				= 112,
            GameInfoService					= 113,
            ContactsExtensionsService		= 114,
            HermesAchievementsService		= 116,
            HermesPartySessionService		= 117,
            DriverIDManagerService			= 118,
            SocialNetworksService			= 119,
            UplayWinService                 = 120,
            DriverG2WService				= 121,
            UplayPassService				= 123,
            OverlordFriendsService			= 5005

            /*
            NATTraversalRelayProtocol           = 3,            
            GlobalNotificationEventProtocol     = 14,
            MatchMakingService                  = 21,
            MessageDeliveryProtocol             = 27,
            AuthenticationService               = 10,
            SecureService                       = 11,
            FriendsService                      = 20,
            UbiAccountManagementService         = 29,
            PrivilegesService                   = 35,
            TelemetryService                    = 36,
            AMMGameClientService                = 101,
            AMMDedicatedServerService           = 102,
            PlayerProfileService                = 103,
            ArmorService                        = 104,
            InventoryService                    = 105,
            LootService                         = 106,
            WeaponService                       = 107,
            UFriendsService                     = 108,
            ChatService                         = 110,
            MissionService                      = 111,
            PartyService                        = 112,
            RegistrationService                 = 113,
            StatisticsService                   = 114,
            AchievementsService                 = 115,
            ProgressionService                  = 116,
            DBGTelemetryService                 = 117,
            RewardService                       = 118,
            StoreService                        = 119,
            AdvertisementsService               = 121,
            SkillsService                       = 122,
            LoadoutService                      = 123,
            TrackingService                     = 124,
            UnlockService                       = 125,
            AvatarService                       = 126,
            WeaponProficiencyService            = 127,
            OpsProtocolService                  = 128,
            ProfilerService                     = 129,
            ServerInfoService                   = 130,
            LeaderboardService                  = 131,
            PveArchetypeService                 = 133,
            InboxMessageService                 = 134,
            ProfanityFilterService              = 135,
            InspectPlayerService                = 136,
            AbilityService                      = 137,
            SurveyService                       = 139,
            LeaderboardProtocolService          = 5000,
            RPNEProtocolService                 = 5001,
            OverlordNewsProtocolService         = 5002,
            OverlordCoreProtocolService         = 5003,
            ExtraContentProtocolService         = 5004,
            OverlordFriendsProtocolService      = 5005,
            OverlordAwardsProtocolService       = 5006,
            OverlordChallengeProtocolService    = 5007,
            OverlordDareProtocolService         = 5008
            */
        }

        public PROTOCOL proto;
        public bool isRequest;
        public bool success;
        public uint error;
        public uint callID;
        public uint methodID;
        public RMCPRequest request;
        public int _afterProtocolOffset;

        public RMCP()
        {
        }

        public RMCP(QPacket p)
        {
            MemoryStream m = new MemoryStream(p.payload);
            Helper.ReadU32(m);
            ushort b = Helper.ReadU8(m);
            isRequest = (b >> 7) == 1;
            try
            {
                if ((b & 0x7F) != 0x7F)
                    proto = (PROTOCOL)(b & 0x7F);
                else
                {
                    b = Helper.ReadU16(m);
                    proto = (PROTOCOL)(b);
                }
            }
            catch
            {
                Log.WriteLine(1, "[RMC Packet] Error: Unknown RMC packet protocol 0x" + b.ToString("X2"));
                return;
            }
            _afterProtocolOffset = (int)m.Position;
        }
        

        public override string ToString()
        {
            return "[RMC Packet : Proto = " + proto + " CallID=" + callID + " MethodID=" + methodID + "]";
        }

        public string PayLoadToString()
        {
            StringBuilder sb = new StringBuilder();
            if (request != null)
                sb.Append(request);
            return sb.ToString();
        }

        public byte[] ToBuffer()
        {
            MemoryStream result = new MemoryStream();
            byte[] buff = request.ToBuffer();
            Helper.WriteU32(result, (uint)(buff.Length + 9));
            byte b = (byte)proto;
            if (isRequest)
                b |= 0x80;
            Helper.WriteU8(result, b);
            Helper.WriteU32(result, callID);
            Helper.WriteU32(result, methodID);
            result.Write(buff, 0, buff.Length);
            return result.ToArray();
        }
    }
}
