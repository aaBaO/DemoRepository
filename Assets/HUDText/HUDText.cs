using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDText : MonoBehaviour {
	private RectTransform m_rect;

	private Vector3 m_startPos = Vector3.zero;

	public Vector3 posOffset;
    private Vector3 m_defaultPosOffset;

	private float m_startTime = 0;
	public float duration = 3;
	private Text m_text;
    public float seperateIndensity = 1;

    public AnimationCurve yAxisCurve = AnimationCurve.Linear(0,0,1,1);
    public AnimationCurve scaleCurve = AnimationCurve.Constant(0,1,1);

	// Use this for initialization
	void Awake() {
		m_rect = GetComponent<RectTransform>();	
		m_text = GetComponent<Text>();
	}

    public void Play(Vector3 startPos)
    {
        m_defaultPosOffset = posOffset;
        m_startPos = startPos;
        posOffset += Random.insideUnitSphere * seperateIndensity;
		m_startTime = Time.time;
        enabled = true;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
    }

    public void Stop()
    {
        enabled = false;
        SetText("");
        m_startPos = Vector3.zero;
        posOffset = m_defaultPosOffset;
        //m_rect.position = new Vector3(-1 * Screen.width, -1 * Screen.height);
        HUDTextColleciton.instance.HideHUDText(gameObject.GetInstanceID());
    }

    // Update is called once per frame
    public void Update() {

		float elpasedTime = Time.time - m_startTime;
		if(elpasedTime >= duration){
            Stop();
            return;
		}
        float normalizedTime = elpasedTime / duration;
		Vector3 offset = posOffset + Vector3.up * yAxisCurve.Evaluate(normalizedTime);
        Vector3 worldPos = m_startPos + offset;
        Vector3 viewport = Camera.main.WorldToViewportPoint(worldPos);
        if(viewport.z > 0)
            m_rect.position = new Vector3(viewport.x * Screen.width, viewport.y * Screen.height);

        transform.localScale = scaleCurve.Evaluate(normalizedTime) * Vector3.one;
    }

	public void SetText(string value){
		m_text.text = value;
	}
}
