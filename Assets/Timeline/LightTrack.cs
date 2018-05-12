using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(1f, 0f, 0f)]
[TrackClipType(typeof(LightClip))]
[TrackBindingType(typeof(Light))]
public class LightControlTrack : TrackAsset 
{
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount){

		return ScriptPlayable<LightMixer>.Create(graph, inputCount);

	}

}
