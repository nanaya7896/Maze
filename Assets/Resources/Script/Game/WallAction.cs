using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAction : MonoBehaviour {

	static public bool isKeyCheck=false;
	float fallSpeed=0.1f;
	float time=0f;
	Vector3 startPosition;
	GameObject wall;
	// Use this for initialization
	void Start () {
		
		isKeyCheck = false;
		setWallObject ("RockWall");

	}

	// Update is called once per frame
	void Update () {
		if (isKeyCheck) {
			wall.transform.position = new Vector3 (startPosition.x+ Random.Range (-0.1f, 0.1f), wall.transform.position.y - (fallSpeed * Time.deltaTime), startPosition.z+Random.Range (-0.1f, 0.1f));
			if (time > 10f) {
				isKeyCheck = false;
			}
			time += Time.deltaTime;
		}
	}


	public void setWallObject(string WallName)
	{
		wall = GameObject.FindWithTag (WallName);
		startPosition = wall.transform.position;
	}


}
