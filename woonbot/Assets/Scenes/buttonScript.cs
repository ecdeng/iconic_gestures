using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class buttonScript : MonoBehaviour {

	public Button yourButton;
	// Use this for initialization
	void Start () {
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		Debug.Log("You have clicked the button!");
		Texture2D texture = Selection.activeObject as Texture2D;
		if (texture == null)
		{
			EditorUtility.DisplayDialog("Select Texture", "You must select a texture first!", "OK");
			return;
		}

		string path = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
		if (path.Length != 0)
		{
			WWW www = new WWW("file:///" + path);
			www.LoadImageIntoTexture(texture);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
