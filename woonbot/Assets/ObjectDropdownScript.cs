using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDropdownScript : MonoBehaviour {

	public Dropdown myDropdown; 
	ObjectManager objManager = GameObject.Find("ObjectManager");

	// Use this for initialization
	void Start () {
		myDropdown.onValueChanged.AddListener(delegate {
			myDropdownValueChangedHandler(myDropdown);
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void myDropdownValueChangedHandler(Dropdown target) {
		Debug.Log("selected: "+ target.value);
		Debug.Log (printStringFromIndex (target.value));
	}

	public void SetDropdownIndex(int index) {
		myDropdown.value = index;
	}

	public string printStringFromIndex(int index) {
		string result;
		if (index == 0) {
			result = "NoObject";
		} else if (index == 1) {
			result = "basketball";
		} else if (index == 2) {
			result = "sword";
		} else if (index == 3) {
			result = "pencil";
		} else {
			result = "ERROR";
		}

		objManager
		return result;
	}
}
