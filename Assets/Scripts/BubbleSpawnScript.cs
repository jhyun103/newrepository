using UnityEngine;
using System.Collections;

public class BubbleSpawnScript : MonoBehaviour {

	public GameObject bubblePrefFive;

	public float bubbleSpawnTimeMin = 3.0f;
	public float bubbleSpawnTimeMax = 5.0f;
	public float nextSpawnTime = 1.0f;

	public bool doSpawn = true;

	/*Spawn locations*/
	public GameObject[] SpawnLocs;


	// Use this for initialization
	void Start () 
	{
		if(bubblePrefFive == null)
		{
			Debug.LogError("Bubble Five Prefab is missing! Check object reference.");
		}

		float interval = Random.Range (bubbleSpawnTimeMin, bubbleSpawnTimeMax);
		nextSpawnTime = Time.time + interval;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(doSpawn && Time.time > nextSpawnTime)
		{
			float interval = Random.Range (bubbleSpawnTimeMin, bubbleSpawnTimeMax);
			nextSpawnTime = Time.time + interval;
			SpawnBubble();
		}
	}


	public void SpawnBubble()
	{
		//generate random index for choosing spawn location
		int loc = Random.Range (1, SpawnLocs.Length);

		if(SpawnLocs[loc-1] == null)
		{
			Debug.LogError("Spawn locations array in bubble spawner is null at selected index!");
		}
		else
		{
			GameObject.Instantiate (bubblePrefFive,SpawnLocs[loc-1].transform.position,SpawnLocs[loc].transform.rotation);
		}
	}
}
