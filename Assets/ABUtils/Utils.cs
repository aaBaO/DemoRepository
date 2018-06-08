using System;
using System.Collections.Generic;
using UnityEngine;

namespace ABUtils
{
    public class Utils
    {
        /// <summary>
        /// Find child in target transfrom
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="recursive">find recursively</param>
        /// <returns></returns>
        public static Transform FindInChild(Transform parent, string name, bool recursive = false)
        {
            if(recursive)
            {
                List<Transform> childList = new List<Transform>();
                childList.AddRange(parent.GetComponentsInChildren<Transform>());
                foreach(var child in childList)
                {
                    if(child.name.Equals(name))
                        return child;
                }
            }
            else
            {
                return parent.Find(name);
            }
            return null;
        }
    }
}
