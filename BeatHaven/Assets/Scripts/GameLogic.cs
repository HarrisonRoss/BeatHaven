using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameLogic : MonoBehaviour {

	private GameObject[] note_templates1;
	private GameObject[] hopo_templates1;
	private GameObject[] hold_templates1;
	private GameObject[] tap_templates1;
	private GameObject[] displayednotes1;
	private GameObject[] hitlines1;
	private Fire[] fires1;
	private NoteCollider note_collider1;
	public int score1;
	public int streak1;
	public int max_streak1;
	public int streak_multiplier1;
	public int notes_hit1;
	public int notes_missed1;
	private Text score_text1;
	private Text combo_text1;

	private GameObject[] note_templates2;
	private GameObject[] hopo_templates2;
	private GameObject[] hold_templates2;
	private GameObject[] tap_templates2;
	private GameObject[] displayednotes2;
	private GameObject[] hitlines2;
	private Fire[] fires2;
	private NoteCollider note_collider2;
	public int score2;
	public int streak2;
	public int max_streak2;
	public int streak_multiplier2;
	public int notes_hit2;
	public int notes_missed2;
	private Text score_text2;
	private Text combo_text2;
	private GameObject pause_panel;

	private Material[] light_materials;
	private Material[] dark_materials;
	private string[] colornames_upper;
	private string[] colornames;
	private SortedDictionary<int, NoteData> songnotes;
	private string songname;
	private string difficulty;
	private int counter;
	private int speed;
	private bool paused;
	private bool has_started_playing;

	void Start () {
		// difficulty = "Expert";
		songname = PlayerPrefs.GetString("song");
		difficulty = PlayerPrefs.GetString("difficulty");
		// songname = "Psycho";
		// songname = "Toxic";
		// songname = "Nightclub Amnesia";
		songnotes = ChartInterpreter.Interpret("Assets/Resources/Songs/" + songname + "/notes.chart", difficulty);
		string song_filename = "Songs/" + songname + "/song";
		GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(song_filename);;

		colornames_upper = new string[5]{"Green", "Red", "Yellow", "Blue", "Orange"};
		colornames = new string[5]{"green", "red", "yellow", "blue", "orange"};
		note_templates1 = new GameObject[5];
		hopo_templates1 = new GameObject[5];
		hold_templates1 = new GameObject[5];
		tap_templates1 = new GameObject[5];
		hitlines1 = new GameObject[5];
		fires1 = new Fire[5];
		note_templates2 = new GameObject[5];
		hopo_templates2 = new GameObject[5];
		hold_templates2 = new GameObject[5];
		tap_templates2 = new GameObject[5];
		hitlines2 = new GameObject[5];
		fires2 = new Fire[5];

		light_materials = new Material[5];
		dark_materials = new Material[5];
		for (int color = 0; color < 5; color++) {
			string colorname = colornames[color];
			note_templates1[color] = GameObject.Find(colorname + "_template1");
			hopo_templates1[color] = GameObject.Find("hopo_" + colorname + "_template1");
			hold_templates1[color] = GameObject.Find("hold_" + colorname + "_template1");
			fires1[color] = GameObject.Find("fire_" + colorname + "1").GetComponent<Fire>();
			tap_templates1[color] = GameObject.Find("tap_" + colorname + "_template1");
			hitlines1[color] = GameObject.Find("hitline_" + colorname + "1");
			note_templates2[color] = GameObject.Find(colorname + "_template2");
			hopo_templates2[color] = GameObject.Find("hopo_" + colorname + "_template2");
			hold_templates2[color] = GameObject.Find("hold_" + colorname + "_template2");
			fires2[color] = GameObject.Find("fire_" + colorname + "2").GetComponent<Fire>();
			tap_templates2[color] = GameObject.Find("tap_" + colorname + "_template2");
			hitlines2[color] = GameObject.Find("hitline_" + colorname + "2");

			light_materials[color] = hold_templates1[color].GetComponent<Renderer>().material;
			dark_materials[color] = hitlines1[color].GetComponent<Renderer>().material;
			note_templates1[color].GetComponent<MeshRenderer>().enabled = false;
			hopo_templates1[color].GetComponent<MeshRenderer>().enabled = false;
			hold_templates1[color].GetComponent<MeshRenderer>().enabled = false;
			tap_templates1[color].GetComponent<MeshRenderer>().enabled = false;
			note_templates2[color].GetComponent<MeshRenderer>().enabled = false;
			hopo_templates2[color].GetComponent<MeshRenderer>().enabled = false;
			hold_templates2[color].GetComponent<MeshRenderer>().enabled = false;
			tap_templates2[color].GetComponent<MeshRenderer>().enabled = false;
		}

		note_collider1 = GameObject.Find("collider1").GetComponent<NoteCollider>();
		score_text1 = GameObject.Find("score_1").GetComponent<Text>();
		combo_text1 = GameObject.Find("combo_1").GetComponent<Text>();
		note_collider2 = GameObject.Find("collider2").GetComponent<NoteCollider>();
		score_text2 = GameObject.Find("score_2").GetComponent<Text>();
		combo_text2 = GameObject.Find("combo_2").GetComponent<Text>();
		pause_panel = GameObject.Find("PausePanel");
		pause_panel.SetActive(false);

		score1 = 0;
		max_streak1 = 0;
		streak1 = 0;
		streak_multiplier1 = 1;
		notes_hit1 = 0;
		notes_missed1 = 0;
		score2 = 0;
		max_streak2 = 0;
		streak2 = 0;
		streak_multiplier2 = 1;
		notes_hit2 = 0;
		notes_missed2 = 0;
		speed = 12;
		counter = 0;
		paused = false;
		has_started_playing = false;
		displayednotes1 = new GameObject[0];
		displayednotes2 = new GameObject[0];
	}
	void FixedUpdate() {
		if (paused) {
			return;
		}
		if (counter == 840 / speed) {
			GetComponent<AudioSource>().Play();
			has_started_playing = true;
		}
		if (songnotes.ContainsKey(counter)) {
			for (int color = 0; color < 5; color++) {
				NoteData note = songnotes[counter];
				if (!note.notes[color]) {
					continue;
				}
				GameObject note_template1;
				GameObject note_template2;
				if (note.tap) {
					note_template1 = tap_templates1[color];
					note_template2 = tap_templates2[color];
				}
				else if (note.hopo) {
					note_template1 = hopo_templates1[color];
					note_template2 = hopo_templates2[color];
				}
				else {
					note_template1 = note_templates1[color];
					note_template2 = note_templates2[color];
				}
				Vector3 pos1 = note_template1.transform.position;
				Vector3 pos2 = note_template2.transform.position;
				Quaternion rot1 = note_template1.transform.rotation;
				Quaternion rot2 = note_template2.transform.rotation;
				GameObject newnote1 = Instantiate(note_template1, pos1, rot1);
				GameObject newnote2 = Instantiate(note_template2, pos2, rot2);
				newnote1.GetComponent<NoteNum>().num = counter;
				newnote2.GetComponent<NoteNum>().num = counter;
				newnote1.GetComponent<MeshRenderer>().enabled = true;
				newnote2.GetComponent<MeshRenderer>().enabled = true;

				// // Hold notes currently buggy
				// if (note.duration != 0) {
				// 	GameObject hold_template1 = hold_templates1[color];
				// 	Vector3 hold_pos1 = hold_template1.transform.position;
				// 	Quaternion hold_rot1 = hold_template1.transform.rotation;
				// 	GameObject hold_object1 = Instantiate(hold_template1, hold_pos1, hold_rot1);

				// 	GameObject hold_template2 = hold_templates2[color];
				// 	Vector3 hold_pos2 = hold_template2.transform.position;
				// 	Quaternion hold_rot2 = hold_template2.transform.rotation;
				// 	GameObject hold_object2 = Instantiate(hold_template2, hold_pos2, hold_rot2);

				// 	hold_object1.transform.parent = newnote1.transform;
				// 	hold_object2.transform.parent = newnote2.transform;
				// 	float oldscale = hold_object1.transform.localScale.y;
				// 	float newscale = note.duration * speed / 60 / 2;
				// 	hold_object1.transform.localScale = Vector3.Scale(hold_object1.transform.localScale, new Vector3(1, newscale, 1));
				// 	hold_object1.transform.Translate(new Vector3(0, newscale - oldscale - 0.5f, 0));
				// 	hold_object1.GetComponent<MeshRenderer>().enabled = true;
				// 	hold_object2.transform.localScale = Vector3.Scale(hold_object2.transform.localScale, new Vector3(1, newscale, 1));
				// 	hold_object2.transform.Translate(new Vector3(0, newscale - oldscale - 0.5f, 0));
				// 	hold_object2.GetComponent<MeshRenderer>().enabled = true;
				// }

				// Extend displayednotes1 array
				GameObject[] temp1 = new GameObject[displayednotes1.Length + 1];
				for (int i = 0; i < displayednotes1.Length; i++) {
					temp1[i] = displayednotes1[i];
				}
				temp1[displayednotes1.Length] = newnote1;
				displayednotes1 = temp1;

				GameObject[] temp2 = new GameObject[displayednotes2.Length + 1];
				for (int i = 0; i < displayednotes2.Length; i++) {
					temp2[i] = displayednotes2[i];
				}
				temp2[displayednotes2.Length] = newnote2;
				displayednotes2 = temp2;
			}
		}
		counter++;
		if (has_started_playing && !GetComponent<AudioSource>().isPlaying) {
			// Game over
			PlayerPrefs.SetInt("score", score1);
			PlayerPrefs.SetInt("notes_hit", notes_hit1);
			PlayerPrefs.SetInt("notes_missed", notes_missed1);
			PlayerPrefs.SetInt("max_streak", max_streak1);
			SceneManager.LoadScene("HighScore");
		}
	}
	private static GameObject[] RemoveNulls(GameObject[] arr) {
		int newlength = 0;
		for (int i = 0; i < arr.Length; i++) {
			if (arr[i] != null) {
				newlength++;
			}
		}
		GameObject[] temp = new GameObject[newlength];
		int index = 0;
		for (int i = 0; i < arr.Length; i++) {
			if (arr[i] != null) {
				temp[index] = arr[i];
				index++;
			}
		}
		return temp;
	}
	void Update() {
		if (Input.GetButtonDown("Pause")) {
			paused = !paused;
			if (paused) {
				GetComponent<AudioSource>().Pause();
				pause_panel.SetActive(true);
			}
			else {
				GetComponent<AudioSource>().Play();
				pause_panel.SetActive(false);
			}
		}
		if (paused) {
			return;
		}
		max_streak1 = Mathf.Max(streak1, max_streak1);
		int new_notes_missed1 = note_collider1.notes_missed;
		if (new_notes_missed1 > notes_missed1) {
			notes_missed1 = new_notes_missed1;
			streak1 = 0;
			streak_multiplier1 = 1;
		}
		max_streak2 = Mathf.Max(streak2, max_streak2);
		int new_notes_missed2 = note_collider2.notes_missed;
		if (new_notes_missed2 > notes_missed2) {
			notes_missed2 = new_notes_missed2;
			streak2 = 0;
			streak_multiplier2 = 1;
		}
		displayednotes1 = RemoveNulls(displayednotes1);
		displayednotes2 = RemoveNulls(displayednotes2);
		for (int i = 0; i < displayednotes1.Length; i++) {
			displayednotes1[i].transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
		}
		for (int i = 0; i < displayednotes2.Length; i++) {
			displayednotes2[i].transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
		}

		bool[] notes_held = new bool[5];
		int num_notes_held = 0;
		for (int color = 0; color < 5; color++) {
			notes_held[color] = Input.GetButton(colornames_upper[color]);
			if (notes_held[color]) {
				num_notes_held++;
				hitlines1[color].GetComponent<Renderer>().material = light_materials[color];
			}
			else {
				hitlines1[color].GetComponent<Renderer>().material = dark_materials[color];
			}
		}
		if (num_notes_held > 0) {
			bool strum = Input.GetButtonDown("Strum Up") || Input.GetButtonDown("Strum Down");
			if (note_collider1.notes.Length == 0 && strum) {
				streak1 = 0;
				streak_multiplier1 = 1;
			}
			// Hopo
			if (note_collider1.notes.Length > 0) {
				NoteData note = songnotes[note_collider1.notes[0].GetComponent<NoteNum>().num];
				if (strum || note.tap || (note.hopo && streak1 > 0)) {
					// Complex logic here to allow for HOPO
					// Basically hits are acceptable if they include (or don't include) notes below it RTL
					// I.e. once you've hit a note, the only criteria is that you hit the notes below
					bool hit = true;
					bool empty_half = true;
					for (int color = 4; color >= 0; color--) {
						if (note.notes[color] && !notes_held[color]) {
							hit = false;
							break;
						}
						if (empty_half) {
							if (note.notes[color] && notes_held[color]) {
								empty_half = false;
							}
							if (!note.notes[color] && notes_held[color]) {
								hit = false;
								break;
							}
						}
					}
					if (hit) {
						notes_hit1 += note.num_notes;
						streak1 += note.num_notes;
						streak_multiplier1 = 1 + Mathf.Min(streak1 / 10, 3);
						score1 += note.num_notes * 50 * streak_multiplier1;
						for (int color = 0; color < 5; color++) {
							if (note.notes[color]) {
								fires1[color].Hit();
							}
						}
						for (int i = 0; i < note.num_notes; i++) {
							Destroy(note_collider1.notes[i]);
						}
					}
					if (!hit && strum) {
						streak1 = 0;
						streak_multiplier1 = 1;
					}
				}
			}
		}

		// AI Player 2
		// Basically just hit with random chance

		for (int color = 0; color < 5; color++) {
			hitlines2[color].GetComponent<Renderer>().material = dark_materials[color];
		}
		if (note_collider2.notes.Length > 0) {
			int note_index = 0;
			// note_collider2.notes = RemoveNulls(note_collider2.notes);
			// while (note_index < note_collider2.notes.Length && note_collider2.notes[note_index].GetComponent<NoteNum>().missed) {
			// 	note_index++;
			// }
			if (note_index < note_collider2.notes.Length) {
				NoteData note = songnotes[note_collider2.notes[note_index].GetComponent<NoteNum>().num];
				bool hit = Random.value < 0.8f;
				if (hit) {
					notes_hit2 += note.num_notes;
					streak2 += note.num_notes;
					streak_multiplier2 = 1 + Mathf.Min(streak2 / 10, 3);
					score2 += note.num_notes * 50 * streak_multiplier2;
					for (int color = 0; color < 5; color++) {
						if (note.notes[color]) {
							fires2[color].Hit();
							hitlines2[color].GetComponent<Renderer>().material = light_materials[color];
						}
					}
				}
				else {
					streak2 = 0;
					streak_multiplier2 = 1;
					// for (int i = note_index; i < note_index+note.num_notes; i++) {
					// 	note_collider2.notes[i].GetComponent<NoteNum>().missed = true;
					// }
				}
				for (int i = note_index; i < note_index+note.num_notes; i++) {
					Destroy(note_collider2.notes[i]);
				}
			}
		}

		score_text1.text = "Score: " + score1;
		combo_text1.text = "Combo: " + streak1;
		score_text2.text = "Score: " + score2;
		combo_text2.text = "Combo: " + streak2;
	}
}
