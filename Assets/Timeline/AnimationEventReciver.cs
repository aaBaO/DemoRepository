using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimationEventReciver : MonoBehaviour {

	public PlayableDirector playableDirector;

	// Use this for initialization
	void Start () {
		if(playableDirector != null)
			playableDirector.Play(playableDirector.playableAsset, DirectorWrapMode.Loop);
	}

	void FootR(){
		// Debug.Log("FootR");	
	}

	void FootL(){
		// Debug.Log("FootL");	
	}

	void Jump(){

	}

	void Land(){

	}
}
