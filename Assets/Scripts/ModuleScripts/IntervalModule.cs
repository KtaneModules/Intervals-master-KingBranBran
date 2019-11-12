using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class IntervalModule : MonoBehaviour {
	public KMBombModule intervalModule;
	public KMAudio intervalAudio;
	public KMSelectable[] buttons;
	public TextMesh[] buttonText;
	private IntervalButtonHandler buttonHandler = new IntervalButtonHandler();
	private IntervalAudioHandler audioHandler = new IntervalAudioHandler();
	private int interval;
	private static int _moduleIdCounter;
	private int _moduleId;
	private int chosenInterval;
	private string chosenIntervalName;
	private string chosenIntervalNameModuleFriendly;
	private string intervalType;
	private string[] buttonComboDebug = {"cycle the left button.", "cycle the right button.", "play the interval.", "submit the given interval."};
	private string[] combos;
	private bool firstSubmitYet = false;
	private bool modulePass = false;
	void Start () {
		_moduleId = _moduleIdCounter++;

		for (int i = 0; i < buttons.Length; i++) {
			var j = i;
			buttons[j].OnInteract += delegate {ButtonPressed(j); return false;};
		}

		chosenInterval = Random.Range(0, 13);
		chosenIntervalName = Enum.GetName(typeof(IntervalName), chosenInterval).Replace("_", " or ");
		chosenIntervalNameModuleFriendly = Enum.GetName(typeof(IntervalNameModuleFriendly), chosenInterval).Replace("t", "+").Replace("_", " or ");

		DebugLog("INTERVAL: {0} ({1})", chosenIntervalName, chosenIntervalNameModuleFriendly);

		intervalType = audioHandler.DetermineIntervalType(Random.Range(0, 3));
		DebugLog("The interval will be played {0}", intervalType);

		buttonHandler.RandomizeButtonCombos();
		combos = buttonHandler.GetButtonCombos();
		DebugLog("[BUTTON COMBINATIONS]");
		
		for (int i = 0; i < combos.Length; i++) {
			var combo = combos[i];
			DebugLog("{0} -> {1} will {2}", combo[0], combo[1], buttonComboDebug[i]);
		}
	}	

	void ButtonPressed(int number) {

		buttons[number].AddInteractionPunch(.5f);

		if (modulePass) {
			return;
		}
		var twoButtonsPressed = buttonHandler.ButtonPressed(number);

		if (twoButtonsPressed) {
			var comboNum = buttonHandler.GetCombination();
			var combo = combos[comboNum];

			switch (comboNum) {
				case 0:
					StartCoroutine(audioHandler.PlayCycleSound(intervalAudio, intervalModule.transform, comboNum));
					buttonHandler.CycleLeftDisplay(buttonText[0]);
					break;
				case 1:
					StartCoroutine(audioHandler.PlayCycleSound(intervalAudio, intervalModule.transform, comboNum));
					buttonHandler.CycleRightDisplay(buttonText[1]);
					break;
				case 2:
					StartCoroutine(audioHandler.PlayIntervalRandomNote(chosenInterval, intervalAudio, intervalModule.transform, intervalType));
					break;
				case 3:
					DebugLog("{0} -> {1} was pressed, I will {2}", combo[0], combo[1], buttonComboDebug[comboNum]);
					audioHandler.PlaySubmitSound(intervalAudio, intervalModule.transform);
					SubmitPressed();
					break;
				default:
					DebugLog("Something went wrong...");
					break;
			}

			buttonHandler.ResetCombo();
		}
	}

	void SubmitPressed() {

		var display = buttonHandler.GetDisplay();

		if (!firstSubmitYet) {
			DebugLog("Just kidding! That was the first submit press, so I canceled it!");
			firstSubmitYet = true;
			return;
		}

		if (chosenIntervalNameModuleFriendly.Contains(display)) {
			intervalModule.HandlePass();
			StartCoroutine(audioHandler.PlaySolveSound(intervalAudio, intervalModule.transform));
			DebugLog("Correct interval submitted ({0}). Module passed!", buttonHandler.GetDisplay());
			modulePass = true;
			buttonHandler.ClearDisplay(buttonText);
		} else {
			intervalModule.HandleStrike();
			DebugLog("Incorrect interval submitted ({0}).  Strike!", buttonHandler.GetDisplay());
		}
	}

	private List<string> RandomizeStringList(List<string> list)
    {
		List<string> RandomizedList = new List<string>();
		var currentList = list;

		for (int i = 0; i < list.Count; i++) {
			var ix = Random.Range(0, currentList.Count);
			RandomizedList.Add(currentList[ix]);
			currentList.RemoveAt(ix);
		}

        return RandomizedList;
    }

	private void DebugLog(string log, params object[] args)
    {
        var logData = string.Format(log, args);
        Debug.LogFormat("[Intervals #{0}] {1}", _moduleId, logData);
    }

	string TwitchHelpMessage = "Do !{0} 1 2 1 to press the two buttons (1 is left, 2 is right).";

	IEnumerator ProcessTwitchCommand(string command) {
		var parts = command.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

		if (parts.All(x => x == "1" || x == "2") && parts.Count() < 25) {

			yield return null;

			for (int i = 0; i < parts.Length; i++) {
				ButtonPressed(int.Parse(command.Split(' ')[i]) - 1);
				yield return new WaitForSeconds(.15f);
			}
		}
	}
}
