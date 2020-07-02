using System.Collections.Generic;

namespace OFivePlugin
{
    public class ReadyCommand
    {
        public ulong SenderDiscordId { get; set; }
        public string CommandName { get; set; }
        public List<string> Args { get; set; }
        public bool IsSuccess { get; set; }
        public string SuccessDebugMessage { get; set; }
        public string FailedDebugMessage { get; set; }
        public ReadyCommand() { }
    }
}
