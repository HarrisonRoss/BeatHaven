using System.Collections;
using System.Collections.Generic;

public class NoteData {
	public bool[] notes;
	public int num_notes;
	public bool tap;
	public bool starpower;
	public bool hopo;
	public int duration;
	public NoteData() {
		this.notes = new bool[5];
		this.num_notes = 0;
		for (int i = 0; i < 5; i++) {
			this.notes[i] = false;
		}
		this.tap = false;
		this.starpower = false;
		this.hopo = false;
		this.duration = 0;
	}
}