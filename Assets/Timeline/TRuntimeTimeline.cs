using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine;

public class TRuntimeTimeline : MonoBehaviour {

    public TimelineAsset timelineAsset;
    private PlayableDirector m_PlayableDirector;

	// Use this for initialization
	void Start () {
        timelineAsset = new TimelineAsset();

        LightControlTrack createTrack = timelineAsset.CreateTrack<LightControlTrack>(null, "TestLightControlTrack");
        TimelineClip createClip = createTrack.CreateClip<LightClip>();
        LightClip lightClip = createClip.asset as LightClip;
        LightData lightData = lightClip.templete;
        lightData.color = Color.yellow; 

        m_PlayableDirector = gameObject.AddComponent<PlayableDirector>();
        m_PlayableDirector.playableAsset = timelineAsset;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
