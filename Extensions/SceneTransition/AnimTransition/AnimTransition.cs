/************************
	FileName:/Qarth/Extensions/SceneTransition/AnimTransition/AnimTransition.cs
	CreateAuthor:xuhonghua
	CreateTime:3/26/2020 2:41:27 PM
************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GFrame
{
    public class AnimTransition
    {
        public class SceneTransitionState_Show : SceneTransitionFSM.SceneTransitionState_Show
        {

            private SceneTransitionControl m_Entity;
            private AddressableRes m_Handle;
            public override void Enter(SceneTransitionControl entity)
            {
                base.Enter(entity);
                m_Entity = entity;
                m_Handle = AddressableResMgr.S.LoadAssetAsync<GameObject>("AnimTransition", LoadAnimGo);
                //m_Overlay.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            }

            private void LoadAnimGo(GameObject result)
            {
                m_Handle.InstantiateAsync((go) =>
                {
                    go.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
                    go.transform.SetParent(m_Entity.transition.transform, false);
                    var anim = go.GetComponent<Animator>();//.GetCurrentAnimatorStateInfo(0);
                    //Debug.LogError(anim.IsName("Show"));
                    m_Entity.transition.StartCoroutine(WaitAnimOver(anim));

                });
            }

            private IEnumerator WaitAnimOver(Animator anim)
            {
                var info = anim.GetCurrentAnimatorStateInfo(0);
                while (info.normalizedTime < 1.0f)
                {
                    info = anim.GetCurrentAnimatorStateInfo(0);
                    yield return null;
                }
                yield return new WaitForEndOfFrame();
                m_Entity.SetState(SceneTransitionControl.SceneTransitionFSMStateID.Load);
            }



            public override void Exit(SceneTransitionControl entity)
            {
                //TODO 资源未卸载干净
                //AddressableResMgr.S.ReleaseRes(m_Handle);
            }
        }

        public class SceneTransitionState_Load : SceneTransitionFSM.SceneTransitionState_Load
        {
            public override void Enter(SceneTransitionControl entity)
            {
                AddressableResMgr.S.LoadSceneAsync("AddressDemo", (result) =>
                {
                    if (result.Scene != null)
                    {
                        entity.SetState(SceneTransitionControl.SceneTransitionFSMStateID.Hide);
                    }
                });
            }
        }

        public class SceneTransitionState_Hide : SceneTransitionFSM.SceneTransitionState_Hide
        {
            private SceneTransitionControl m_Entity;
            public override void Enter(SceneTransitionControl entity)
            {
                m_Entity = entity;
                Debug.LogError(entity.transition.transform);
                var root = entity.transition.transform.GetChild(0);
                Debug.LogError(root.name);
                var anim = root.GetComponent<Animator>();

                //AddressableResMgr.S.ReleaseRes

                anim.Play("Hide");
                entity.transition.StartCoroutine(WaitAnimOver(anim));
            }

            private IEnumerator WaitAnimOver(Animator anim)
            {
                var info = anim.GetCurrentAnimatorStateInfo(0);
                while (info.normalizedTime < 1.0f)
                {
                    info = anim.GetCurrentAnimatorStateInfo(0);
                    yield return null;
                }
                yield return new WaitForEndOfFrame();
                m_Entity.SetState(SceneTransitionControl.SceneTransitionFSMStateID.Clean);


            }

        }

    }

}