using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshComponentsDemoUI : MonoBehaviour {

	public GameObject freeMoveObj;
	public GameObject dynamicNavMeshObj;

	public void ViewFreeMove(){
		freeMoveObj.SetActive(true);
		dynamicNavMeshObj.SetActive(false);
	}

	public void ViewDynamicNavMesh(){
		freeMoveObj.SetActive(false);
		dynamicNavMeshObj.SetActive(true);
	}
}
