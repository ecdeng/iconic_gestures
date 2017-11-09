using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Behavior script for generating table of given rows and columns (taken from user input). Allows user to enter point IDs to group points into gesturess
/// </summary>
public class GenerateTableButtonScript : Singleton<GenerateTableButtonScript> {

	// UI element for user input
	public Button generateTableButton;
	public InputField rowsField;
	public InputField colsField;
	 
	public bool showTable = false; // only render this table in OnGUI once button is clicked
	public int numRows; // storing input from user for numRows
	public int numCols; // storing input from user for numCols
	public List<string> inputVals; // 
	public Vector2 scrollPosition; // current scroll position, used to create vertical and horizontal scroll views for the table

	public int maxWidth; // maximum width for the table

	// Use this for initialization
	void Start () {
		
		Debug.Log ("START");
		scrollPosition = Vector2.zero; // initialize scroll position

		// get input for # of rows and cols from user. 
		// Columns is number of actors. The points within each gesture is the points entered in the rows below that column.
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

			// head each column with "Actor #x"
			int actorNumber = 1;
			GUILayout.BeginHorizontal ();
			for (int j = 0; j<numCols; j++) {
				GUILayout.Label ("Actor #" + actorNumber.ToString(), GUILayout.Width(85), GUILayout.Height(20));
				actorNumber++;

			}
			GUILayout.EndHorizontal ();

			// draw the rest of the table
			for (int i = 0; i < numRows; i++) {
				GUILayout.BeginHorizontal ();
				for (int j = 0; j<numCols; j++) {
					inputVals[i*numCols + j] = GUILayout.TextField (inputVals[i*numCols + j], 3, GUILayout.Width(85), GUILayout.Height(20));

				}
				GUILayout.EndHorizontal ();
			}

			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

	}

	public void GenerateTable() {
		ExportButtonScript.Instance.Initialize ();
		FileUpload.Instance.Disable ();
		Disable ();
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

	public void Disable() {
		generateTableButton.gameObject.SetActive(false);
		rowsField.gameObject.SetActive(false);
		colsField.gameObject.SetActive(false);
	}
}
