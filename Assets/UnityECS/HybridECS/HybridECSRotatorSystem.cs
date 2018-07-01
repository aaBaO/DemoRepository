using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class HybridECSRotator : MonoBehaviour
{
	public float rotateSpeed = 100;
}

public class HybridECSRotatorSystem : ComponentSystem
{
	struct CubeEntity
	{
		public Transform transform;
		public HybridECSRotator rotator;
	}

	override protected void OnUpdate()
	{
		float deltaTime = Time.deltaTime;
		foreach (var cubeEntity in GetEntities<CubeEntity>())
		{
			cubeEntity.transform.rotation *= Quaternion.AngleAxis(deltaTime * cubeEntity.rotator.rotateSpeed, Vector3.up);	
		}	
	}
}

