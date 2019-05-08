using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour {
	public GameObject instructionsPanel;
	public GameObject startPanel;

	// Use this for initialization
	public void onClick(string difficulty) {
		instructionsPanel.SetActive(false);
		startPanel.SetActive(true);
	}
}
