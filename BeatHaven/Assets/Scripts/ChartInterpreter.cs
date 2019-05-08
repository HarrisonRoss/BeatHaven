using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class ChartInterpreter {
	public static SortedDictionary<int, NoteData> Interpret(string filename, string difficulty) {
		SortedDictionary<int, NoteData> notes = new SortedDictionary<int, NoteData>();
		StreamReader reader = new StreamReader(filename);
		string text = "";
		while(!reader.EndOfStream) {
			text += reader.ReadLine() + "\n";
		}
		reader.Close();
		int resolution = System.Int32.Parse(Regex.Match(text, @"Resolution = (\d+)\n").Groups[1].Value);
		int bpm = System.Int32.Parse(Regex.Match(text, @"0 = B (\d+)\n").Groups[1].Value) / 1000;
		int offset = (int)(float.Parse(Regex.Match(text, @"Offset = (\d+\.?\d*)\n").Groups[1].Value) * 60);

		int start_of_notes = Regex.Match(text, @"\[" + difficulty + @"Single\]").Index;
		text = text.Substring(start_of_notes);
		int end_of_notes = Regex.Match(text, @"\}").Index;
		text = text.Substring(0,end_of_notes);

		Regex reg = new Regex(@"(\d+) = (.) (\d) (\d+)");
		MatchCollection matches = reg.Matches(text);
		int prevnum = int.MinValue;
		foreach (Match match in matches) {
			GroupCollection groups = match.Groups;
			int num = System.Int32.Parse(groups[1].Value);
			bool hopo = ((num - prevnum) <= resolution / 3);
			prevnum = num;
			num = num * 60 * 60 / (bpm * resolution) + offset;
			bool starpower = (groups[2].Value == "S");
			int note_num = System.Int32.Parse(groups[3].Value);
			bool tap = (note_num == 6);
			int duration = System.Int32.Parse(groups[4].Value);
			duration = duration * 60 * 60 / (bpm * resolution);
			if (starpower) {
				// Currently not supported
			}
			else {
				if (!notes.ContainsKey(num)) {
					notes.Add(num, new NoteData());
				}
				notes[num].hopo = hopo;
				notes[num].duration = duration;
				if (tap) {
					notes[num].tap = true;
				}
				notes[num].starpower = starpower;
				if (note_num < 5) {
					notes[num].notes[note_num] = true;
					notes[num].num_notes++;
				}
			}
		}
		NoteData prev_note = new NoteData();
		// NoteData prev_note = notes[offset];
		foreach (KeyValuePair<int, NoteData> entry in notes) {
			NoteData note = entry.Value;
			if (note.hopo) {
				if (note.num_notes > 1) {
					// If note is a chord, disable hopo
					note.hopo = false;
				}
				else {
					// If note is following same note, disable hopo
					for (int i = 0; i < 5; i++) {
						if (prev_note.notes[i] && note.notes[i]) {
							note.hopo = false;
						}
					}
				}
			}
			prev_note = note;
		}
		return notes;
	}
}