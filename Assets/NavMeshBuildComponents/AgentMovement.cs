using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour {

	private NavMeshAgent m_agent;

	void Start () {
		m_agent = GetComponent<NavMeshAgent>();	
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)){
				m_agent.SetDestination(hit.point);
			}	
		}
	}
}
