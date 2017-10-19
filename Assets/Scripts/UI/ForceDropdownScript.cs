﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceDropdownScript : MonoBehaviour {

	public Dropdown myForceDropdown;
	private GameObject model;
	private GameObject hand1;
	private GameObject hand2;

	// Use this for initialization
	void Start () {
		model = GameObject.Find ("Model");
		myForceDropdown.onValueChanged.AddListener(delegate {
			myForceDropdownValueChangedHandler(myForceDropdown);
		});
		model = GameObject.Find ("Model");
		hand1 = GameObject.Find ("Hand1");
		hand2 = GameObject.Find ("Hand2");
	}

	// Update is called once per frame
	void Update () {

	}

	private void myForceDropdownValueChangedHandler(Dropdown target) {
		Debug.Log("selected force: "+ target.value);


	}

	public void SetDropdownIndex(int index) {
		myForceDropdown.value = index;
	}
}
