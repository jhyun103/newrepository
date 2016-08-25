using UnityEngine;
using System.Collections;

public class soundTrigger : MonoBehaviour {
	public GameObject newplayer;

	AudioSource audio; 

	// Use this for initialization
	void Start () {
		audio = newplayer.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D( Collider2D col)
	{

		if (col.gameObject.tag == "soundTrigger") {
			Debug.Log (" stop");

			audio.Stop ();
		}
	}

	void OnTriggerExit2D( Collider2D col)
	{

		if (col.gameObject.tag == "soundTrigger") {

			Debug.Log ("Play");
			audio.Play();
		}
	}
}
