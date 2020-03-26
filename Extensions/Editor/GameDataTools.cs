using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace GameWish.Game
{
    public class PlayerPrefTools
    {
        [MenuItem("Tools/GameData/Clear All Saved Data")]
        static public void ClearSavedData()
        {
            PlayerPrefs.DeleteAll();

            bool isHaveData = Directory.Exists(Application.persistentDataPath + "/cache");
            if (isHaveData)
            {
                Directory.Delete(Application.persistentDataPath + "/cache", true);
            }
        }
    }
}
