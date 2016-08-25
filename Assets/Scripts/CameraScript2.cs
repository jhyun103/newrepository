using UnityEngine;
using System.Collections;

public class CameraScript2 : MonoBehaviour {

	public GameObject character;
	public float cameraMoveTime = 1.0f;
	public bool coRoutineStarted = false;
	
	// Use this for initialization
	void Start () 
	{   //Cursor.visible = false;
		character = GameObject.FindGameObjectWithTag("Player");
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!coRoutineStarted) {
			
			coRoutineStarted = true;
			Vector2 pos = new Vector2(character.transform.position.x, character.transform.position.y);
			StartCoroutine("MoveCameraSmooth", pos);
			
		}
		
	}
	
	
	
	IEnumerator MoveCameraSmooth(Vector2 playerPos) 
	{
		
		Vector2 currPos = new Vector2 (this.transform.position.x, this.transform.position.y);
		Vector2 targetPos = new Vector2 (playerPos.x, playerPos.y);
		Vector2 updatePos = new Vector2 (0, 0);
		
		float i = 0.0f;
		float rate = 1.0f / cameraMoveTime;
		
		while(i < 1.0f)
		{
			i += Time.deltaTime * rate;
			updatePos = Vector2.Lerp(currPos,targetPos,i);
			
			this.transform.position = 
				new Vector3(updatePos.x,
				            updatePos.y,
				            this.transform.position.z);
			
			yield return null;
		}
		
		coRoutineStarted = false;
		
		
		//		if(Input.GetKeyDown(KeyCode.F))
		//		{
		//			Camera.main.transform.Translate(viewportWidth,0,0);
		//		}
	}
}
