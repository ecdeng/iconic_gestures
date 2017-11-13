using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles behavior for upload button
/// opens file explorer and gets absolute file path
/// </summary>
public class FileUpload : Singleton<FileUpload> {

	private GameObject model;
	public Button fileUploadButton;
	public InputField fileInputField;
	public string path;

	// Use this for initialization
	void Start () {
		InputField fileInput = fileInputField.GetComponent<InputField> ();
		Button btn = fileUploadButton.GetComponent<Button>();
		btn.onClick.AddListener(GetFilePath);
	}

//	public void OpenExplorer() {
//		path = EditorUtility.OpenFilePanel ("Object Upload", "", "obj");
//		//System.Diagnostics.Process.Start ("explorer.exe", "-p");
//		if(path.Length != 0)
//			ObjManager.Instance.LoadModel (path);
//
//	}

	public void GetFilePath() {
		path = fileInputField.text;
		if (path.Length != 0) {
			ObjManager.Instance.LoadModel (path);
		}
	}

	public void Disable () {
		Button btn = fileUploadButton.GetComponent<Button>();
		btn.gameObject.SetActive (false);
	}
}
