using UnityEngine;
using System.Collections;

public class BubbleScript : MonoBehaviour {

	/// <summary>
	/// The speed at which this object floats upward.
	/// </summary>
	public float floatSpeedVert = 0.95f;
	/// <summary>
	/// The speed at which this object floats horizontally
	/// </summary>
	public float floatSpeedHoriz = 0.5f;
	/// <summary>
	/// Flag for this object's current horizontal direction
	/// </summary>
	public int hDir = 1;

	public float dirTimer = 0.0f;

	public float changeDirTime = 1.75f;

	public bool temporary = true;

	float lifeSpan = 6.0f;

	float destroyTime = 1.0f;

	// Use this for initialization
	void Start () 
	{
		destroyTime = Time.time + lifeSpan;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if the temporary flag is set and lifespan is exceeded then destroy
		if(temporary && Time.time>this.destroyTime)
		{
			Destroy(this.gameObject);
		}

		//If time for this direction has been exceeded than change direction
		if(Time.time>dirTimer)
		{
			//Switch horizontal direction flag
			hDir *= -1;
			//reset timer
			dirTimer = Time.time+changeDirTime;
		}

		//Float upward
		Float ();
	}

	public void Float()
	{
		Vector3 pos = this.gameObject.transform.position;
		this.gameObject.transform.position = 
			new Vector3(pos.x + (floatSpeedHoriz*Time.deltaTime*hDir), pos.y + (floatSpeedVert*Time.deltaTime), pos.z);
	}

}
