using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
this class works with the main game manager
it has got turn of player, when game is over, player move etc..
**/
public class ButtonScript : MonoBehaviour {

	public int spot = 0;
	public int jspot = 0;
	public static int turn = 1;//set turn to be one so palyer 1 goes first
	public int longestOrientation;
	List<int[]>longest = new List<int[]>();//create a list to store our longest orientation


	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(Chosen);


	}

	/**
	 *Chosen function to run the player 1 move
	**/

	void Chosen()
	{
		if ((GameManager.Instance.ComputerAI && turn == 2) || GameManager.Instance.GameOver) {
			return;
		}


		if (!GameManager.Instance.IsSpotFree (spot, jspot)) {
			return;
		}
		GameManager.Instance.Played(spot,jspot,turn);

		GetComponent<Image>().color = GameManager.Instance.mPlayerColours[turn];
		turn = (turn == 1 ? 2 : 1);//switch between turns for human players
		if (!GameManager.Instance.GameOver) {// don't get stuck in an infinite loop when the game is not over
			if (GameManager.Instance.ComputerAI) {
				//if computer ai is true from our boolean when the user clicks the play against AI button from menu

				if (GameManager.Instance.AIlevelEasy) {//if easy is true then 
					ComputerPlay ();//run this function
				} else {
					ComputerPlayAdvAI ();//if its false run this
				}
			} 
			else {
				
			}
		}
	}
		


	/**
	 *Chosen function to run the computer easy random  move
	**/

	public void ComputerPlay ()
	{

		while (true) {//while computer is playing
			int r = Random.Range (0, GameManager.Instance.BoardSize);
			if (GameManager.Instance.IsSpotFree (r)) {//cast a randon move on the board 
				turn = 1;//switch turn
				return;
			}
		}

	}


	public static void winner(){
		if (GameManager.Instance.GameOver) {//if game over
			if (GameManager.Instance.Tie) {//and tie

				GameManager.Instance.winnerPlayer (" It's a Tie!");
				//message will display tie
			}
			else if (turn == 1) {//and player 1
				GameManager.Instance.winnerPlayer ("Blue You Win!");
				//message will display blue wins since player one is blue
			}
			else if (turn == 2) {// and player 2/ai
				GameManager.Instance.winnerPlayer ("Green You win!");
				//message will display blue wins since player two/ai is green

			}
		}
	}

	/**
	 *Chosen function to run the computer ai move
	**/
	//problem NOT working AI;

	void ComputerPlayAdvAI ()
	{



		while (true) {
			if (GameManager.Instance.firstTurn) {//if AI's first turn
				int r = Random.Range (0, GameManager.Instance.BoardSize); //put random   
				if (GameManager.Instance.IsSpotFree (r)) {//random so it can work from the postion
					//that it has placed onto the board
					GameManager.Instance.firstTurn = false;//set the first turn false
					print (GameManager.Instance.firstTurn);//so it can go on second one
					turn = 1;//set the turn to 1

					return;
				}
			} else {

				List<int[]> toCheck = updateList (); //run the update list
				foreach (var buttonCoord in toCheck) {//check the button coord

					neighbourcheck (buttonCoord [0], buttonCoord [1]);//check its neighbours
				}

				int[] nextCoord = getNextMove ();//get the next move ///problem occurs here///

				if (GameManager.Instance.IsSpotFree (nextCoord [0], nextCoord [1])) {//if spot is free
					GameManager.Instance.Played (nextCoord [0], nextCoord [1], 2);//place the coord
				}
				turn = 1;//player turn
			}
		}
	}


	public List<int[]> updateList(){
		List<int[]> updated = new List<int[]>();//create a list to store the updated coord of the pieces
		int[] buttonCoord = new int[2];//create array to hold button coords
		for (int i = 0; i < GameManager.Instance.Board.GetLength (0); i++) {//get the size of board
			for (int j = 0; j < GameManager.Instance.Board.GetLength (1); j++) {
				if (GameManager.Instance.Board [i, j] == 2) {//if it is played by AI
					buttonCoord [0] = i;//replace the button coord
					buttonCoord [1] = j;

					updated.Add(buttonCoord);//add the buttoncoord to the list 
				}
			}
		}

		return updated;
	}

	/**
	 * function to check the neighbours for the 
	 * get the next move, it is not working due to not having time to work with it
	 * and the reason might be it has too many while loops and might be the problem
	 * what it was trying to do was to get the all possible moves that the ai can do from the first random move
	 * so it can build it up like dfs it ties to get the starting position of the link and end position in
	 * order to make the next move until it has 5 but have not been able to complete it due to not knowing where
	 * the error is occuring and short of time.
	 * */

	public void neighbourcheck(int x, int y){//check the neighbour or surrounding of the buttons
		List<int[]> temp = new List<int[]> ();//create a new temp list
		int tempX = x;//for replacing coord x of button
		int tempY = y;// for replacing coord y of button
		int[] tempCoord = new int[2];
		int secTempX = x;// same as tempX but for inverse coord
		int secTempY = y;//same as tempY but for inverse coord
		int counter = 1;//set the counter to be 1

		if (legal (tempX, tempY + 1)) {//checks the neighbour if it is a legal move for y when it is up the board
			while (true) {
				if (GameManager.Instance.Board [tempX, tempY + 1] == 2) {//if the board[x][y+1] == AI turn then
					tempCoord [0] = tempX;//replace temp coord with tempX
					tempCoord [1] = tempY;//replace temp coord with tempX
					temp.Add (tempCoord);//add the temp coord to temp list
					counter++;//keep the counter going so we can track until it reach to 5
					tempY++;//keep moving in the Y direction until 5 in a row or is stoped
					if (!legal (tempX, tempY + 1)) {//if not legal move then break
						break;
					}

				}
			}
		}

		if (legal (secTempX, secTempY - 1)) {//same process as first one for when x value go on the inverse side
			//so on for the rest
			while (true) {
				if (GameManager.Instance.Board [secTempX, secTempY - 1] == 2) {
					tempCoord [0] = secTempX;
					tempCoord [1] = secTempY;
					temp.Add (tempCoord);
					counter++;
					secTempY--;
					if (!legal (secTempX, secTempY - 1)) {
						break;
					}

				}

			}
		}
		getMax (counter, temp, 1);//get the max value for vertical
		//*****************************************//
		tempX = x;
		tempY = y;
		secTempX = x;
		secTempY = y;
		temp = new List<int[]> ();
		counter = 1;

		if (legal (tempX + 1, tempY)) {
			while (true) {
				if (GameManager.Instance.Board [tempX + 1, tempY] == 2) {

					tempCoord [0] = tempX;
					tempCoord [1] = tempY;
					temp.Add (tempCoord);//CARE
					counter++;
					tempX++;
					if (!legal (tempX + 1, tempY)) {
						break;
					}

				}
			}
		}

		if (legal (secTempX - 1, secTempY)) {
			while (true) {
				if (GameManager.Instance.Board [secTempX - 1 , secTempY] == 2) {
					tempCoord [0] = secTempX;
					tempCoord [1] = secTempY;
					temp.Add (tempCoord);
					counter++;
					secTempX--;
					if (!legal (secTempX - 1, secTempY)) {
						break;
					}
				}
			}
		}
		getMax (counter, temp, 2);//get the max value for horizontal

		/********************************/
		/*******************************/

		tempX = x;
		tempY = y;
		secTempX = x;
		secTempY = y;
		temp = new List<int[]> ();
		counter = 1;

		if (legal (tempX + 1, tempY + 1)) {
			while (true) {
				if (GameManager.Instance.Board [tempX + 1, tempY + 1] == 2) {
					tempCoord [0] = tempX;
					tempCoord [1] = tempY;
					temp.Add (tempCoord);//CARE
					counter++;
					tempX++;
					tempY++;
					if (!legal (tempX + 1, tempY + 1)) {
						break;
					}
				}
			}
		}

		if (legal (secTempX - 1, secTempY - 1)) {
			while (true) {
				if (GameManager.Instance.Board [secTempX - 1 , secTempY - 1] == 2) {
					tempCoord [0] = secTempX;
					tempCoord [1] = secTempY;
					temp.Add (tempCoord);
					counter++;
					secTempX--;
					secTempY--;
					if (!legal (secTempX - 1, secTempY - 1)) {
						break;
					}
				}
			}
		}
		getMax (counter, temp, 3);//get the max value for diag 1

		/*************************************/
		/**************************************/
		tempX = x;
		tempY = y;
		secTempX = x;
		secTempY = y;
		temp = new List<int[]> ();
		counter = 1;

		if (legal (tempX + 1, tempY - 1)) {
			while (true) {
				if (GameManager.Instance.Board [tempX + 1, tempY - 1] == 2) {

					tempCoord [0] = tempX;
					tempCoord [1] = tempY;
					temp.Add (tempCoord);//CARE
					counter++;
					tempX++;
					tempY--;
					if (!legal (tempX + 1, tempY - 1)) {
						break;
					}

				}
			}
		}

		if (legal (secTempX - 1, secTempY + 1)) {
			while (true) {
				if (GameManager.Instance.Board [secTempX - 1, secTempY + 1] == 2) {
					tempCoord [0] = secTempX;
					tempCoord [1] = secTempY;
					temp.Add (tempCoord);//CARE
					counter++;
					secTempX--;
					secTempY++;
					if (!legal (secTempX - 1, secTempY + 1)) {
						break;
					}

				}

			}
		}
		getMax (counter, temp, 4);//get the max value for diag 2
	}

	/**
	 * function to check legal valid moves
	 * */
	public bool legal(int x, int y) {//get the legal move a player can make

		return ((x >= 0) && (x < GameManager.Instance.Board.GetLength(0)) && (y >= 0) &&
			(y < GameManager.Instance.Board.GetLength (1)));
	}


	/**
	 function to get most highest link from the list 
	 **/
	public void getMax(int counter, List<int[]> temp, int orientation){// get the maximum line from the list

		if (longest.Count < counter) {//if counter is less than the array of the longest list
			longest = new List<int[]> (temp);//
			longestOrientation = orientation;// initialize longestorientation as orientation

		}

	}
	/**
	 get the next move for the AI which builds it in list
	 **/

	public int [] getNextMove(){//get the next move for the AI
		int[] nextMove = new int[2];// create an array called next move that hold value for board position

		if (longestOrientation == 4) {//orientation is diagonal left down or right up
			int headX = -99;//head x is the starting position
			int headY = -99;//head y is the starting position
			int tailX = 99;//tailx and y is the end of the position
			int tailY = 99;

			foreach (var buttonCoord in longest) {//for the buttoncord in longest list

				if (buttonCoord [0] >= headX) {// if the button on [x],y is greater than or equal to headX
					headX = buttonCoord [0];//then switch the headX when teh button coord[x]
					headY = buttonCoord [1];//then switch the headX when teh button coord[y]
				} 

				if (buttonCoord [0] <= tailX) {//if buttoncoord[x]y is less than or equal to tailX
					tailX = buttonCoord [0];//then switch the tailX when teh button coord[x]
					tailY = buttonCoord [1];//then switch the tailY when teh button coord[y]
				}
			}

			if (legal (headX + 1, headY - 1)) {//if it is legal move for headX and headY [x+1][y-1]
				nextMove [0] = headX + 1;//next move is going to be [x+1]
				nextMove [1] = headY - 1;//                         [y-1]
			} else if (legal (tailX - 1, tailY + 1)) {//same process but checks reverse move
				nextMove [0] = tailX - 1;
				nextMove [1] = tailY + 1;        
			}

		}

		/////////////////////////////////////////////////////////////////
		/// //////same process as above but with different orientation///
		/// /////////////////////////////////////////////////////////////
		if (longestOrientation == 3) {//orientation is diagonal left up or right down
			int headX = -99;
			int headY = - 99;
			int tailX = 99;
			int tailY = 99;

			foreach (var buttonCoord in longest) {
				if (buttonCoord [0] >= headX) {
					headX = buttonCoord [0];
					headY = buttonCoord [1];
				} if (buttonCoord [0] <= tailX) {
					tailX = buttonCoord [0];
					tailY = buttonCoord [1];
				}
			}

			if (legal (headX + 1, headY + 1)) {
				nextMove [0] = headX + 1;
				nextMove [1] = headY + 1;
			} else if (legal (tailX - 1, tailY - 1)) {
				nextMove [0] = tailX - 1;
				nextMove [1] = tailY - 1;        
			}

		}

		////////////////////////////////////////////
		/// //////same process as above/////////////
		/// /////////////////////////////////////////
		if (longestOrientation == 2) {//orientation is diagonal left down or right up
			int headX = -99;
			int headY = - 99;
			int tailX = 99;
			int tailY = 99;

			foreach (var buttonCoord in longest) {//orientation is horizontal
				if (buttonCoord [0] >= headX) {
					headX = buttonCoord [0];
					headY = buttonCoord [1];
				} if (buttonCoord [0] <= tailX) {
					tailX = buttonCoord [0];
					tailY = buttonCoord [1];
				}
			}

			if (legal (headX + 1, headY)) {
				nextMove [0] = headX + 1;
				nextMove [1] = headY;
			} else if (legal (tailX - 1, tailY)) {
				nextMove [0] = tailX - 1;
				nextMove [1] = tailY;        
			}

		}
		////////////////////////////////////////////
		/// //////same process as above/////////////
		/// ////////////////////////////////////////
		if (longestOrientation == 1) {//orientation is vertical
			int headX = -99;
			int headY = -99;
			int tailX = 99;
			int tailY = 99;
			foreach (var buttonCoord in longest) {
				if (buttonCoord [1] >= headX) {
					headX = buttonCoord [0];
					headY = buttonCoord [1];
				} if (buttonCoord [1] <= tailX) {
					tailX = buttonCoord [0];
					tailY = buttonCoord [1];
				}
			}

			if (legal (headX, headY + 1)) {
				nextMove [0] = headX;
				nextMove [1] = headY + 1;
			} else if (legal (tailX , tailY - 1)) {
				nextMove [0] = tailX ;
				nextMove [1] = tailY - 1;        
			}

		}

		return nextMove;
	} 
}

