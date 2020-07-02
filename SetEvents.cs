using Smod2.EventHandlers;
using Smod2.Events;
using UnityEngine;

namespace OFivePlugin
{
    internal class SetEvents : IEventHandler, IEventHandlerWaitingForPlayers, IEventHandlerRoundRestart, IEventHandlerCallCommand, IEventHandlerRoundStart
    {
        public SetEvents(Smod2.Plugin mainSettings)
        {
            Global.plugin = mainSettings;
        }

        private bool StringIsValid(string str)
        {
            foreach (string s in wrongsymbols)
            {
                if (str.Contains(s))
                    return false;
            }
            return true;
        }

        private readonly System.Collections.Generic.List<string> wrongsymbols = new System.Collections.Generic.List<string>()
        {
            "|",
            "/",
            "*",
            "%",
            "$",
            "#",
            "@",
            "\"",
            "^",
            "<",
            ">",
            "]",
            "[",
            "}",
            "{",
            "~",
            "`"
        };
        
        public void OnCallCommand(PlayerCallCommandEvent ev)
        {
            if (!Global.can_use_commands)
            {
                ev.ReturnMessage = "Дождитесь начала раунда!";
                return;
            }

            if (ev.Command.Split(' ')[0].ToLower() == "request")
            {
                if ((ev.Player.GetGameObject() as GameObject).GetComponent<BlockSpamRequestComponent>() != null)
                {
                    ev.ReturnMessage = "Запрос не отправлен. Пожалуйста, подождите еще " + (ev.Player.GetGameObject() as GameObject).GetComponent<BlockSpamRequestComponent>().timeProgress + " секунд";
                    return;
                }
                int id = Global.GetGlobalRequestCount();
                if (id == -1)
                {
                    (ev.Player.GetGameObject() as GameObject).AddComponent<BlockSpamRequestComponent>();
                    ev.ReturnMessage = "Упс! Что-то пошло явно не по плану...";
                    return;
                }
                string reason = string.Empty;
                for (int i = 0; i < ev.Command.Split(' ').Length; i++)
                {
                    if (i != 0)
                        reason = reason + ev.Command.Split(' ')[i] + " ";
                }
                if (!StringIsValid(reason))
                {
                    ev.ReturnMessage = "Упс! Сообщение имеет некорректные символы!";
                    return;
                }
                if (reason.Length > 1500)
                {
                    ev.ReturnMessage = "Упс! Сообщение слишком длинное!";
                    return;
                }
                Global.Requests.Add(new Request()
                {
                    Id = id,
                    Reason = reason,
                    SenderNickname = ev.Player.Name
                });
                (ev.Player.GetGameObject() as GameObject).AddComponent<BlockSpamRequestComponent>();
                ev.ReturnMessage = "Ваш запрос отправлен. Уникальный номер: " + id;
                return;
            }
        }
        public void OnRoundRestart(RoundRestartEvent ev)
        {
            Global.DeleteServerInfoFile();
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            Global.can_use_commands = true;
        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            Global.can_use_commands = false;
            try
            {
                Global.IsHost = System.Convert.ToBoolean(Global.plugin.GetConfigString("IsHost"));
                Global.plugin.Info("<IsHost> set from config file: " + Global.IsHost.ToString());
            }
            catch (System.FormatException)
            {
                Global.IsHost = true;
                Global.plugin.Info("Failed convert <IsHost> from config file. <IsHost> set to default value: " + Global.IsHost.ToString());
            }
            Global.DeleteServerInfoFile();
            GameObject mainSetter = GameObject.FindWithTag("FemurBreaker");
            mainSetter.AddComponent<TimeComponent>();
        }
    }
}