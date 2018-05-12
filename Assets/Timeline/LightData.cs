using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class LightData : PlayableBehaviour{
	
	public float range;
	public Color color;
	public float intensity;
	[HideInInspector]
	public Transform lookTarget;

	public override void OnPlayableCreate(Playable playable){
		var duration = playable.GetDuration();
		if(Mathf.Approximately((float)duration, 0)){
			throw new UnityException("A Clip Cannot have a duration of zero");
		}
	}

}
