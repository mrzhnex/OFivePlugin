using System.Collections.Generic;

namespace OFivePlugin
{
    public class ServerInfo
    {
        public List<Player> players = new List<Player>();
        public string ServerName { get; set; }
        public int MaxPlayers { get; set; }
        public int CurrentPlayers { get; set; }
        public int CurrentAdmins { get; set; }
        public string Output { get; set; }
        public ServerInfo() { }
    }
}
