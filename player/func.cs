using System;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

using System.Security.Cryptography;
using System.Text;

namespace player.funcs {
	public class player_function : Script{

		public static void Register(Client player)
		{
			API.shared.triggerClientEvent(player, "Player_Register", player);
		}

		public static void Login(Client player){
			API.shared.triggerClientEvent(player, "Player_Login");
		}

		public static string sha256(string randomString)
		{
			System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
			System.Text.StringBuilder hash = new System.Text.StringBuilder();
			byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString), 0, Encoding.UTF8.GetByteCount(randomString));
			foreach (byte theByte in crypto)
			{
				hash.Append(theByte.ToString("x2"));
			}
			return hash.ToString();
		}

		public static int GetMoney(Client player)
        {
            return (player.hasData("cash_amount")) ? player.getData("cash_amount") : 0;
        }

		public static void ChangeMoney(Client player, int amount){
			if (!player.hasData("cash_amount")) return;
            player.setData("cash_amount", func.Clamp(player.getData("cash_amount") + amount, corso_rp.MoneyCap * -1, corso_rp.MoneyCap));
            API.shared.triggerClientEvent(player, "UpdateMoneyHUD", Convert.ToString(player.getData("cash_amount")), Convert.ToString(amount));
		}
		public static void Spawn_Player(Client player){

		}
	}
}
