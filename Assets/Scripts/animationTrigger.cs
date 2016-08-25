using UnityEngine;
using System.Collections;

public class animationTrigger : MonoBehaviour {
	public GameObject[] objects;
	// Use this for initialization
	//	public GameObject bicycle;
	//public float happy = 0.0f;

	private bool hasPlayed = false;


	void OnTriggerEnter2D(Collider2D other) {


		if (other.gameObject.tag == "Player") {

			for (var i = 0; i < objects.Length; i ++)
				if(!hasPlayed){
				//	objects [i].GetComponent<Animation> ().wrapMode = WrapMode.Once;
				objects [i].GetComponent<Animation> ().Play ();
					hasPlayed = true;
				}

		}


	}


	/*void OnTriggerExit2D(Collider2D other){

		if (other.gameObject.tag == "person" ) {

			happy = 0.1f;

		}

		//bicycle2.GetComponent<Animation>().wrapMode = WrapMode.Once;
		//bicycle2.GetComponent<Animation>().Play ("ride2");

	}
}*/
}

