/************************
	FileName:/Qarth/Extensions/Transition/FadeTransition/FadeTransition.cs
	CreateAuthor:xuhonghua
	CreateTime:3/25/2020 4:47:20 PM
************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;
namespace GFrame
{
    public class FadeTransition
    {
        public class SceneTransitionState_Show : SceneTransitionFSM.SceneTransitionState_Show
        {
            private Color m_StartColor = Color.black;
            private float m_FadeTime = 1.0f;

            public override void Enter(SceneTransitionControl entity)
            {
                base.Enter(entity);

                var bgTex = new Texture2D(1, 1);
                bgTex.SetPixel(0, 0, m_StartColor);
                bgTex.Apply();

                var image = m_Overlay.AddComponent<Image>();
                var rect = new Rect(0, 0, bgTex.width, bgTex.height);
                var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
                image.sprite = sprite;
                image.canvasRenderer.SetAlpha(0);
                m_Overlay.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
                entity.transition.StartCoroutine(FadeAnimIn(image, entity));
            }

            private IEnumerator FadeAnimIn(Image image, SceneTransitionControl entity)
            {
                float time = 0.0f;
                while (time < m_FadeTime)
                {
                    time += Time.deltaTime;
                    image.canvasRenderer.SetAlpha(Mathf.InverseLerp(0, 1, time / m_FadeTime));
                    yield return new WaitForEndOfFrame();
                }

                image.canvasRenderer.SetAlpha(1);

                yield return new WaitForEndOfFrame();
                entity.SetState(SceneTransitionControl.SceneTransitionFSMStateID.Load);
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
            private float m_FadeTime = 1.0f;
            public override void Enter(SceneTransitionControl entity)
            {
                var root = entity.transition.transform.GetChild(0);
                var image = root.GetComponent<Image>();
                entity.transition.StartCoroutine(FadeAnimOut(image, entity));
            }

            private IEnumerator FadeAnimOut(Image image, SceneTransitionControl entity)
            {
                float time = 0.0f;
                while (time < m_FadeTime)
                {
                    time += Time.deltaTime;
                    image.canvasRenderer.SetAlpha(Mathf.InverseLerp(1, 0, time / m_FadeTime));
                    yield return new WaitForEndOfFrame();
                }

                image.canvasRenderer.SetAlpha(1);

                yield return new WaitForEndOfFrame();
                entity.SetState(SceneTransitionControl.SceneTransitionFSMStateID.Clean);
            }
        }

    }



}