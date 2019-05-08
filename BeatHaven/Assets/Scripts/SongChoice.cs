using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SongChoice : MonoBehaviour {
	public GameObject song1;
	public GameObject song2;
	public GameObject song3;
	public string songChoice;
	public string difficulty;
	public GameObject songPanel;
	public GameObject difficultyPanel;

	private string[] fileEntries;

	void Start () {
		fileEntries = Directory.GetDirectories("Assets/Resources/Songs");
		song1.GetComponentInChildren<Text>().text = fileEntries[2].Substring(23);
		song2.GetComponentInChildren<Text>().text = fileEntries[1].Substring(23);
		song3.GetComponentInChildren<Text>().text = fileEntries[0].Substring(23);
	}

	public void onClick(string songtitle) {
		PlayerPrefs.SetString("song", songtitle);
		if(songtitle == "Nightclub Amnesia"){
			songPanel.SetActive(false);
			difficultyPanel.SetActive(true);
		}
		else {
			PlayerPrefs.SetString("difficulty", "Expert");
			SceneManager.LoadScene("Game");
		}
	}
}
