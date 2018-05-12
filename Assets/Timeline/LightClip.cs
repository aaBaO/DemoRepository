using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class LightClip : PlayableAsset
{
	public LightData templete = new LightData();
	public ExposedReference<Transform> lookTarget;
	// Factory method that generates a playable based on this asset
	public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
		var playable = ScriptPlayable<LightData>.Create(graph, templete); 
		LightData clone = playable.GetBehaviour();
		clone.lookTarget = lookTarget.Resolve(graph.GetResolver());
		return playable;
	}
}
