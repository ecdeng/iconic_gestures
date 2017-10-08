using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButtonScript : MonoBehaviour {

	public Button myButton;
	private GameObject controlPanel;


	// Use this for initialization
	void Start () {
		myButton.onClick.AddListener(delegate {
			TaskOnClick(myButton);
		});
		controlPanel = GameObject.Find ("Control_Panel");
		controlPanel.SetActive(false);
	}

	// Update is called once per frame
	void Update () {

	}

	//make control panel appear
	void TaskOnClick(Button target)
	{
		Debug.Log ("hello is this working");
		if (controlPanel.activeInHierarchy) {
			controlPanel.SetActive (false);
		} else {
			controlPanel.SetActive (true);
		}
	}
}
