using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class LightMixer : PlayableBehaviour
{
	private float m_defaultRange = 1;
	private float m_defaultIntensity = 1;
	private Color m_defaultColor = Color.white;
	private Quaternion m_defaultRotation = Quaternion.identity;
	private	Light m_trackBinding;		

	private bool m_isFirstFrameProcess = true;

	// Called when the owning graph starts playing
	public override void OnGraphStart(Playable playable) {
	}

	// Called when the owning graph stops playing
	public override void OnGraphStop(Playable playable) {
		m_isFirstFrameProcess = false;
		if(m_trackBinding == null)
			return;
		m_trackBinding.range = m_defaultRange;
		m_trackBinding.color = m_defaultColor;
		m_trackBinding.intensity = m_defaultIntensity;
		m_trackBinding.transform.rotation = m_defaultRotation;
	}

	// Called when the state of the playable is set to Play
	public override void OnBehaviourPlay(Playable playable, FrameData info) {
	}

	// Called when the state of the playable is set to Paused
	public override void OnBehaviourPause(Playable playable, FrameData info) {
		
	}

	// Called each frame while the state is set to Play
	public override void PrepareFrame(Playable playable, FrameData info) {
	}

	public override void ProcessFrame(Playable playable, FrameData info, object playerData){
		m_trackBinding = playerData as Light;		
		if(m_trackBinding == null)
			return;

		if(!m_isFirstFrameProcess){
			m_isFirstFrameProcess = true;
			m_defaultRange = m_trackBinding.range;
			m_defaultColor = m_trackBinding.color;
			m_defaultIntensity = m_trackBinding.intensity;
			m_defaultRotation = m_trackBinding.transform.rotation;
		}

		int inputCount = playable.GetInputCount();
		float blendRange = 0;
		Color blendColor = Color.clear;
		float blendIntensity = 0;
		Quaternion blendRotation = Quaternion.identity;

		float totalWeight = 0;
		float greatestWeight = 0;
		int currentInputs = 0;

		for(int i = 0; i < inputCount; i++){
			ScriptPlayable<LightData> playableInput = (ScriptPlayable<LightData>)playable.GetInput(i);
			LightData input = playableInput.GetBehaviour();

			float inputWeight = playable.GetInputWeight(i);
			blendRange += input.range * inputWeight;
			blendColor += input.color * inputWeight;
			blendIntensity += input.intensity * inputWeight;
			if(input.lookTarget != null && inputWeight > 0)
				blendRotation *= Quaternion.LookRotation((input.lookTarget.position - m_trackBinding.transform.position) * inputWeight); 

			totalWeight += inputWeight;

			if(inputWeight > greatestWeight)
				greatestWeight = inputWeight;
			
			if(!Mathf.Approximately(inputWeight, 0f))
				currentInputs++;
		}

		m_trackBinding.range = blendRange + m_defaultRange * (1 - totalWeight);
		m_trackBinding.color = blendColor + m_defaultColor * (1 - totalWeight);
		m_trackBinding.intensity = blendIntensity + m_defaultIntensity * (1 - totalWeight);
		m_trackBinding.transform.rotation =  blendRotation;
	}
}
