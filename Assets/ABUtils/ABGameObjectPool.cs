using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace ABUtils
{
    public class GameObjectTag
    {
        public string name;
        public string assetPath;
        public GameObject collectionParent;

        public GameObjectTag(string name, string assetPath)
        {
            this.name = name;
            this.assetPath = assetPath;
        }

        public bool isError()
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(assetPath))
                return false;

            if (!File.Exists(assetPath))
                return false;

            return true;
        }
    }

    public class ABGameObjectPool : Singleton<ABGameObjectPool>
    {
        /*
                --- InstanceID: GameObject
        Tag ------- InstanceID: GameObject
                --- InstanceID: GameObject
         */
        private static Dictionary<GameObjectTag, Dictionary<int, GameObject>> m_gameObjectMap = 
            new Dictionary<GameObjectTag, Dictionary<int, GameObject>>();
        public static Dictionary<GameObjectTag, Dictionary<int, GameObject>> gameObjectMap
        {
            get { return m_gameObjectMap; }
        }

        private static Dictionary<string, GameObject> m_assetMap = new Dictionary<string, GameObject>();

        private GameObject m_poolGameObject;

        public void Init()
        {
            if(m_poolGameObject == null)
            {
                m_poolGameObject = new GameObject("ABFooPool");
                MonoBehaviour.DontDestroyOnLoad(m_poolGameObject);
            }
        }

        public bool Alloc(GameObjectTag tag, int initialNumber = 1)
        {
            if (tag.isError())
            {
                Debug.LogError("Error GameObejct Tag");
                return false;
            }

            if (!m_gameObjectMap.ContainsKey(tag))
            {
                m_gameObjectMap.Add(tag, new Dictionary<int, GameObject>());
                GameObject tagCollectionObj = new GameObject(string.Format("Tag Name:[{0}]", tag.name));
                tagCollectionObj.transform.SetParent(m_poolGameObject.transform, false);
                tag.collectionParent = tagCollectionObj;
            }

            for(int i = 0; i < initialNumber; i++)
            {
                GameObject preLoadObj = null;
                if (InitGameObject(tag, out preLoadObj))
                {
                    if(!m_gameObjectMap[tag].ContainsKey(preLoadObj.GetInstanceID()))
                        m_gameObjectMap[tag].Add(preLoadObj.GetInstanceID(), preLoadObj);
                    Debug.Log(string.Format("PreLoad instanceID:{0}:{1}", preLoadObj.GetInstanceID(), preLoadObj.name));
                }
            }

            return true;
        }

        public bool TryUse(GameObjectTag tag, out GameObject result)
        {
            result = null;
            if (tag.isError())
            {
                Debug.LogError("Error GameObejct Tag");
                return false;
            }
            if (!m_gameObjectMap.ContainsKey(tag))
            {
                Debug.LogError("Tag has not LogIn to the Pool");
                return false;
            }
            
            foreach(var pair in m_gameObjectMap[tag])
            {
                if (!pair.Value.activeInHierarchy)
                    result = pair.Value;
            }

            if(result == null)
            {
                if(InitGameObject(tag, out result))
                {
                    if(!m_gameObjectMap[tag].ContainsKey(result.GetInstanceID()))
                        m_gameObjectMap[tag].Add(result.GetInstanceID(), result);
                }
            }

            return true;
        }

        private bool InitGameObject(GameObjectTag tag, out GameObject result)
        {
            result = null;
            //Get Asset from loaded AssetMap
            GameObject asset = null;
            if (m_assetMap.ContainsKey(tag.assetPath))
                asset = m_assetMap[tag.assetPath];
            else
                asset = Resources.Load<GameObject>(tag.assetPath);
            if (asset == null)
            {
                Debug.Log("Asset error");
                return false;
            }

            //Init a GameObject
            result = GameObject.Instantiate(asset);
            result.transform.SetParent(tag.collectionParent.transform, false);
            result.SetActive(false);
            result.name = tag.name;
            return true;
        }

        #region Collection

        /// <summary>
        /// Collect specific GameObjectTag in the pool
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool Collect(GameObjectTag tag)
        {
            if (tag.isError())
            {
                Debug.LogError("Error GameObejct Tag");
                return false;
            }

            return true;
        }

        public bool Collect(GameObjectTag tag, int instanceID)
        {
            if (tag.isError())
            {
                Debug.LogError("Error GameObejct Tag");
                return false;
            }

            Dictionary<int, GameObject> gameObejcts = m_gameObjectMap[tag];
            GameObject result = null;
            if (gameObejcts.TryGetValue(instanceID, out result))
            {
                result.SetActive(false);
                result.transform.SetParent(tag.collectionParent.transform, false);
                return true;
            }
            else
            {
                Debug.LogError(string.Format("can not find this instance in map [{0}]", instanceID));
                return false;
            }
        }

        public bool Collect(GameObjectTag tag, GameObject obj)
        {
            return Collect(tag, obj.GetInstanceID());
        }

        /// <summary>
        /// Collect all GameObject in the pool
        /// </summary>
        public void Collect()
        {

        }
        #endregion

        public void Clear()
        {

        }

        public void Clear(GameObjectTag tag)
        {

        }
    }
}
