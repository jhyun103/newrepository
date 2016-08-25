using UnityEngine;
using System.Collections;

public class CollectableItem : MonoBehaviour {


	public Vector2 originalPosition;
	/// <summary>
	/// Flag for if this object can move/float.
	/// </summary>
	bool _canMove = true;
	/// <summary>
	/// Whether or not this item is collected and held in the player's list / trail.
	/// </summary>
	bool _collected = false;
	/// <summary>
	/// Flag for whether or not this object is currently moving.
	/// </summary>
	bool _moving = false;
	/// <summary>
	/// The position offset between this object and the others in the trail.
	/// </summary>
	public Vector2 posOffset = new Vector2(0f,0f);
	/// <summary>
	/// The target position used when this item is positioning in collected items trail.
	/// </summary>
	public Vector2 targetPos = new Vector2(0f,0f);
	/// <summary>
	/// Object before this object in the items list. Used for positioning when collected
	/// </summary>
	public GameObject refObj;
	/// <summary>
	/// Collectable object that uses this as reference for positioning
	/// </summary>
	public GameObject childObj;
	/// <summary>
	/// The rate/time at which this object moves from point A to point B
	/// </summary>
	float moveTime = 0.75f;
	//Object's vertical move speed
	public float moveSpeedVert = 0.75f;
	//Vector for object's bounce distance
	public Vector2 collisionBounce = new Vector2(.25f,.75f);
	//Speed or rate at which object bounces from point A to point B
	public float bounceTime = 0.25f;
	//Flag for whether collision coroutine has stated
	bool colCoroutineStarted = false;
	/// <summary>
	/// Flag for if this object should detect collision currently.
	/// </summary>
	bool _canCollide = true;
	/// <summary>
	/// Timer for how long this object should disregard collision with obstacles.
	/// </summary>
	float _collisionTimer = 0.0f;
	/// <summary>
	/// Time this object remains invulnerable after collision 
	/// </summary>
	public float invulnerableTime = .25f;
	//Reference to player's list of items
	CollectedItems items;
	//This object's place in the players list of items
	int _itemIndex = -1;
	//This sprite's original color
	Color origColor;
	//Amount of alpha to fade on detach
	float fadeAmount = 0.5f;
	//speed to fade color on detach
	float fadeSpeed = 0.015f;
	/// <summary>
	/// Flag for whether or not the alpha is fading.
	/// </summary>
	bool _fading = false;
	/// <summary>
	/// The speed at which this object floats upward when not held by player.
	/// </summary>
	public float floatSpeed = 0.2f;


	// sound 
	public AudioClip musicbox;
	AudioSource audio; 
	private bool hasPlayed = false;

	// Use this for initialization
	void Start () 


	{	audio = GetComponent<AudioSource> ();
		originalPosition = transform.position;
		items = GameObject.FindGameObjectWithTag ("Player").GetComponent<CollectedItems> ();
		origColor = this.gameObject.GetComponent<SpriteRenderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Check if obstacle collision flag should be reset
		if(!CanCollide)
		{
			//Invulnerable time limit has been reached so reset flag and color/alpha
			if(Time.time > _collisionTimer && !_fading)
			{
				Debug.Log("---Invulnerability Expired---");
				this.FadeAlpha(1);
				CanCollide = true;
				_collisionTimer = 0.0f;
			}
		}

		if(!Collected)
		{
			Float();
		}

//		if(Input.GetKeyDown(KeyCode.F))
//		{
//			this.FadeAlpha(-1);
//		}
//		if(Input.GetKeyDown(KeyCode.D))
//		{
//			this.FadeAlpha(1);
//		}
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if (CanCollide) {
			if (col.gameObject.tag == "Obstacle") {
				CollideObstacle (col.gameObject);
			}
		}
		if (col.gameObject.tag == "Player") {

			if (!hasPlayed) {

				audio.PlayOneShot (musicbox, 0.7f);
				hasPlayed = true;
			}
		}

		if ( col.gameObject.tag == "soundTrigger")
		{

			Debug.Log ("original");
			StopCoroutine("MoveToPos");
			transform.position = originalPosition;

		}

	}



	public void CollideObstacle(GameObject obstacle)
	{
		//detache from list
		if(Collected)
		{
			items.DetachItemIndex(_itemIndex);
		}

		if(!colCoroutineStarted)
		{
			StartCoroutine("CollisionMove",obstacle);
		}
	}

	/// <summary>
	/// Method called when object is collected.  Moves it into position in collection
	/// </summary>
	public void GetCollected()
	{
		if(!_moving)
		{
			StartCoroutine("MoveToPos",targetPos);
		}
	}

	/// <summary>
	/// Detach item from list and reset flags
	/// </summary>
	public void GetDetached()
	{
		//Set flag to prevent collision
		CanCollide = false;
		//Update collision timer
		_collisionTimer = Time.time + invulnerableTime;
		//fade alpha
		this.FadeAlpha (-1);
		//reset collected flag
		this.Collected = false;
		//reset index
		this.ItemIndex = -1;
		//update child object's 'parent' - the ref used for positioning & index
		if (this.childObj != null) 
		{
			CollectableItem child = childObj.GetComponent<CollectableItem> ();
			child.refObj = this.refObj;
			child.UpdateIndex();
		}

		//reset the child object reference
		if(this.childObj != null){this.childObj = null;}
		//reset the 'ref' object used for positioning in collected list
		refObj = null;

	}

	/// <summary>
	/// Coroutine that moves object to it's position
	/// </summary>
	/// <returns>The to position.</returns>
	IEnumerator MoveToPos(Vector2 pos)
	{
		//flag as moving
		_moving = true;
		// iterator variable
		float i = 0.0f;
		//rate of movement
		float rate = 1 / moveTime;
		Vector2 startPos = new Vector2(this.gameObject.transform.position.x,this.gameObject.transform.position.y);
		Vector2 updatePos = new Vector2();

		while(i < 1 && _canMove)
		{
			i += rate * Time.deltaTime;
			updatePos = Vector2.Lerp(startPos,pos,i);
			this.gameObject.transform.position = new Vector3(updatePos.x,updatePos.y,0);
			yield return null;
		}

		_moving = false;
	}

	public void UpdatePos()
	{
		if(!_moving && _canMove)
		{
			// New pos will be previous object's position + offset, OR if no previous obj, then player pos + offset 
			Vector2 newPos = new Vector2 (refObj.transform.position.x + posOffset.x, refObj.transform.position.y + posOffset.y);

			StartCoroutine("MoveToPos",newPos);
		}
	}

	IEnumerator CollisionMove(GameObject obstacle)
	{
		//set coroutine flag
		colCoroutineStarted = true;
		
		//Disable movement temporarily
		_canMove = false;
		
		//Determine bounce direction
		int dirX,dirY = 1;
		dirX = obstacle.transform.position.x > this.gameObject.transform.position.x ? -1 : 1;  //if obstacle is to the right, then bounce left. Else, bounce right.
		dirY = obstacle.transform.position.y > this.gameObject.transform.position.y ? -1 : 1; //if Obstacle is above, then bounce down. Else, bounce up.
		
		//establish target position
		Vector2 currPos = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
		Vector2 targetPos = new Vector2(currPos.x + (collisionBounce.x * dirX),currPos.y + (collisionBounce.y * dirY)); //target position is current position plus bounce
		Vector2 updatePos = new Vector2 ();
		//lerp in loop from i=0 to 1, with interval being set bounce speed
		float i = 0.0f;
		float rate = 1.0f / bounceTime;
		while(i<1)
		{
			i+=rate * Time.deltaTime;
			updatePos = Vector2.Lerp(currPos,targetPos,i);
			this.gameObject.transform.position = new Vector3(updatePos.x,updatePos.y,this.gameObject.transform.position.z);
			
			yield return null;
		}
		
		colCoroutineStarted = false;
		_canMove = true;
	}

	public bool Collected
	{
		get{return _collected;}

		set{ _collected = value;}
	}

	public int ItemIndex
	{
		get{return _itemIndex;}
		set{ this._itemIndex = value;}
	}

	/// <summary>
	/// Gets a value indicating whether this instance can collide with a negative object
	/// </summary>
	/// <value><c>true</c> if this instance can collide; otherwise, <c>false</c>.</value>
	public bool CanCollide
	{
		get{return this._canCollide;}
		set{ this._canCollide = value;}
	}

	/// <summary>
	/// Updates this object's index, and in turn calls method on its child
	/// </summary>
	public void UpdateIndex()
	{
		this.ItemIndex--;
		if(this.childObj != null)
		{
			this.childObj.GetComponent<CollectableItem>().UpdateIndex();
		}
	}

	void FadeAlpha(int fadeDir)
	{
		if(!_fading)
		{
			_fading = true;
			StartCoroutine ("FadeAlphaCo",fadeDir);
		}
	}

	IEnumerator FadeAlphaCo(int dir)
	{
		Color targetColor;
		Color currColor;
		float i = 0.0f;
		float rate = 1/fadeSpeed;

		/*If dir variable is -1 then lerp original color to orig color minus fade amount*/
		if(dir <0)
		{
			targetColor = new Color(origColor.r,origColor.g,origColor.b,origColor.a-this.fadeAmount);

			while(i < 1)
			{
				i+=rate*Time.deltaTime;
				currColor = Color.Lerp(origColor,targetColor,i);
				this.gameObject.GetComponent<SpriteRenderer>().material.color = currColor;
				yield return null;
			}
		}
		else
		{
			targetColor = new Color(origColor.r,origColor.g,origColor.b,origColor.a);

			while(i < 1)
			{
				i+=rate*Time.deltaTime;
				currColor = Color.Lerp(origColor,targetColor,i);
				this.gameObject.GetComponent<SpriteRenderer>().material.color = currColor;
				yield return null;
			}
		}
	
		_fading = false;

		Debug.Log ("Fading FINISHED");
	}

	public void Float()
	{
		Vector3 pos = this.gameObject.transform.position;
		this.gameObject.transform.position = new Vector3(pos.x, pos.y + (floatSpeed*Time.deltaTime), pos.z);
	}
}
