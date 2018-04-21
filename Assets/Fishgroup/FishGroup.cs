using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGroup : MonoBehaviour {

    public List<GameObject> fishList = new List<GameObject>();
    /// <summary>
    /// 鱼群警戒距离
    /// </summary>
    public float escapeRadius = 5;
    /// <summary>
    /// 每条鱼的间隔
    /// </summary>
    public float perRadius = 1;
    public float neighborRadius = 5;
    /// <summary>
    /// 鱼的质量
    /// </summary>
    public float mass = 1;
    /// <summary>
    /// 鱼的逃跑速度
    /// </summary>
    public float speed = 1;

    private GameObject leaderFish;

    public Transform followPoint;

    private void Awake()
    {
    }

    // Use this for initialization
    void Start () {
        foreach(var fish in fishList)
        {
            FishGizmo fishGizmo = fish.GetComponent<FishGizmo>();
            if (fishGizmo == null)
                return;
            fishGizmo.SetProperty(escapeRadius, perRadius, neighborRadius, mass, speed, followPoint.transform);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    foreach(var fish in fishList)
        {
            // Move(fish);
            FishGizmo fishGizmo = fish.GetComponent<FishGizmo>();
            if (fishGizmo == null)
                return;
            fishGizmo.GetComponent<FishGizmo>().SetNeighbors(ref fishList);
            fishGizmo.MoveUpdate();
        }	
	}

    private void OnDrawGizmosSelected()
    {
        foreach(var fish in fishList)
        {
            FishGizmo fishGizmo = fish.GetComponent<FishGizmo>();
            if (fishGizmo == null)
                return;
            fishGizmo.SetProperty(escapeRadius, perRadius, neighborRadius, mass, speed);
            fishGizmo.DrawGizmos();
        }
    }
}
