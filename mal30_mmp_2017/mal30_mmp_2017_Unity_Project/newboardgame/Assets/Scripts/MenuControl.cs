using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour{
	/**
A function to call the scene 
Used unity online documentation to do this
https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
	**/
	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
