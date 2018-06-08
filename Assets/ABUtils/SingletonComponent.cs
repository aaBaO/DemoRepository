using UnityEngine;

public class SingletonComponent<T> : MonoBehaviour where T : MonoBehaviour
{
    private static GameObject m_rootGameObject;
    private static T m_instance;
    public static T instance
    {
        get
        {
            if(m_instance == null)
            {
                Init();
            }
            return m_instance;
        }
    }

    public static void Init()
    {
        m_rootGameObject = GameObject.Find("SingletonComponentRoot");
        if (m_rootGameObject == null)
        {
        m_rootGameObject = new GameObject("SingletonComponentRoot");
        DontDestroyOnLoad(m_rootGameObject);
        }

        m_instance = new GameObject(typeof(T).Name).AddComponent<T>();
        m_instance.transform.SetParent(m_rootGameObject.transform, false);
    }
}
