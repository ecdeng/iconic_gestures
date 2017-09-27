﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointScript : MonoBehaviour {
	public Dropdown myPointsDropdown;

	// Use this for initialization
	void Start() {
		myPointsDropdown.options.Clear();
		for (int i = 0; i < 20; i++) {
			int temp = i + 1;
			myPointsDropdown.options.Add (new Dropdown.OptionData () {text = temp.ToString()});
		}
		myPointsDropdown.value = 0;

		//also add code here to make it look greyed out
		myPointsDropdown.onValueChanged.AddListener(delegate {
			myDropdownValueChangedHandler(myPointsDropdown);
		});
	}

	private void myDropdownValueChangedHandler(Dropdown target) {
		Debug.Log("selected from points selector: "+ target.value);
	}

	//update options based on which model was selected
	public void UpdateOptions(string name) {
		myPointsDropdown.options.Clear();
		myPointsDropdown.value = 0;
		//update this later based on option
		for (int i = 0; i < 20; i++) {
			myPointsDropdown.options.Add (new Dropdown.OptionData () {text = i.ToString()});
		}
	}

	public void SetDropdownIndex(int index) {
		myPointsDropdown.value = index;
	}

	// Update is called once per frame
	void Update () {

	}
}
