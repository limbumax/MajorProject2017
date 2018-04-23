using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/**
 * Main class which works with buttonscript class
 * creates board, checks winner, save, load
 * When Board[row,col] == 0 it means the space has not been picked and its free
 * when its 1 it means it has picked by player 1 and they have played on it
 * when its 2 it means it has picked by player 2/ai and they have played on it
 **/
public class GameManager : MonoBehaviour {

	[SerializeField]
	Button mButtonPrefab;
	[SerializeField]
	RectTransform mPanel;
	public int[,] Board;//create a 2 array for the board
	public Text winText;//create a text which can be used
	//to display the message later assign it with UI text in unity editor
	int turn;
	public static GameManager Instance;

	public Color[] mPlayerColours = new Color[] { Color.white, Color.blue, Color.green };
	public bool ComputerAI = false; // computer AI set to false because we dont want it to be true everytime the game is ran
	public bool AIlevelEasy = true; // AI level set to true so we can use this later to determine the level
	int SpotsPlayed = 0;//counter for the spaces at the beginning its set to 0 because the game has not been played yet
	public int BoardSize;
	int colsize = 0;
	public bool GameOver = false;//set the game over to false because we want it to be true only when the game is over
	public bool Tie = false;//set the game over to false because we want it to be true only when the game is a tie
	public bool firstTurn = true;//we want the first turn to be true for Adv AI so it can make random move
	public int savedBoardSize;//save the board size for load option
	public bool getsize =false;



	private void Awake()
	{
		Instance = this;//instance the gamemanager class

	}


	/**
function to make the game in general e.g. board size etc
	**/
	public void MakeGame()
	{
		// The first variable takes the size of the board that we set from the script in the main menu
		int size = GameBoardSize.BoardSize;
		BoardSize = size*size;//make the board size according to selection
		//getsize = false;
		ComputerAI = changetoai.ComputerAI;//assign the computerAI bool to changetoai script so we can make changes when switching 
		AIlevelEasy = changeailevel.AIlevelEasy;
		GameOver = false;//set the gameover to be false every time we play a new game
		ButtonScript.turn = 1;//we want the turn to be one everytime we play a new game

		mPanel.GetComponent<GridLayoutGroup>().constraintCount = size;
		SpotsPlayed = 0;//we want the spaces that has been played to 
		Board = new int[size, size];//create the size of the board according to want the player has picked

		colsize = size;
		for (int i = 0; i < size; ++i)
		{
			for (int j = 0; j < size; ++j)
			{//create our board full of UI buttons which has row and column
				Button b = Instantiate(mButtonPrefab);
				ButtonScript bs = b.GetComponent<ButtonScript>();
				bs.spot = i;
				bs.jspot = j;
				b.transform.SetParent(mPanel);
			}
		}
		if (GameBoardSize.def == true) {
			//			//int size = GameBoardSize.BoardSize;
			Load ();
		} 
	}



	/**
	 function to check if space is free in the board
	 **/
	public bool IsSpotFree(int spot, int jspot)
	{
		if (SpotsPlayed == BoardSize) return false;//or return false when it is full or not been taken
		if (Board[spot,jspot] == 0) return true;//checks if spot is free so that it can return true when its free 
		return false;
	}




	/**
	 function to check if space is free in the board for AI so it can do random moves
	 **/

	public bool IsSpotFree(int spot)
	{//for AI so that it can move randomly when it will be called

		int i = spot / colsize;
		int j = spot % colsize;

		if (SpotsPlayed == BoardSize) return false;
		if (Board[i, j] == 0)
		{
			Played(i, j, 2); // always computer

			return true;
		}
		return false;
	}




	public void Played(int spot, int jspot, int played)
	{
		//print("Played : " + spot + " + " + jspot);
		Board[spot,jspot] = played;
		if (played == 2)
			mPanel.GetChild(spot*(int)Mathf.Sqrt(BoardSize)+jspot).GetComponent<Image>().color = mPlayerColours[2];
		SpotsPlayed++;
		CheckWon();
		ButtonScript.winner ();

	}


	/**
	 this is the function to place the already owned spot when load function is used
	 **/

	public void Played2(int spot, int jspot, int played, int bsize) // bsize is board size
	{
		mPanel.GetChild(spot*bsize+jspot).GetComponent<Image>().color = mPlayerColours[played];
		SpotsPlayed++;
	}




	/**
	 display out our winner using UI text after the game has ended
	 Help recieved from:
	 https://forum.unity3d.com/threads/unity-text-implementation.467706/#post-3044056
	 **/


	public void winnerPlayer(string message){
		winText.text = message;

	}


	/**
	 function to check if there is any 5 line in a row in the board
idea taken from source: http://stackoverflow.com/questions/32770321/connect-4-check-for-a-win-algorithm
	 * */
	void CheckWon ()
	{

		//check for horizontal
		for(int x = 0; x < Board.GetLength(0)-4;x++) {//scans the length of the board
			for(int y = 0; y < Board.GetLength (1);y++) {
				if (Board[x ,y ] != 0 &&
					Board[x, y] == Board [x + 1, y ] 
					&&  Board[x, y] ==  Board[x + 2, y ] 
					&& Board[x, y] ==  Board[x + 3, y ] 
					&&  Board[x, y] == Board[x + 4, y ] ) 
				{
					GameOver = true;//if 5 in a row then set gamover to be true
				}
			}


		}	
		//check for vertical
		for (int x = 0; x < Board.GetLength (0); x++) {
			for (int y = 0; y < Board.GetLength (1)-4; y++) {
				if (Board [x, y] != 0 &&
					Board [x, y] == Board [x, y + 1]
					&& Board [x, y] == Board [x, y + 2]
					&& Board [x, y] == Board [x, y + 3]
					&& Board [x, y] == Board [x, y + 4]) {
					GameOver = true;
				}
			}
		}

		//check for diagonal 
		for (int x = 4; x < Board.GetLength (0); x++) {
			for (int y = 0; y < Board.GetLength (1) -4; y++) {
				if (Board [x, y] != 0 &&
					Board [x, y ] == Board [x - 1, y + 1]
					&& Board [x, y ] == Board [x - 2, y + 2]
					&& Board [x, y ] == Board [x - 3, y + 3]
					&& Board [x, y ] == Board [x - 4, y + 4]) {
					GameOver = true;
				}
			}
		}

		//check for diagonal

		for (int x = 4; x < Board.GetLength (0); x++) {
			for (int y = 4; y < Board.GetLength (1); y++) {
				if (Board [x, y] != 0 &&
					Board [x, y ] == Board [x - 1, y - 1]
					&& Board [x, y ] == Board [x - 2, y - 2]
					&& Board [x, y ] == Board [x - 3, y - 3]
					&& Board [x, y ] == Board [x - 4, y - 4]) {
					GameOver = true;
				}
			}
		}

		//check for the tie
		if (SpotsPlayed == BoardSize ) {//if our spotplayed couter is same as our board
			Tie = true;//set tie to true
			GameOver = true;//set gameover to true

		}

	}


	/**
Save function to save the game
Idea taken from Unity:
https://unity3d.com/learn/tutorials/topics/scripting/persistence-saving-and-loading-data

	**/
	public void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath
			+ "/playerInfo.dat");//create a file called playerInfo.dat to save the position
		{

			PlayerData data = new PlayerData ();//assign is as playerdata class as data
			data.allinone = Board;//save the board
			//	allinone = GameManager.Instance.Board;
			data.size = GameBoardSize.BoardSize;//save size of board
			data.turn = ButtonScript.turn;//save turn
			bf.Serialize (file, data);
			file.Close ();//close the file
		}
	}


/**
load function to load the game
Idea taken from Unity:
https://unity3d.com/learn/tutorials/topics/scripting/persistence-saving-and-loading-data

**/
	public void Load(){

		if (File.Exists (Application.persistentDataPath
			+ "/playerInfo.dat")) {//check if file called playerInfo exists
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath
				+ "/playerInfo.dat", FileMode.Open);//open it up if it exists
			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close ();
			savedBoardSize = data.allinone.GetLength(0);
			Board = data.allinone;
			//place the value in according to the last saved state
			for (int k = 0; k < data.allinone.GetLength (0); k++) {
				for (int l = 0; l < data.allinone.GetLength (1); l++) {
					int p = data.allinone [k, l];
					if (p == 0)
						continue;
					Played2(k, l, p,savedBoardSize);

				}
			}
			ButtonScript.turn = data.turn;//assign the last saved turn
		}
	}



}


[Serializable]

/**
make a new class with serializable to help make sure we can save and load data
Idea taken from Unity:
https://unity3d.com/learn/tutorials/topics/scripting/persistence-saving-and-loading-data

**/
class PlayerData{

	public int[,] allinone;// have our 2d array than stores the move from board when saving
	public int x = 0;//our row and column is set to 0 as default of not having any position
	public int y = 0;
	public int turn;//create a turn variable so we can use it to save the turn
	public int size;

}
