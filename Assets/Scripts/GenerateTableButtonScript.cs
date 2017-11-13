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
		scrollPosition = Vector2.zero; // initialize scroll position

		// get input for # of rows and cols from user. 
		// Columns is number of actors. The points within each gesture is the points entered in the rows below that column.
		InputField rows = rowsField.GetComponent<InputField>();
		InputField cols = colsField.GetComponent<InputField>();
		inputVals = new List<string> (); 
		Button btn = generateTableButton.GetComponent<Button>();
		btn.onClick.AddListener(GenerateTable);
	}

	/// <summary>
	/// If showTable boolean is true, create table with GUILayout
	/// </summary>
	void OnGUI(){
		if (showTable){
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

	/// <summary>
	/// Takes number of rows and columns in and create appropriate list of strings to store the GUI.TextField's data
	/// Triggers showTable boolean
	/// Triggers removal of unselected points from model and list
	/// Disables appropriate buttons and fields
	/// </summary>
	public void GenerateTable() {
		if (rowsField.text.Length > 0 && colsField.text.Length > 0 && ObjManager.Instance.counter > 0) {

			ExportButtonScript.Instance.Initialize ();
			FileUpload.Instance.Disable ();
			Disable ();
			ObjManager.Instance.setInSelectionMode (false); // change the state we're in
			ListControllerScript.Instance.RemoveUnselectedPoints ();

			showTable = true;
			numRows = int.Parse (rowsField.text);
			numCols = int.Parse (colsField.text);
			int invalid = -1;
			for (int i = 0; i < numRows; i++) {
				for (int j = 0; j < numCols; j++) {
					inputVals.Add (invalid.ToString ());
				}
			}
		}
	}

	/// <summary>
	/// Disable this button in second stage
	/// </summary>
	public void Disable() {
		rowsField.gameObject.SetActive(false);
		colsField.gameObject.SetActive(false);
		generateTableButton.gameObject.GetComponent<CanvasGroup> ().alpha = 0;
	}
}