using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;
using XNode;

namespace  GFrame.Drama
{
    [CustomNodeEditor(typeof(DialogueChoose))]
    public class DialogueChooseEditor : NodeEditor
    {

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            //NodeEditorGUILayout.PortField(target.GetInputPort("Input"), GUILayout.Width(100));
            //标题
            //NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Content"));

            //选项列表
            NodeEditorGUILayout.DynamicPortList("Chooses", typeof(DialogueChoose.Choose), serializedObject, NodePort.IO.Output, Node.ConnectionType.Override);
        }

        public override int GetWidth()
        {
            return 300;
        }

        public override Color GetTint()
        {
            return new Color(70f / 255f, 110f / 255f, 153f / 255f, 1f);
        }
    }
}
