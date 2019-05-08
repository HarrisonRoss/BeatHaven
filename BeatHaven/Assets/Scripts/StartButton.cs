using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {
	public GameObject startPanel;
	public GameObject songPanel;
	public GameObject difficultyPanel;
	public GameObject instructionsPanel;
	public InputField nickname;

	void Start(){
		startPanel.SetActive(true);
		songPanel.SetActive(false);
		difficultyPanel.SetActive(false);
		instructionsPanel.SetActive(false);

	}

	public void onClick(string mode) {
		// TODO: Use mode to determine singleplayer/multiplayer
		PlayerPrefs.SetString("name", nickname.text);
		startPanel.SetActive(false);
		if(mode == "song"){
			songPanel.SetActive(true);
		}
		else{
			instructionsPanel.SetActive(true);
		}
		// SceneManager.LoadScene("Game");
	}

}
