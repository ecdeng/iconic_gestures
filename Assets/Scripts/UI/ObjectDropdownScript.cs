using System.Collections;
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

		List<Dropdown.OptionData> menuOptions = myDropdown.options;


		//get the string value of the selected index
		string value = menuOptions [0].text;

		model.SendMessage("ChangeObject",new string[]{value,"10"});
		hand1.SendMessage ("ResetState");
		hand2.SendMessage ("ResetState");
		pointsDropdown.SendMessage ("UpdateOptions", value);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void myDropdownValueChangedHandler(Dropdown target) {
		Debug.Log("selected: "+ target.value);

		//get all options available within this dropdown menu
		List<Dropdown.OptionData> menuOptions = target.options;

		//get the string value of the selected index
		string value = menuOptions [target.value].text;
		Debug.Log (value);



		model.SendMessage("ChangeObject",new string[]{value,""});
		hand1.SendMessage ("ResetState");
		hand2.SendMessage ("ResetState");
		pointsDropdown.SendMessage ("UpdateOptions", value);
		//pointsDropdown.SendMessage ("UpdateOptions", printStringFromIndex(target.value));

	}

	public void SetDropdownIndex(int index) {
		myDropdown.value = index;
	}
		
}
