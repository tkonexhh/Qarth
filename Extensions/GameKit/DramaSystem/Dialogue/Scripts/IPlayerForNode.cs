using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace  GFrame.Drama
{
    public interface IPlayerForNode
    {
        /// <summary>
        /// 执行一个普通内容对话
        /// </summary>
        /// <param name="content"></param>
        void DoContent(DialogueContent content);


        /// <summary>
        /// 执行一个选择分支对话
        /// </summary>
        /// <param name="choose"></param>
        void DoChoose(DialogueChoose choose);

        /// <summary>
        /// 结束
        /// </summary>
        /// <param name="param"></param>
        void DoFinish(DialogueFinish finish);
    }

}