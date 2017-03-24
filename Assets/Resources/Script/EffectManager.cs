using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

	[SerializeField]
	List<GameObject> particle =new List<GameObject>();


	// Use this for initialization
	void Start () {
		//リソースフォルダの中のパーティクルを全て読み込む
		foreach (GameObject obj in Resources.LoadAll("Prefab/Particle/")) {
			particle.Add (obj);
		}
	}


	public void CreateParticle (string path, Vector3 position)
	{
		GameObject obj = null;
		switch (path) {
		case "Shine":
			{
				obj = Instantiate (particle [1].gameObject, position, Quaternion.identity);
				obj.transform.SetParent (this.transform);
				obj.layer = LayerMask.NameToLayer ("Particle");
			}
			break;
		default:
			break;
		}
	}


	// Update is called once per frame
	void Update () {
		
	}

	public void AllDeleteParticle()
	{
		for (int i = 0; i < transform.childCount; i++) {
			Destroy(transform.GetChild(i).gameObject);
		}
	}
}
