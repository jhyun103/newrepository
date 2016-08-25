using UnityEngine;
using System.Collections;

public class musicbox : MonoBehaviour {
	private SpriteRenderer spriteRenderer;

	public Sprite open;
	public GameObject handle;
	public AudioClip music;
	AudioSource audio; 

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {

			this.gameObject.GetComponent<SpriteRenderer> ().sprite = open;

			handle.GetComponent<Animation> ().Play ();

			audio.Play ();
		}
	}

}