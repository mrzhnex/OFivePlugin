using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Smod2.API;

namespace OFivePlugin
{
    public class TimeComponent : MonoBehaviour
    {
        private float timer = 0.0f;
        public static readonly float timeIsUp = 2.0f;
        private string answer = string.Empty;
        public void Update()
        {
            timer += Time.deltaTime;

            if (timer > timeIsUp)
            {
                timer = 0f;
                
                Global.SaveServerInfo();

                List<ReadyCommand> commands = Global.LoadReadyCommands();
                foreach (ReadyCommand command in commands)
                {
                    if (command.CommandName.ToLower() == "ban")
                    {
                        if (Global.plugin.Server.GetPlayer(int.Parse(command.Args[0])) != null)
                        {
                            Global.plugin.Server.GetPlayer(int.Parse(command.Args[0])).Ban(int.Parse(command.Args[1]), command.Args[2]);
                        }
                        else
                        {
                            command.IsSuccess = false;
                        }
                    }
                    else if (command.CommandName.ToLower() == "oban")
                    {
                        if (Global.plugin.Server.GetPlayers().Where(x => x.SteamId == command.Args[1]).FirstOrDefault() != default)
                        {
                            Global.plugin.Server.GetPlayers().Where(x => x.SteamId == command.Args[1]).FirstOrDefault().Ban(int.Parse(command.Args[2]), command.Args[3]);
                        }
                        else
                        {
                            command.IsSuccess = false;
                            BanDetails banDetails = new BanDetails()
                            {
                                Expires = DateTime.UtcNow.AddMinutes((double)int.Parse(command.Args[2])).Ticks,
                                Id = command.Args[1],
                                IssuanceTime = TimeBehaviour.CurrentTimestamp(),
                                OriginalName = command.Args[0],
                                Issuer = command.SenderDiscordId.ToString(),
                                Reason = command.Args[3]
                            };
                            BanHandler.IssueBan(banDetails, BanHandler.BanType.Steam);
                        }
                    }
                    else if (command.CommandName.ToLower() == "unban")
                    {
                        if (BanHandler.GetBans(BanHandler.BanType.Steam).Where(x => x.Id == command.Args[0]).FirstOrDefault() != default)
                        {
                            command.SuccessDebugMessage = command.SuccessDebugMessage.Replace("(Nickname)", BanHandler.GetBans(BanHandler.BanType.Steam).Where(x => x.Id == command.Args[0]).FirstOrDefault().OriginalName);
                            command.SuccessDebugMessage = command.SuccessDebugMessage.Replace("(Reason)", BanHandler.GetBans(BanHandler.BanType.Steam).Where(x => x.Id == command.Args[0]).FirstOrDefault().Reason);
                            BanHandler.RemoveBan(command.Args[0], BanHandler.BanType.Steam);
                        }
                        else
                        {
                            command.IsSuccess = false;
                        }
                    }
                    else if (command.CommandName.ToLower() == "bc")
                    {
                        Global.plugin.Server.Map.ClearBroadcasts();
                        Global.plugin.Server.Map.Broadcast(uint.Parse(command.Args[0]), command.Args[1], true);
                    }
                    else if (command.CommandName.ToLower() == "pbc")
                    {
                        if (Global.plugin.Server.GetPlayer(int.Parse(command.Args[0])) != null)
                        {
                            Global.plugin.Server.GetPlayer(int.Parse(command.Args[0])).PersonalClearBroadcasts();
                            Global.plugin.Server.GetPlayer(int.Parse(command.Args[0])).PersonalBroadcast(uint.Parse(command.Args[1]), command.Args[2], true);
                        }
                        else
                        {
                            command.IsSuccess = false;
                        }
                    }
                    else if (command.CommandName.ToLower() == "tbc")
                    {
                        foreach (Smod2.API.Player player in Global.plugin.Server.GetPlayers())
                        {
                            if (player.TeamRole.Team.ToString() == command.Args[0])
                            {
                                player.PersonalClearBroadcasts();
                                player.PersonalBroadcast(uint.Parse(command.Args[1]), command.Args[2], true);
                            }
                        }
                    }
                    else if (command.CommandName.ToLower() == "dbc")
                    {

                    }
                    else if (command.CommandName.ToLower() == "sbc")
                    {

                    }
                    else if (command.CommandName.ToLower() == "slay")
                    {
                        if (Global.plugin.Server.GetPlayer(int.Parse(command.Args[0])) != null)
                        {
                            Global.plugin.Server.GetPlayer(int.Parse(command.Args[0])).Kill((Smod2.API.DamageType)System.Enum.Parse(typeof(Smod2.API.DamageType), command.Args[1]));
                        }
                        else
                        {
                            command.IsSuccess = false;
                        }
                    }
                    else
                    {
                        Global.plugin.Info("UNKNOWN COMMAND FROM SITE!");
                    }
                    answer = string.Empty;
                    foreach (string s in command.Args)
                    {
                        answer = answer + s + " ";
                    }
                    Global.plugin.Info("Execute command. Sender(DiscordId): " + command.SenderDiscordId + ", command: " + command.CommandName + ", args: " + answer);                   
                }
                Global.SaveReadyCommands(commands);
                Global.SaveRequests();
            }
        }
    }
}
