/************************
	FileName:/Qarth/Extensions/SceneTransition/SceneTransitionFSM.cs
	CreateAuthor:xuhonghua
	CreateTime:3/26/2020 10:21:54 AM
************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GFrame
{
    public class SceneTransitionFSM : FSMStateMachine<SceneTransitionControl>
    {

        public SceneTransitionFSM(SceneTransitionControl transition) : base(transition)
        {

        }

        public class SceneTransitionState : FSMState<SceneTransitionControl>
        {
            //

            public override void Enter(SceneTransitionControl entity)
            {

            }

            public override void Exit(SceneTransitionControl entity)
            {

            }

            public override void Execute(SceneTransitionControl entity, float dt)
            {

            }
        }

        public class SceneTransitionState_Show : SceneTransitionState
        {

            public override void Enter(SceneTransitionControl entity)
            {

            }
        }

        public class SceneTransitionState_Load : SceneTransitionState
        {

        }

        public class SceneTransitionState_Hide : SceneTransitionState
        {

        }

        public class SceneTransitionState_Clean : SceneTransitionState
        {
            public override void Enter(SceneTransitionControl entity)
            {
                entity.transition.EndTransition();
            }
        }
    }

}