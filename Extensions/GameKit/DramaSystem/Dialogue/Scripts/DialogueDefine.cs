using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFrame.Drama
{
    public class DialogueDefine
    {
        public const string NODE_START = "Start";
        public const string NODE_START_OUT = "Start";
        public const string NODE_CONTENT_OUTPUT = "Output";
    }


    public enum DialogueState
    {
        None,
        Content,
        Choose,
        Finish,
    }

}