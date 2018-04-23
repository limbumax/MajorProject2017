using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changetoai : MonoBehaviour {
	public static bool ComputerAI;

	public void onClick(){//when user clicks the AI
		ComputerAI = true;//set AI to true so computer can play
	}

	public void onClickHumanFalse(){//when they choose to play against player
		ComputerAI = false;//set AI to false so user can play against another person
	}
//		
}
