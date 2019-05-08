using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScore : MonoBehaviour {
	public void onClick() {
		SceneManager.LoadScene("TitleScreen");
	}
	void Start () {
		// Begin calculations
		int size = 3;
		string songname = PlayerPrefs.GetString("song");
		string difficulty = PlayerPrefs.GetString("difficulty");
		int score = PlayerPrefs.GetInt("score");
		string name = PlayerPrefs.GetString("name");
		int[] highscores = new int[size];
		string[] highscore_names = new string[size];
		for (int i = 0; i < size; i++) {
			highscores[i] = PlayerPrefs.GetInt(songname + difficulty + i, 0);
			highscore_names[i] = PlayerPrefs.GetString(songname + difficulty + i + "name", "-");
		}

		if (score > highscores[size-1]) {
			highscores[size-1] = score;
			highscore_names[size-1] = name;
		}
		for (int i = size - 2; i >= 0; i--) {
			if (highscores[i] < highscores[i+1]) {
				// swap
				int temp_score = highscores[i];
				string temp_name = highscore_names[i];
				highscores[i] = highscores[i+1];
				highscore_names[i] = highscore_names[i+1];
				highscores[i+1] = temp_score;
				highscore_names[i+1] = temp_name;
			}
		}
		for (int i = 0; i < size; i++) {
			GameObject.Find("score" + (i+1)).GetComponent<Text>().text = highscores[i].ToString(); 
			GameObject.Find("name" + (i+1)).GetComponent<Text>().text = highscore_names[i]; 
			PlayerPrefs.SetInt(songname + difficulty + i, highscores[i]);
			PlayerPrefs.SetString(songname + difficulty + i + "name", highscore_names[i]);
		}

		int notes_hit = PlayerPrefs.GetInt("notes_hit");
		int notes_missed = PlayerPrefs.GetInt("notes_missed");
		int best_combo = PlayerPrefs.GetInt("max_streak");
		int total_notes = notes_hit + notes_missed;

		GameObject.Find("song_name").GetComponent<Text>().text = songname + " - " + difficulty;
		GameObject.Find("notes_hit").GetComponent<Text>().text = "Notes hit: " + notes_hit + "/" + total_notes + "(" + (100 * notes_hit/total_notes) + "%)";
		GameObject.Find("best_combo").GetComponent<Text>().text = "Best combo: " + best_combo + " notes";
		GameObject.Find("score").GetComponent<Text>().text = "Score: " + score;
	}
}
