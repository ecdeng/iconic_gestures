using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used to share data across different scripts and classes
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance;
	void Awake() {
		Instance = (T) FindObjectOfType(typeof(T));
	}
}