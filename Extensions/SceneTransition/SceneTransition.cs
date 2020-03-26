/************************
	FileName:/Qarth/Extensions/SceneTransion/SceneTransition.cs
	CreateAuthor:xuhonghua
	CreateTime:3/25/2020 4:24:22 PM
************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using UnityEngine.SceneManagement;


namespace GFrame
{
    public class SceneTransition : MonoBehaviour
    {
        public string scene = "<Insert scene name>";

        public void StartTransition()
        {
            SceneTransitionControl control = new SceneTransitionControl();
            control.Init();
            control.transition = Transition.StartTransition();
            control.SetState(SceneTransitionControl.SceneTransitionFSMStateID.Show);
        }
    }


    public class SceneTransitionControl
    {
        public enum SceneTransitionFSMStateID
        {
            Show,
            Load,
            Hide,
            Clean,
        }

        private static SceneTransitionFSM m_StateMachine;

        public Transition transition
        {
            get;
            set;
        }

        public SceneTransitionControl()
        {
            m_StateMachine = new SceneTransitionFSM(this);
            m_StateMachine.stateFactory = new FSMStateFactory<SceneTransitionControl>(false);
        }

        public void Init()
        {
            m_StateMachine.stateFactory.RegisterState(SceneTransitionFSMStateID.Show, new FadeTransition.SceneTransitionState_Show());
            m_StateMachine.stateFactory.RegisterState(SceneTransitionFSMStateID.Load, new FadeTransition.SceneTransitionState_Load());
            m_StateMachine.stateFactory.RegisterState(SceneTransitionFSMStateID.Hide, new FadeTransition.SceneTransitionState_Hide());
            m_StateMachine.stateFactory.RegisterState(SceneTransitionFSMStateID.Clean, new SceneTransitionFSM.SceneTransitionState_Clean());
        }

        public void SetState(SceneTransitionFSMStateID state)
        {
            if (m_StateMachine == null) return;
            m_StateMachine.SetCurrentStateByID(state);
        }


    }




}