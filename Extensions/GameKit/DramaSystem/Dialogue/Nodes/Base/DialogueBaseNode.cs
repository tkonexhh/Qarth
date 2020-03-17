using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


namespace GFrame.Drama
{
    public abstract class DialogueBaseNode : Node
    {

        abstract public void Enter(IPlayerForNode player);
        abstract public void Exit(IPlayerForNode player);
        abstract public void DoNext(IPlayerForNode player);
        abstract public void DoNextWhitParam(IPlayerForNode player, params object[] args);

    }

    [System.Serializable]
    public class Connection
    {

    }

}