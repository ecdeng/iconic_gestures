using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateTableButtonScript : Singleton<GenerateTableButtonScript> {

	public Button generateTableButton;
	public InputField rowsField;
	public InputField colsField;
	public bool showTable = false;
	public int numRows;
	public int numCols;
	public List<string> inputVals;
	public Vector2 scrollPosition;

	public int maxWidth;
	public int maxCols;

	// Use this for initialization
	void Start () {
		
		Debug.Log ("START");
		scrollPosition = Vector2.zero;
		InputField rows = rowsField.GetComponent<InputField>();
		InputField cols = colsField.GetComponent<InputField>();
		inputVals = new List<string> ();
		Button btn = generateTableButton.GetComponent<Button>();
		btn.onClick.AddListener(GenerateTable);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI(){
		//Debug.Log ("ONGUI");
		
		if (showTable){
			
//			GUILayout.BeginArea(new Rect(Screen.width - numCols * 50, 0 , numCols*50, numRows*28), GUI.skin.window);
			//Debug.Log ("numCols:" + numCols + " numRows: " + numRows);

			GUILayout.BeginArea(new Rect(Screen.width - Screen.width/4, 0 , Screen.width/4, Screen.height - Screen.height / 20), GUI.skin.window);
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true); 

			for (int i = 0; i < numRows; i++) {
				GUILayout.BeginHorizontal ();
				for (int j = 0; j<numCols; j++) {
					inputVals[i*numCols + j] = GUILayout.TextField (inputVals[i*numCols + j], 3, GUILayout.Width(35), GUILayout.Height(20));

				}
				GUILayout.EndHorizontal ();
			}

			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

	}

	public void GenerateTable() {
		ExportButtonScript.Instance.Initialize ();
		ObjManager.Instance.setInSelectionMode(false); // change the state we're in
		ListControllerScript.Instance.RemoveUnselectedPoints();

		showTable = true;
		numRows = int.Parse(rowsField.text);
		numCols = int.Parse(colsField.text);
		int invalid = -1;
		for (int i = 0; i<numRows; i++) {
			for (int j = 0; j<numCols; j++) {
				inputVals.Add (invalid.ToString ());
			}
		}
	}
}
