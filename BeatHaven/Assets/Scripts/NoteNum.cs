using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteNum : MonoBehaviour {
	public int num;
	public bool missed; // Only used by AI
	void Start() {
		missed = false;
	}
}
