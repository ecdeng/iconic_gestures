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
	public int numRows = 2;
	public int numCols = 2;
	public string str = "";
	List<string> inputVals;

	// Use this for initialization
	void Start () {
		
		Debug.Log ("START");
		inputVals = new List<string> ();
		Button btn = generateTableButton.GetComponent<Button>();
		btn.onClick.AddListener(GenerateTable);
		TablePanel = GameObject.Find ("TablePanel");
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI(){
		//Debug.Log ("ONGUI");
		
		if (showTable){
			
			GUILayout.BeginArea(new Rect(Screen.width - numCols * 150, 0 , numCols*150, numRows*40), GUI.skin.window);
			Debug.Log ("numCols:" + numCols + " numRows: " + numRows);

			for (int i = 0; i < numRows; i++) {
				GUILayout.BeginHorizontal ();
				for (int j = 0; j<numCols; j++) {
					inputVals[i*numCols + j] = GUILayout.TextField (inputVals[i*numCols + j], 3);

				}
				GUILayout.EndHorizontal ();
			}

			GUILayout.EndArea();
		}


	}

	public void GenerateTable() {
		Debug.Log ("GENERATE TABLE");
		showTable = true;
		numRows = 10;
		numCols = 2;
		int invalid = -1;
		for (int i = 0; i<numRows; i++) {
			for (int j = 0; j<numCols; j++) {
				inputVals.Add (invalid.ToString ());
			}
		}
	}
}
