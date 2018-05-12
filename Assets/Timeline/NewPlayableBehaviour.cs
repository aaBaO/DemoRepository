using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// A behaviour that is attached to a playable
public class NewPlayableBehaviour : PlayableAsset, IPlayableBehaviour 
{
	public ExposedReference<Transform> lookTarget;
	// Factory method that generates a playable based on this asset
	public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
		return Playable.Create(graph);
	}
	// Called when the owning graph starts playing
	public void OnGraphStart(Playable playable) {
		
	}

	// Called when the owning graph stops playing
	public void OnGraphStop(Playable playable) {
		
	}

	// Called when the state of the playable is set to Play
	public void OnBehaviourPlay(Playable playable, FrameData info) {
		Debug.Log("OnBehaviourPlay");		
	}

	// Called when the state of the playable is set to Paused
	public void OnBehaviourPause(Playable playable, FrameData info) {
		
	}

	// Called each frame while the state is set to Play
	public void PrepareFrame(Playable playable, FrameData info) {
		
	}

    public void OnPlayableCreate(Playable playable)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayableDestroy(Playable playable)
    {
        throw new System.NotImplementedException();
    }

    public void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        throw new System.NotImplementedException();
    }
}
