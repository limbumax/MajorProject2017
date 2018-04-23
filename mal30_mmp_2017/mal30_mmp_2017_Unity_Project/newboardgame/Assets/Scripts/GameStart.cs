using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStart : MonoBehaviour {


    // These OnEnable/Disable get called when the script is turned on/off
    // In this situation, we're just using it to add callback delegates to Unity
    // We remove them on Disable; pretty much like saying "We don't need messages about this anymore (for now)
	//help recieved from Unity Forum : https://forum.unity3d.com
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        // Menu is scene 0 and the board is scene 1. So, this function is called when we load any new scene
        // We just want to check we're on the 'board' not the main menu, and if we are, we want to Make the Game.
        if (arg0.name == "gamescene")
            GameManager.Instance.MakeGame();
    }
}
