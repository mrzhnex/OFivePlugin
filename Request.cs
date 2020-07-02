namespace OFivePlugin
{
    public class Request
    {
        public int Id { get; set; }
        public string SenderNickname { get; set; }
        public string Reason { get; set; }
        public ulong MessageId { get; set; }
        public string ServerName { get; set; }
        public string DiscordMessage { get; set; }
        public Request() { }
    }
}
