using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeailevel : MonoBehaviour {

	public static bool AIlevelEasy;
	/**
same process as changing between human and ai opponent
	**/
		public void onClickEasy(){
			AIlevelEasy = true;//when ai easy clicked set to true so ai easy can run
			
		}
		public void onClickAdv(){
			AIlevelEasy = false;// when adv ai click set easy to false so it can run the ai adv function
			
		}
		//		
	}
	
