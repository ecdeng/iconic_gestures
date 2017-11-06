using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateTableButtonScript : MonoBehaviour {

	public Button generateTableButton;
	public GameObject TablePanel;
	public GameObject ListItemPrefab;
	private GameObject model;
	public bool showTable = false;
	public int numRows = 3;
	public int numCols = 2;
	public string str = "";
	List<string> inputVals;

	// Use this for initialization
	void Start () {
		inputVals = new List<string> ();
		Button btn = generateTableButton.GetComponent<Button>();
		btn.onClick.AddListener(delegate{GenerateTable(2,2);});
		TablePanel = GameObject.Find ("TablePanel");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		Debug.Log ("ONGUI");
		//for testing

		//
		if (showTable){

			GUILayout.BeginArea(new Rect(Screen.width - numCols * 150, 0 , numCols*150, numRows*40), GUI.skin.window);

			for (int i = 0; i < numRows; i++) {
				GUILayout.BeginHorizontal ();
				for (int j = 0; j<numCols; j++) {
					inputVals[i*numRows + j] = GUILayout.TextField (inputVals[i*numRows + j], 3);
				}
				GUILayout.EndHorizontal ();
			}

			GUILayout.EndArea();
		}


	}

	public void GenerateTable(int rows, int cols) {
		Debug.Log ("GENERATE TABLE");
		showTable = true;

		int invalid = -1;
		for (int i = 0; i<numRows; i++) {
			for (int j = 0; j<numCols; j++) {
				inputVals.Add (invalid.ToString ());
			}
		}
	}
}
