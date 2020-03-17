using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;


namespace  GFrame.Drama
{

    [CustomNodeEditor(typeof(DialogueFinish))]
    public class DialogueFinishEditor : NodeEditor
    {
        public override Color GetTint()
        {
            if (EditorGUIUtility.isProSkin)
            {
                //黑色界面
                return new Color(0 / 255f, 120f / 255f, 200f, 1f);
            }
            else
            {
                //浅色界面
                return new Color(255f / 255f, 192f / 255f, 203f / 255f, 1f);
            }
        }

        public override int GetWidth()
        {
            return 200;
        }
    }
}
