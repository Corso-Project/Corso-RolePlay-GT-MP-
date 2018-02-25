using System;
using MySql.Data.MySqlClient;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.IO;
using System.Collections.Generic;

using player.funcs;
using CharCreator;

namespace MySQL {
	public class Database : Script {
		public static string myConnectionString = "SERVER=localhost;" + "DATABASE=corso_rp;" + "UID=root;" + "PASSWORD=;";
		public static MySqlConnection connection;
		public static MySqlCommand command;
		public static MySqlDataReader Reader;

		public Database(){
			Init();
		}

	public static void Init()
	{
		Console.WriteLine("\n[MySQL] Attempt to connect to the database!");
		try
		{
			connection = new MySqlConnection(myConnectionString);
			connection.Open();
			func.Debug(true,"[MySQL] MySQL succesefull connected!\n");
		}
		catch (MySqlException error)
		{
			func.Debug(false,"[MySQL] MySQL error: (#"+error.Number+") "+error.Message+"\n");
		}
	}
		public static void Register_Account(Client player,string Social_ID, string Name, string Password){
			connection = new MySqlConnection(myConnectionString);
			connection.Open();

			int cash = 1954;

			command = connection.CreateCommand();
			command.CommandText = "INSERT INTO accounts (Social_ID, Name, Password, Cash) VALUES (?social_id, ?name, ?password, ?cash)";
			command.Parameters.AddWithValue("?social_id", Social_ID);
			command.Parameters.AddWithValue("?name", Name);
			command.Parameters.AddWithValue("?password", Password);
			command.Parameters.AddWithValue("?cash", cash);

			command.ExecuteNonQuery();
			connection.Close();

			API.shared.setPlayerName(player, Name);

			player.setData("cash_amount", cash);
			API.shared.triggerClientEvent(player, "UpdateMoneyHUD", Convert.ToString(cash));
		}
		public static void Save_Account(Client player, string Social_ID){
			if(player.getData("InGame") == 0) return;

			connection = new MySqlConnection(myConnectionString);
			connection.Open();
			command = connection.CreateCommand();
			command.CommandText = "UPDATE accounts SET Cash = ?cash WHERE Social_ID = ?social_id";
			command.Parameters.AddWithValue("?cash", player.getData("cash_amount"));
			command.Parameters.AddWithValue("?social_id", Social_ID);
			func.Debug(true, "Account "+Social_ID+" has been updated; Cash = "+player.getData("cash_amount"));

			command.ExecuteNonQuery();
			connection.Close();
		}
		public static bool Login_Account(Client player,string inputName, string inputPassword){
			connection = new MySqlConnection(myConnectionString);
			command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM accounts";
			connection.Open();
			Reader = command.ExecuteReader();
			while (Reader.Read())
			{
				string name = Reader.GetString("Name");
				string password = Reader.GetString("Password");
				int cash = Reader.GetInt32("Cash");
				if (inputName == name && inputPassword == password)
				{
					func.Debug(true,"Success: "+name +" | "+password);
					API.shared.setPlayerName(player, name);

					connection.Close();
					player.setData("InGame", 1);
					charcreator_main.LoadCharacter(player);

					API.shared.setEntityPosition(player, new Vector3(-1034.276f, -2734.014f, 20.16927f));
					API.shared.setEntityRotation(player, new Vector3(0, 0, -32.2951f));
					API.shared.setCameraBehindPlayer(player);

					player.setData("cash_amount", cash);
					API.shared.triggerClientEvent(player, "UpdateMoneyHUD", Convert.ToString(cash));
					player.freeze(false);
                    API.shared.setEntityTransparency(player.handle, 255);
					return true;
				}
			}
			player.setData("InGame", 0);
			func.Debug(false, "Failure MySQL");
			API.shared.triggerClientEvent(player, "Wrong_Password");
			connection.Close();
			return false;
		}
		public static bool playerExists(Client player)
		{
			connection = new MySqlConnection(myConnectionString);
			command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM accounts";

			connection.Open();
			Reader = command.ExecuteReader();
			while (Reader.Read())
			{
				string name = Reader.GetString("Social_ID");
                if (name == player.socialClubName.ToString())
				{
		            func.Debug(true, "playerExists");
					Reader.Close();
					connection.Close();
					return true;
				}
			}
			func.Debug(false, "playerExists");
			connection.Close();
			return false;
		}
	}
}
