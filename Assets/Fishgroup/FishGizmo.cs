using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGizmo : MonoBehaviour {

    public float escapeRadius = 0;
    public float perRadius = 0;
    public float neighborRadius = 0;
    public float mass = 0;
    public float maxSpeed = 0;
    Vector3 currentVelocity = Vector3.zero;

    public Transform target;

    public List<GameObject> neighbors; 

    public void SetProperty(params object[] args)
    {
        escapeRadius = (float)args[0];
        perRadius = (float)args[1];
        neighborRadius = (float)args[2];
        mass = (float)args[3];
        maxSpeed = (float)args[4];
        if(args.Length > 5)
            target = (Transform)args[5];
    }

    public void DrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, escapeRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position, perRadius);
        Gizmos.DrawRay(gameObject.transform.position, gameObject.transform.forward * 2);
        if(target == null)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(gameObject.transform.position, target.position);
    }

    void OnDrawGizmosSelected()
    {
        DrawGizmos();
    }

    public Vector3 Separation(){
        Vector3 force = Vector3.zero;
        for(int i = 0; i < neighbors.Count; i++){
            if(IsNeighbor(neighbors[i])){
                var dir = gameObject.transform.position - neighbors[i].transform.position ; 
                force += Vector3.Normalize(dir) * perRadius / dir.magnitude; 
            }
        }
        return force;
    }

    public Vector3 Alignment(){
        Vector3 averageForwardDir = Vector3.zero;
        int count = 0;
        for(int i = 0; i < neighbors.Count; i++){
            if(IsNeighbor(neighbors[i])){
                count++;
                averageForwardDir += neighbors[i].transform.forward; 
            }
        }
        if(count > 0){
            averageForwardDir /= count;
            averageForwardDir -= gameObject.transform.forward;
        }
        return averageForwardDir;
    }

    public Vector3 Cohesion(){
        Vector3 centerOfMass = Vector3.zero;
        Vector3 steeringForce = Vector3.zero;
        int count = 0;
        for(int i = 0; i < neighbors.Count; i++){
            if(IsNeighbor(neighbors[i])){
                count++;
                centerOfMass += neighbors[i].transform.position;
            }
        }
        if(count > 0){
            centerOfMass /= count;
            steeringForce = Seek(centerOfMass);
        }
        return steeringForce;
    }

    public Vector3 Seek(Vector3 aim){
        Vector3 desiredVelocity = (aim - gameObject.transform.position).normalized * maxSpeed;
        Vector3 steeringForce = desiredVelocity - currentVelocity;
        return steeringForce;
    } 

    public Vector3 Arrive(Vector3 aim){
        Vector3 toAim = aim - gameObject.transform.position;
        float dist = toAim.magnitude;
        if(dist > 0) {
            Vector3 desiredVelocity = Vector3.zero;
            float speed = dist / 3;
            speed = Mathf.Min(speed, maxSpeed);
            desiredVelocity = toAim * speed / dist;
            return desiredVelocity - currentVelocity;
        }
        else {
            return Vector3.zero;
        }
    }

    public void MoveUpdate(){
        //steering force = caculate()
        //acceleration = force / mass;
        //velocity = acceleration * time.deltatime
        Vector3 steeringForce = Arrive(target.position) + Separation() + Alignment() + Cohesion();
        Vector3 acceleration = steeringForce / mass;
        currentVelocity += acceleration * Time.deltaTime;
        currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxSpeed);
        transform.position += currentVelocity * Time.deltaTime;
        if(currentVelocity.magnitude > 0)
            transform.rotation = Quaternion.LookRotation(currentVelocity, transform.up);
    }

    public void SetNeighbors(ref List<GameObject> group){
        List<GameObject> tempList = new List<GameObject>();
        for(int i = 0; i < group.Count; i++){
            if(group[i].Equals(gameObject))
                continue;

            var dis = Vector3.Distance(gameObject.transform.position,group[i].transform.position);
            if(dis <= neighborRadius)
                tempList.Add(group[i]);
        }
        neighbors.Clear();
        neighbors.AddRange(tempList);
    }

    private bool IsNeighbor(GameObject other){
        var distance = Vector3.Distance(gameObject.transform.position, other.transform.position);
        if(distance <= neighborRadius && neighbors.Contains(other))
            return true;
        else 
            return false;
    }
}
