using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectedItems : MonoBehaviour {

	public List<GameObject> collectedItems;
	GameObject player;
	GameObject lastItem;
	Vector2 posOffset = new Vector2(-0.75f,1.00f);

	void Awake()
	{
		/*Initialize variables*/
		collectedItems = new List<GameObject> ();
		/*Get needed references*/
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach(GameObject obj in collectedItems)
		{
			obj.GetComponent<CollectableItem>().UpdatePos();
		}
	}

	public void CollectItem(GameObject obj)
	{
		/*grab script*/
		CollectableItem itemScript = obj.GetComponent<CollectableItem> ();

		if(itemScript != null)
		{
			/*Flag item as collected to avoid repeat calls*/
			itemScript.Collected = true;
			Vector2 newPos = new Vector2();

			/*Set position offset for item -*/
			if(lastItem == null)
			{
				//If there is no last item, this is the first item so do offset from player
				newPos = new Vector2 (player.transform.position.x + posOffset.x, player.transform.position.y + posOffset.y);
				//Set the item's offset
				itemScript.posOffset = posOffset;
				//Set the item's temporary target position
				itemScript.targetPos = newPos;
				//set the item's refObject - the player in this case
				itemScript.refObj = player;
				//Call object's GetCollected method 
				itemScript.GetCollected();
			}
			else
			{
				//There is a last item so calculate offset from it's position
				newPos = new Vector2 (lastItem.transform.position.x + posOffset.x, lastItem.transform.position.y + posOffset.y);
				//Set the item's offset
				itemScript.posOffset = posOffset;
				//Set the item's temporary target position
				itemScript.targetPos = newPos;
				//set the item's refObject - the player in this case
				itemScript.refObj = lastItem;
				//set the lastItem's child object to be the current object for updating when objects are removed
				lastItem.GetComponent<CollectableItem>().childObj = itemScript.gameObject;
				//Call object's GetCollected method 
				itemScript.GetCollected();
			}

			/*set last item to current*/
			lastItem = obj;

			/*Add object to list of collectedObjects*/
			this.collectedItems.Add(obj);
			//Set object's item index- it's place in the list
			itemScript.ItemIndex = this.collectedItems.Count-1;
		}
		else
		{
			Debug.LogException(new System.Exception("No collectableItem script found!"));
		}
	}

	/// <summary>
	/// Detachs the item at the given index
	/// </summary>
	/// <returns>The item.</returns>
	/// <param name="index">Index.</param>
	public CollectableItem DetachItemIndex (int index)
	{
		//Get reference to item
		GameObject obj = this.collectedItems [index];
		//Remove item from list
		this.collectedItems.RemoveAt (index);
		//Call detach method on removed item to ensure flags are set
		obj.GetComponent<CollectableItem> ().GetDetached ();
		//update last item reference - needed to correctly position objects
		if(this.collectedItems.Count > 0)
		{
			lastItem = this.collectedItems[this.collectedItems.Count-1];
		}
		else
		{
			lastItem = null;
		}

		return obj.GetComponent<CollectableItem> ();
	}
}
