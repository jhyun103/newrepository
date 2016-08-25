using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public float CameraMoveSpeed = 0.5f;
	GameObject player;

	void Awake()
	{
		//Get reference to player gameobject
		GameObject obj = GameObject.FindGameObjectWithTag ("Player");
		if(obj == null)
		{
			Debug.LogException(new UnityException("CameraScript couldn't find player!"));
		}
		else
		{
			player = obj;
		}

	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the player sprite has moved past the midpoint of the screen, then move the camera up
		if(player.transform.position.y >  ((Screen.height/2)-player.GetComponent<SpriteRenderer>().sprite.bounds.size.y))
		{
			UpdatePos(false);
		}
		else //Otherwise move the camera down
		{
			UpdatePos (true);
		}
	}

	/// <summary>
	/// Updates the camera's position.
	/// </summary>
	/// <param name="moveDown">If set to <c>true</c> camera's position will move downward. Else, moves up.</param>
	public void UpdatePos(bool moveDown)
	{
		Vector3 oldPos = this.gameObject.transform.position;

		if(moveDown)
		{
			this.gameObject.transform.position = new Vector3(oldPos.x, oldPos.y - CameraMoveSpeed * Time.deltaTime,oldPos.z);
			Debug.Log ("Camera moving down.");
		}
		else
		{
			this.gameObject.transform.position = new Vector3(oldPos.x, oldPos.y + CameraMoveSpeed * Time.deltaTime,oldPos.z);
			Debug.Log ("Camera moving up.");
		}
	}
}
