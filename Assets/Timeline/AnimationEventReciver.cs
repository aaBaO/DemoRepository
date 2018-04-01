using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimationEventReciver : MonoBehaviour {

	public PlayableDirector playableDirector;

	// Use this for initialization
	void Start () {
		playableDirector.Play(playableDirector.playableAsset, DirectorWrapMode.Loop);
	}

	void FootR(){
		Debug.Log("FootR");	
	}

	void FootL(){
		Debug.Log("FootL");	
	}
}
