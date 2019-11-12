using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class IntervalButtonHandler {

    private string[] buttonCombos = {"11", "22", "12", "21"};
    private string currentButtonPresses = "";
    private string leftButtonOrder = "IA+oP";
    private string displayState = "I1";

    public void RandomizeButtonCombos() {
        buttonCombos = RandomizeStringList(buttonCombos.ToList()).ToArray();
    }

    public bool ButtonPressed(int number) {        
        if (currentButtonPresses.Length == 0) {
            currentButtonPresses += number + 1;
            // Do visual stuff //
            return false;
        } else {
            currentButtonPresses += number + 1;
            // Do visual stuff //
            return true;
        }
    }

    public int GetCombination() {
        return Array.IndexOf(buttonCombos, currentButtonPresses);
    }

    public string[] GetButtonCombos() {
        return buttonCombos;
    }

    public void ResetCombo() {
        currentButtonPresses = "";
    }

    public void CycleLeftDisplay(TextMesh text) {
        text.text = leftButtonOrder[(Array.IndexOf(leftButtonOrder.ToArray(), Char.Parse(text.text)) + 1) % 5].ToString(); // Next char in leftButtonOrder
        displayState = text.text + displayState[1];
    }

    public void CycleRightDisplay(TextMesh text) {
        text.text = ((int.Parse(text.text) % 8) + 1).ToString(); // only 1 - 8
        displayState = displayState[0] + text.text;
    }

    public string GetDisplay() {
        return displayState;
    }

    public void ClearDisplay(TextMesh[] texts) {
        for (int i = 0; i < texts.Count(); i++) {
            texts[i].text = "";
        }
    }

    private List<string> RandomizeStringList(List<string> list)
    {
		List<string> RandomizedList = new List<string>();
		var currentList = list;

		for (int i = 0; i < 4; i++) {
			var ix = Random.Range(0, currentList.Count);
			RandomizedList.Add(currentList[ix]);
			currentList.RemoveAt(ix);
		}

        return RandomizedList;
    }


}