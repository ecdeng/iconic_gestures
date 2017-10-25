using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class listViewScript : MonoBehaviour {

	Vector2 scrollPosition = Vector2.zero;
	private GameObject model;
//	public Dictionary<int,GameObject> point_ids;

	void OnGUI () {
//		Dictionary<int,GameObject> point_ids = ObjManager.Instance.GetVerticesWithIDs();
//		List<string> listElements = new List<string>();
//		foreach(KeyValuePair<int, GameObject> entry in point_ids)
//		{
//			// do something with entry.Value or entry.Key
//			listElements.Add(entry.Key.ToString() + ": (" 
//				+ entry.Value.transform.position.x.ToString() + ", " 
//				+ entry.Value.transform.position.y.ToString() + ", "
//				+ entry.Value.transform.position.z.ToString() + ")");
//		} 
//
//		GUILayout.BeginArea(new Rect(0f, 0f, 300f, Screen.height), GUI.skin.window);
//		scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true); 
//		GUILayout.BeginVertical(GUI.skin.box);
//
//		foreach (string item in listElements)
//		{
//			GUILayout.Label(item, GUI.skin.box, GUILayout.ExpandWidth(true));
//		}
//
//		GUILayout.EndVertical();
//		GUILayout.EndScrollView();
//		GUILayout.EndArea();
	}
}