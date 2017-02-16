using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour {


	[SerializeField,Header("UI描画用のアイテムを格納")]
	List<MeshRenderer> itemMesh =new List<MeshRenderer>();
	[SerializeField]
	List<Material> material =new List<Material>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetItemMaterial(int stageNum)
	{
		if (itemMesh [stageNum].material.ToString () == material [stageNum].ToString ()) {
			return;
		}
		itemMesh [stageNum].material = material [stageNum];
	}

	public void ResetItem()
	{
		for (int i = 0; i < 4; i++) {
			itemMesh [i].material = material [4];
		}
	}

}
