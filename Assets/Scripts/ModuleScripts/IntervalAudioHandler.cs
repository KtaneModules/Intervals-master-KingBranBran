using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class IntervalAudioHandler {

	static string soundList = "C3,C#3,D3,D#3,E3,F3,F#3,G3,G#3,A3,A#3,B3,C4,C#4,D4,D#4,E4,F4,F#4,G4,G#4,A4,A#4,B4,C5,C#5,D5,D#5,E5,F5,F#5,G5,G#5,A5,A#5,B5,C6,C#6,D6,D#6,E6,F6,F#6,G6,G#6,A6,A#6,B6,C7,C#7,D7,D#7,E7,F7,F#7,G7,G#7,A7,A#7,B7,C8";
	string[] soundListArray = soundList.Split(',');

	public IEnumerator PlayIntervalRandomNote(int intervalInSemitones, KMAudio moduleAudio, Transform transform, string type) {
		var randomStartingNote = Random.Range(0, soundListArray.Count() - intervalInSemitones);
		
		switch(type) 
		{
			case "Harmonically":
				moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote], transform);
				moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote + intervalInSemitones], transform);
				break;	
			default:
				moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote + (type == "Ascending" ? 0 : intervalInSemitones)], transform);
				yield return new WaitForSeconds(1);
				moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote + (type == "Descending" ? 0 : intervalInSemitones)], transform);
				break;
		}
		yield return null;
	}

	public IEnumerator PlayCycleSound(KMAudio moduleAudio, Transform transform, int comboNum) {
		var randomStartingNote = Random.Range(8, soundListArray.Count() - 8);
		var offset = 0;

		for (int i = 0; i < 3; i++) {
			moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote + offset], transform);
			offset += (comboNum == 0 ? 1 : -1) * (Random.Range(0, 4) + 1);
			yield return new WaitForSeconds(.1f);
		}
	}

	public void PlaySubmitSound(KMAudio moduleAudio, Transform transform) {
		var randomStartingNote = Random.Range(8, soundListArray.Count() - 8);
		var offset = 0;

		for (int i = 0; i < 3; i++) {
			moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote + offset], transform);
			offset += Random.Range(0, 2) == 0 ? 3 : 4;
		}
	}

	public IEnumerator PlaySolveSound(KMAudio moduleAudio, Transform transform) {

		var randomStartingNote = Random.Range(0, soundListArray.Count() - 12);
		
		yield return new WaitForSeconds(1f);
		moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote], transform);
		yield return new WaitForSeconds(.2f);
		moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote + 4], transform);
		yield return new WaitForSeconds(.2f);
		moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote + 7], transform);
		yield return new WaitForSeconds(.2f);
		moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote + 12], transform);
		yield return new WaitForSeconds(.2f);
		moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote + 7], transform);
		yield return new WaitForSeconds(.2f);
		moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote + 4], transform);
		yield return new WaitForSeconds(.2f);
		moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote], transform);
		moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote + 4], transform);
		moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote + 7], transform);
		moduleAudio.PlaySoundAtTransform(soundListArray[randomStartingNote + 12], transform);	
	}

	public string DetermineIntervalType(int number) {
		if (number == 1) {
			return "Ascending";
		} else if (number == 2) {
			return "Descending";
		} else {
			return "Harmonically";
		}
	}
}

	
	
