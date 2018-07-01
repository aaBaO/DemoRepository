using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldMonoBehaviour : MonoBehaviour 
{
	public float rotateSpeed = 100;

	void Update () 
	{
		transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * rotateSpeed, Vector3.up);	
	}
}
