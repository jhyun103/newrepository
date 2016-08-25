using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	//Flag for if the player can move
	bool _canMove = true;
	//Flag for if player can collide again
	bool _canCollide = true;
	//Timer used to track how long player has been invulnerable
	float _collisionTimer = 0.0f;
	//Time player is invulnerable after a collision
	public float invulnerableTime = .25f;
	//Player's horizontal move speed
	public float moveSpeedHoriz = 0.7f;
	//Speed at which player floats upward when no key is pressed
	public float moveSpeedFloat = 0.05f; 
	//Player's vertical move speed
	public float moveSpeedVert = 0.75f;
	//Vector for player's bounce distance
	public Vector2 collisionBounce = new Vector2(.25f,.75f);
	//Speed or rate at which player bounces from point A to point B
	public float bounceTime = 0.25f;
	bool colCoroutineStarted = false;
	public CollectedItems items;

	// Use this for initialization
	void Start () 
	{
		items = this.GetComponent<CollectedItems> ();
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (_canMove) 
		{
			if((Input.GetKey(KeyCode.UpArrow))|| (Input.GetKey(KeyCode.JoystickButton3)))
			{
				Move (6);
			}
			else if(Input.GetKey(KeyCode.RightArrow)&&Input.GetKey(KeyCode.DownArrow)||(Input.GetKey(KeyCode.JoystickButton0))&& (Input.GetKey(KeyCode.JoystickButton1)))
			{
				Move(5);
			}
			else if(Input.GetKey(KeyCode.LeftArrow)&&Input.GetKey(KeyCode.DownArrow)|| (Input.GetKey(KeyCode.JoystickButton4))&& (Input.GetKey(KeyCode.JoystickButton1)))
			{
				Move(4);
			}
			else if((Input.GetKey(KeyCode.RightArrow))|| (Input.GetKey(KeyCode.JoystickButton0)))
			{
				Move (3);
			}
			else if((Input.GetKey(KeyCode.DownArrow))||(Input.GetKey(KeyCode.JoystickButton1)))
			{
				Move (2);
			}
			else if((Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.JoystickButton4)))
			{
				Move (1);
			}
			else
			{
				Move(0);
			}
		}

		//Check if obstacle collision flag should be reset
		if(!CanCollide)
		{
			//Invulnerable time limit has been reached so reset flag
			if(Time.time > _collisionTimer)
			{
				CanCollide = true;
				_collisionTimer = 0.0f;
			}
		}
	}

	/// <summary>
	/// Moves the gameobject in the specified direction
	/// </summary>
	/// <param name="moveDir">Move direction.  0 is none so float, 1 is left, 2 is down, 3 is right, 4 is left and down, 5 is right and down</param>
	void Move(int moveDir)
	{
		Vector3 pos = this.gameObject.transform.position;

		switch(moveDir) 
		{
			case(6):
				this.gameObject.transform.position = new Vector3(pos.x, pos.y + (moveSpeedFloat*Time.deltaTime)+(moveSpeedVert*Time.deltaTime), pos.z);
			break;
			case(5):
				//Update gameobject's pos based on adjusted transform
				this.gameObject.transform.position = new Vector3(pos.x+(moveSpeedHoriz*Time.deltaTime), pos.y - (moveSpeedVert*Time.deltaTime), pos.z);
			break;
			case(4):
				this.gameObject.transform.position = new Vector3(pos.x-(moveSpeedHoriz*Time.deltaTime), pos.y - (moveSpeedVert*Time.deltaTime), pos.z);
			break;
			case(3):
				this.gameObject.transform.position = new Vector3(pos.x+(moveSpeedHoriz*Time.deltaTime), pos.y, pos.z);
			break;
			case(2):
			this.gameObject.transform.position = new Vector3(pos.x, pos.y - (moveSpeedVert*Time.deltaTime), pos.z);
			break;
			case(1):
				this.gameObject.transform.position = new Vector3(pos.x-(moveSpeedHoriz*Time.deltaTime), pos.y, pos.z);
			break;
			case(0):
				this.gameObject.transform.position = new Vector3(pos.x, pos.y + (moveSpeedFloat*Time.deltaTime), pos.z);
			break;
		}

	}
	

	void OnTriggerEnter2D(Collider2D other)
	{
		/*If collided object is a collectable object*/
		if(other.gameObject.tag == "CollectableObj")
		{
			Debug.Log("Object Hit.");
			//if object hasn't been collected, then collect it
			if(!other.gameObject.GetComponent<CollectableItem>().Collected && other.gameObject.GetComponent<CollectableItem>().CanCollide)
			{
				items.CollectItem(other.gameObject);
				Debug.Log("Object collected.");
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Obstacle" && this._canCollide)
		{
			this._canCollide = false;
			Debug.Log("HIT OBSTACLE");
			CollideObstacle(col.gameObject);
		}
	}





	void CollideObstacle(GameObject obstacle)
	{
		if(!colCoroutineStarted)
		{
			StartCoroutine("CollisionMove",obstacle);
		}

		//Handle collision timer stuff
		_collisionTimer = Time.time + invulnerableTime;

		if(items.collectedItems.Count>0)
		{
			//Detach closest item from items list
			items.DetachItemIndex (0);
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

	/// <summary>
	/// Gets a value indicating whether this instance can collide with a negative object
	/// </summary>
	/// <value><c>true</c> if this instance can collide; otherwise, <c>false</c>.</value>
	public bool CanCollide
	{
		get{return this._canCollide;}
		set{ this._canCollide = value;}
	}
	
}

