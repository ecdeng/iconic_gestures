using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

	public GameObject Panel;

	// Use this for initialization
	void Start () {
		Panel.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void showPanel() {
		Panel.gameObject.SetActive (true);
	}

	public void hidePanel() {
		Panel.gameObject.SetActive (false);
	}
}
