using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDTextColleciton : MonoBehaviour {

    private static HUDTextColleciton m_instance;
    public static HUDTextColleciton instance{
        get{
            if (m_instance == null)
                m_instance = new GameObject("HUDTextColleciton").AddComponent<HUDTextColleciton>();
            return m_instance;
        }
    }

    private GameObject m_UICanvas;

    public static ABUtils.GameObjectTag HUDTextTag = new ABUtils.GameObjectTag("HUDText", "UI/HUDText"); 

    public void Init()
    {
        ABUtils.ABGameObjectPool.GetInstance().Alloc(HUDTextTag, 5);
    }

    public void ShowHUDText(GameObject followTarget, string content)
    {
        if (m_UICanvas == null)
        {
            m_UICanvas = GameObject.FindObjectOfType<Canvas>().gameObject;
        }
        if (m_UICanvas == null)
            return;

        GameObject hudtextObj = null;
        if(!ABUtils.ABGameObjectPool.GetInstance().TryUse(HUDTextTag, out hudtextObj))
            return;

        HUDText hudtext = hudtextObj.GetComponent<HUDText>();
        if (hudtext == null)
            return;

		hudtext.transform.SetParent(m_UICanvas.transform);
        RenderTheBubble(hudtext, followTarget, content);
    }

    public void HideHUDText(int instanceID){
        ABUtils.ABGameObjectPool.GetInstance().Collect(HUDTextTag, instanceID);
    }

    void RenderTheBubble(HUDText hudtext, GameObject followTarget, string content){
		hudtext.SetText(content);
        hudtext.Play(followTarget.transform.position);
    }

    private void OnDestroy()
    {
        ABUtils.ABGameObjectPool.GetInstance().Clear(HUDTextTag);
    }
}
