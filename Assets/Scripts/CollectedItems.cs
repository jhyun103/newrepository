using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectedItems : MonoBehaviour {

	public List<GameObject> collectedItems;
	public Vector2[] posOffsets;

	GameObject player;
	GameObject lastItem;
	int collectionTier = 1;
	int offsetIndex = 0;

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

			/*Add object to list of collectedObjects*/
			this.collectedItems.Add(obj);

			Vector2 newPos = new Vector2();

			/*Set position offset for item -*/
			//Determine bounce direction
			Vector2 currentOffset = posOffsets [offsetIndex] * collectionTier;
			offsetIndex++;

			//establish target position
			Vector2 currPos = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
			Vector2 targetPos = new Vector2(currPos.x + currentOffset.x, currPos.y + currentOffset.y); //target position is current position plus bounce

			//If there is no last item, this is the first item so do offset from player
			newPos = targetPos;
			//Set the item's offset
			itemScript.posOffset = currentOffset;
			//Set the item's temporary target position
			itemScript.targetPos = newPos;
			//set the item's refObject - the player in this case
			itemScript.refObj = player;
			//Call object's GetCollected method 
			itemScript.GetCollected();

			if (lastItem != null){
				//set the lastItem's child object to be the current object for updating when objects are removed
				lastItem.GetComponent<CollectableItem>().childObj = itemScript.gameObject;
			}

			/*set last item to current*/
			lastItem = obj;

			//Set object's item index- it's place in the list
			itemScript.ItemIndex = this.collectedItems.Count-1;

			if (collectedItems.Count % posOffsets.Length == 0) {
				collectionTier++;
				offsetIndex = 0;
			}
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
