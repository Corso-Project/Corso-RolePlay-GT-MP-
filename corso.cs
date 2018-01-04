using System;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using MySQL;

using System.Security.Cryptography;
using System.Text;

public class corso_rp : Script{
	public const Int32 MoneyCap = 2147483647; // Max value of money.

    public corso_rp()
    {
        API.onResourceStart += onResourceStart;
		API.onResourceStop += onResourceStop;
    }
    public void onResourceStart(){
        API.consoleOutput("Corso Role Play has been started!");
		API.setGamemodeName("alpha");
    }
	public void onResourceStop()
	{
		API.consoleOutput("Terminating resource...");
		var players = API.shared.getAllPlayers();
		foreach (var player in players)
		{
			Database.Save_Account(player, player.socialClubName);
			player.setData("InGame", 0);
		}
	}
}
