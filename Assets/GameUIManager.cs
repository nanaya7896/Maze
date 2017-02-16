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
		if (Input.GetKeyDown (KeyCode.G)) {
			itemMesh [0].material = material [0];
		}
		if (Input.GetKeyDown (KeyCode.H)) {
			itemMesh [1].material = material [1];
		}	
		if (Input.GetKeyDown (KeyCode.V)) {
			itemMesh [2].material = material [2];
		}	
		if (Input.GetKeyDown (KeyCode.B)) {
			itemMesh [3].material = material [3];
		}	
	}

	public void SetItemMaterial(int stageNum)
	{
		itemMesh [stageNum].material = material [stageNum];
	}

	public void ResetItem()
	{
		for (int i = 0; i < 4; i++) {
			itemMesh [i].material = material [4];
		}
	}

}
