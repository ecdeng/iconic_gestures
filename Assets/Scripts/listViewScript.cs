using UnityEngine;
using System.Collections;

public class listViewScript : MonoBehaviour {

	Vector2 scrollPosition = Vector2.zero;

	void OnGUI () {
		string[] listItems = 
		{
			"Hello world,",
			"this",
			"is a",
			"very",
			"very",
			"very",
			"very",
			"very",
			"very",
			"long",
			"list.",
		};

		GUILayout.BeginArea(new Rect(0f, 0f, 300f, 200f), GUI.skin.window);
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true); 
		GUILayout.BeginVertical(GUI.skin.box);

		foreach (string item in listItems)
		{
			GUILayout.Label(item, GUI.skin.box, GUILayout.ExpandWidth(true));
		}

		GUILayout.EndVertical();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}
}