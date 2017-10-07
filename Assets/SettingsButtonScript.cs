using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control_Panel : MonoBehaviour {

	public Button myButton;
	private GameObject controlPanel;








	// Use this for initialization
	void Start () {
		myButton.onClick.AddListener(delegate {
			TaskOnClick(myButton);
		});
		controlPanel = GameObject.Find ("Control_Panel");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//make control panel appear
	void TaskOnClick(Button target)
	{
		Debug.Log ("hello is this working");
		controlPanel.SetActive(false);
	}
}
