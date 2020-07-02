using System;
using System.Collections.Generic;
using Smod2;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

namespace OFivePlugin
{
    public static class Global
    {
        public static Plugin plugin;
        public const string Version = "1.2.G.4";

        #region serverinfo
        private static readonly string ServerInfoFileName = "ServerInfo.xml";

        private static readonly XmlSerializer Formatter_ServerInfo = new XmlSerializer(typeof(ServerInfo));

        public static void DeleteServerInfoFile()
        {
            if (File.Exists(ServerInfoFileName))
            {
                File.Delete(ServerInfoFileName);
            }
        }

        public static void SaveServerInfo()
        {
            List<Player> players = new List<Player>();
            players.Add(Player.Server);
            foreach (Smod2.API.Player player in plugin.Server.GetPlayers())
            {
                players.Add(new Player(player.Name, player.PlayerId, player.SteamId, player.TeamRole.Role.ToString(), player.GetHealth(), player.TeamRole.MaxHP));
            }
            ServerInfo server = new ServerInfo
            {
                CurrentAdmins = plugin.Server.GetPlayers().Where(x => x.GetRankName() != "" && x.GetRankName() != null && x.GetRankName() != string.Empty).Count(),
                players = players,
                CurrentPlayers = plugin.Server.NumPlayers - 1,
                MaxPlayers = plugin.Server.MaxPlayers,
                ServerName = plugin.Server.Name
            };
            try
            {
                using (FileStream fs = new FileStream(ServerInfoFileName, FileMode.OpenOrCreate))
                {
                    Formatter_ServerInfo.Serialize(fs, server);
                }
            }
            catch (InvalidOperationException)
            {
                plugin.Info("Failed to refresh serverInfo");
            }
        }

        #endregion

        #region Ready
        private static readonly XmlSerializer Formatter_Commands = new XmlSerializer(typeof(List<ReadyCommand>));

        public static string ReadyCommandsFileName = "ReadyCommands.xml";

        public static string ResultReadyCommandsFileName = "ResultReadyCommands.xml";

        public static List<ReadyCommand> LoadReadyCommands()
        {
            List<ReadyCommand> commands = new List<ReadyCommand>();
            if (File.Exists(ReadyCommandsFileName))
            {
                using (StreamReader sw = new StreamReader(ReadyCommandsFileName))
                {
                    commands = (List<ReadyCommand>)Formatter_Commands.Deserialize(sw);
                }
            }
            File.Delete(ReadyCommandsFileName);
            return commands;
        }

        public static void SaveReadyCommands(List<ReadyCommand> Commands)
        {
            if (File.Exists(ResultReadyCommandsFileName))
            {
                File.Delete(ResultReadyCommandsFileName);
            }
            using (StreamWriter sw = new StreamWriter(ResultReadyCommandsFileName))
            {
                Formatter_Commands.Serialize(sw, Commands);
            }
        }
        #endregion

        #region request
        public static string requests_file_name = "requests.xml";
        public static XmlSerializer Formatter_Requests = new XmlSerializer(typeof(List<Request>));

        public static readonly float spam_block_time = 60.0f;
        public static bool can_use_commands = false;

        public static List<Request> Requests = new List<Request>();
        public static void SaveRequests()
        {
            if (File.Exists(requests_file_name))
            {
                File.Delete(requests_file_name);
            }
            using (StreamWriter sw = new StreamWriter(requests_file_name))
            {
                Formatter_Requests.Serialize(sw, Requests);
            }
            Requests.Clear();
        }

        public static XmlSerializer Formatter_SiteInfo = new XmlSerializer(typeof(SiteInfo));
        public static int GetGlobalRequestCount()
        {
            SiteInfo siteInfo = new SiteInfo();
            try
            {
                using (FileStream fs = new FileStream(Path.Combine(GetDataPath(), "InfoSite.xml"), FileMode.OpenOrCreate))
                {
                    siteInfo = (SiteInfo)Formatter_SiteInfo.Deserialize(fs);
                }
            }
            catch (Exception)
            {
                return -1;
            }
            siteInfo.RequestCount++;
            try
            {
                using (FileStream fs = new FileStream(Path.Combine(GetDataPath(), "InfoSite.xml"), FileMode.Open))
                {
                    Formatter_SiteInfo.Serialize(fs, siteInfo);
                }
            }
            catch (Exception)
            {
                return -1;
            }
            return siteInfo.RequestCount;
        }
        #endregion

        #region site
        public static bool IsHost = false;
        public static string GetDataPath()
        {
            if (IsHost)
                return hostglobalpath;
            else
                return debugglobalpath;
        }
        public static readonly string hostglobalpath = Path.Combine("//etc//OFiveSite//");
        public static readonly string debugglobalpath = Path.Combine("D://etc//OFiveSite//");
        #endregion
    }
}
