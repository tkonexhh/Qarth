using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


namespace GFrame.Drama
{
    [CreateNodeMenu("GFrame/Drama System/Node/AppendString")]
    public class NodeAppendString : DialogueBaseNode
    {
        [Input] public string Text1;
        [Input] public string Text2;

        [Output(ShowBackingValue.Always, ConnectionType.Override)]
        public Connection Output;

        protected override void Init()
        {
            this.name = "Append String";
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "Output")
            {
                var t1 = GetInputValue<string>("Text1", this.Text1);
                var t2 = GetInputValue<string>("Text2", this.Text2);
                return t1 + t2;
            }
            return null;
        }

        public override void Enter(IPlayerForNode player) { }
        public override void Exit(IPlayerForNode player) { }
        public override void DoNext(IPlayerForNode player) { }
        public override void DoNextWhitParam(IPlayerForNode player, params object[] args) { }
    }

}