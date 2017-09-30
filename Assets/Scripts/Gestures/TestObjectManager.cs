using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectManager : MonoBehaviour {

	public string ObjectName;
	private GameObject currObject;
	public static TestObjectManager Instance = null;


	void Awake() {

		if (Instance == null) {
			Instance = this;
		}
		currObject = (GameObject)Instantiate(Resources.Load(ObjectName + "/" + ObjectName));
		currObject.transform.parent = gameObject.transform;
	}

	// Use this for initialization
	void Start () {


		
	}

	public GameObject GetObject () {
		return currObject;
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
