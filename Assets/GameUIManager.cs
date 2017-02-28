using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameUIManager : MonoBehaviour {


	[SerializeField,Header("UI描画用のアイテムを格納")]
	List<MeshRenderer> itemMesh =new List<MeshRenderer>();
	[SerializeField]
	List<Material> material =new List<Material>();

	public Image black;


	public void SetItemMaterial(int stageNum)
	{
		if (itemMesh [stageNum].material.ToString () == material [stageNum].ToString ()) {
			return;
		}
		itemMesh [stageNum].material = material [stageNum];
	}
	public void SetPoseTexture(float alpha)
	{
		black.color = new Color (0f, 0f, 0f, alpha);
	}

	public bool isGetAllItem()
	{
		int count = 0;
		for (int i = 0; i < 4; i++) {
			Debug.Log (itemMesh [i].material.ToString ());
			if (itemMesh [i].material.ToString() != material [4].ToString()) {
				count++;
			}
		}
		if (count == 4) {
			return true;
		}

		return false;
	}

	public void ResetItem()
	{
		black.color = new Color (0f, 0f, 0f, 0f);
		for (int i = 0; i < 4; i++) {
			itemMesh [i].material = material [4];
			itemMesh[i].material.name ="Black";
		}
	}

}
