using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {


	bool isRun=false;
	bool isAttack=false;
	bool isHit=false;
	int MaxHP=10;

	public Transform target;
	public float speed = 0.1f;


	Animator anim;
	void Start()
	{
		anim = this.GetComponent<Animator> ();
		target = GameObject.FindWithTag("Player").transform;
	}

	public void EnemyMove()
	{
		//targetの方に少しずつ向きが変わる
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.position - transform.position), 0.3f);
		//targetに向かって進む
		transform.position += transform.forward * speed;
		//this.transform.position
	}

	public void setisRun(bool val)
	{
		isRun = val;
		anim.SetBool ("isRun", isRun);
	}

}
