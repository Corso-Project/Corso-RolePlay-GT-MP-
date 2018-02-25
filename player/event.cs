using System;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

using player.funcs;
using MySQL;
using CharCreator;

using System.Security.Cryptography;
using System.Text;

public class player_event : Script{
	public player_event()
    {
		API.onPlayerConnected += OnPlayerConnectedHandler;
		API.onPlayerDisconnected += OnPlayerDisconnectedHandler;
		API.onClientEventTrigger += OnClientEventTrigger;
		API.onPlayerFinishedDownload += OnPlayerFinishedDownload;
    }

	public static readonly Vector3 _startPos = new Vector3(3433.339f, 5177.579f, 39.79541f);
	public static readonly Vector3 _startCamPos = new Vector3(3476.85f, 5228.022f, 9.453369f);

	public void OnPlayerFinishedDownload(Client player){
		API.shared.setEntityTransparency(player.handle, 0);
		if(Database.playerExists(player) == true)
			player_function.Login(player);
		else if (Database.playerExists(player) == false)
			player_function.Register(player);
	}
	public void OnPlayerConnectedHandler(Client player){
		player.setData("InGame", 0);

		API.triggerClientEvent(player, "interpolateCamera", 20000, _startCamPos, _startCamPos + new Vector3(0.0, -50.0, 50.0), new Vector3(0.0, 0.0, 180.0), new Vector3(0.0, 0.0, 95.0));
		player.position = _startPos;
		player.freeze(true);

		API.sendChatMessageToPlayer(player, "Welcome to Corso RolePlay!");
	}
	public void OnPlayerDisconnectedHandler(Client player, string reason)
	{
		Database.Save_Account(player, player.socialClubName);
		player.setData("InGame", 0);
	}
	public void OnClientEventTrigger(Client player, string eventName, params object[] arguments)
	{
		if(eventName == "User_Login")
		{
			player.setData("InGame", 1);

			var name = arguments[0].ToString();
			var password = arguments[1].ToString();

			Database.Login_Account(player, name, player_function.sha256(password));
			//API.shared.setEntityTransparency(player.handle, 255);
		}
		if(eventName == "User_Register"){
			var name = arguments[0].ToString();
			var password = arguments[1].ToString();

            if(name.Length <= 6 || password.Length <= 5){
                player_function.Register(player);
				func.Debug(true,"test");
				player.sendNotification("Server", "Nickname and password length is at least 5 characters!",true);
				return;
			}
			Database.Register_Account(player, player.socialClubName, name, player_function.sha256(password));
			charcreator_main.SendToCreator(player);
		}
	}
}
