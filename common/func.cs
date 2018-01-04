using System;
using MySql.Data.MySqlClient;

using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

public class func{
	public static void Debug(bool debug_level,string text){
		if(debug_level == true)
			Console.ForegroundColor = ConsoleColor.Green;
		else
			Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("[Debug]"+text);
		Console.ResetColor();
	}
	/* spaghetti begins here */
	public static int Clamp( int value, int min, int max )
	{
		return (value < min) ? min : (value > max) ? max : value;
	}
}