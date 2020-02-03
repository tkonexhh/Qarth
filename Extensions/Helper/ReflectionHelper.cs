using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace GameWish.Game
{
    public class ReflectionHelper
    {
        public static Assembly GetAssembly()
        {
            Assembly[] AssbyCustmList = System.AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < AssbyCustmList.Length; i++)
            {
                string assbyName = AssbyCustmList[i].GetName().Name;
                if (assbyName == "Assembly-CSharp")
                {
                    return AssbyCustmList[i];
                }
            }
            return null;
        }
    }

}