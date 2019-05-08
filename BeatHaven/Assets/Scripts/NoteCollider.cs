using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCollider : MonoBehaviour {
	public GameObject[] notes;
	public int notes_missed;
	void Start() {
		notes_missed = 0;
	}
	void OnTriggerEnter(Collider collider) {
		GameObject note = collider.gameObject;
		// Extend array
		GameObject[] temp = new GameObject[notes.Length + 1];
		for (int i = 0; i < notes.Length; i++) {
			temp[i] = notes[i];
		}
		temp[notes.Length] = note;
		notes = temp;
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
	void OnTriggerExit(Collider collider) {
		Destroy(collider.gameObject);
		notes_missed++;
	}
	void Update() {
		notes = RemoveNulls(notes);
	}
}
