using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongDifficulty : MonoBehaviour {
	public GameObject difficulty1;
	public GameObject difficulty2;
	public GameObject difficulty3;
	public GameObject difficulty4;

	void Start () {
		difficulty1.GetComponentInChildren<Text>().text = "Easy";
		difficulty2.GetComponentInChildren<Text>().text = "Medium";
		difficulty3.GetComponentInChildren<Text>().text = "Hard";
		difficulty4.GetComponentInChildren<Text>().text = "Expert";	
	}

	public void onClick(string difficulty) {
		PlayerPrefs.SetString("difficulty", difficulty);
		SceneManager.LoadScene("Game");
	}
}
