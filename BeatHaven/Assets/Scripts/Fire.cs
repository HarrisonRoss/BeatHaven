using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
	private int enabled;

	// Use this for initialization
	void Start () {
		enabled = 0;
		GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		enabled--;
		GetComponent<SpriteRenderer>().enabled = (enabled >= 0);
	}

	public void Hit() {
		enabled = 5;
	}
}
