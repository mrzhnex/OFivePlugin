namespace OFivePlugin
{
    public class Player
    {
        public string Nickname { get; set; }
        public int Playerid { get; set; }
        public string SteamId { get; set; }
        public string Role { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public string Output { get; set; }
        public Player(string Nickname, int Playerid, string SteamId, string Role, int Health, int MaxHealth)
        {
            this.Nickname = Nickname;
            this.Playerid = Playerid;
            this.SteamId = SteamId;
            this.Role = Role;
            this.Health = Health;
            this.MaxHealth = MaxHealth;
            Output = Nickname + " id:" + Playerid + " steam:" + SteamId + " role:" + Role + " health:" + Health + "/" + MaxHealth;
        }

        public static Player Server = new Player()
        {
            Health = 0,
            MaxHealth = 0,
            Nickname = "Server",
            Output = "Server id:0 steam: 0 role: Server health: 0/0",
            Playerid = 0,
            Role = "Server",
            SteamId = "0"
        };

        public Player() { }
    }
}