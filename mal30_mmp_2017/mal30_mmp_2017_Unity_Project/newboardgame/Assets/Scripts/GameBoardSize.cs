using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameBoardSize : MonoBehaviour {

    // This whole script exists just as a simple way to let a button in the main menu set a variable here.
    // This variable is then used when the game loads to determine the size of the game board.
    public static int BoardSize;
	public static bool def = false;//set our default save to be false so when we load its true
	public void SetGameSize(int size)
    {
        BoardSize = size;
		def = false;
    }

	public void LoadSaved()//run load once the user has clicked load button
	{
		if (File.Exists (Application.persistentDataPath
			+ "/playerInfo.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath
				+ "/playerInfo.dat", FileMode.Open);//open it up if it exists
			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close ();
			BoardSize = data.allinone.GetLength (0);
			def = true;
		}
	}
}
