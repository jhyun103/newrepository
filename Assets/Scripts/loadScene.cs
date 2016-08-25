using UnityEngine;
using System.Collections;

public class loadScene : MonoBehaviour {
	public string level;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	


		if (Input.GetKey (KeyCode.A)) {

			Application.LoadLevel (level);

		}

		if (Input.GetKey (KeyCode.JoystickButton11)) {

			Application.LoadLevel (level);

		}
	}
}
