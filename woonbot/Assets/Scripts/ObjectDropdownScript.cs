﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDropdownScript : MonoBehaviour {

	public Dropdown myDropdown;
	private GameObject model;
	private GameObject hand1;
	private GameObject hand2;
	private GameObject pointsDropdown;

	// Use this for initialization
	void Start () {
		myDropdown.onValueChanged.AddListener(delegate {
			myDropdownValueChangedHandler(myDropdown);
		});
		model = GameObject.Find ("Model");
		hand1 = GameObject.Find ("Hand1");
		hand2 = GameObject.Find ("Hand2");
		pointsDropdown = GameObject.Find ("POCDropdown");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void myDropdownValueChangedHandler(Dropdown target) {
		Debug.Log("selected: "+ target.value);
		Debug.Log (printStringFromIndex (target.value));
		model.SendMessage("ChangeObject",printStringFromIndex (target.value));
		hand1.SendMessage ("ResetState");
		hand2.SendMessage ("ResetState");
		pointsDropdown.SendMessage ("UpdateOptions", printStringFromIndex(target.value));

	}

	public void SetDropdownIndex(int index) {
		myDropdown.value = index;
	}

	public string printStringFromIndex(int index) {
		string result;
		if (index == 0) {
			result = "NoObject";
		} else if (index == 1) {
			result = "basketball"; //1-20
		} else if (index == 2) {
			result = "sword"; 
		} else if (index == 3) {
			result = "pencil";
		} else {
			result = "ERROR";
		}
		return result;
	}
}
