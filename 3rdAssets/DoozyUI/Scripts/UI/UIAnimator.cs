// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.AnimatedValues;
#endif

#if dUI_MasterAudio
using DarkTonic.MasterAudio;
#endif

namespace DoozyUI
{
    public class UIAnimator
    {
        /// <summary>
        /// Default duration set to an animation
        /// </summary>
        public const float DEFAULT_DURATION = 0.5f;
        /// <summary>
        /// Default start delay set to an animation
        /// </summary>
        public const float DEFAULT_START_DELAY = 0f;
        /// <summary>
        /// Default ease set to an animations
        /// </summary>
        public const Ease DEFAULT_EASE = Ease.Linear;
        /// <summary>
        /// Default loops set to a loop animation. -1 means infinite loops.
        /// </summary>
        public const int DEFAULT_LOOPS = -1;
        /// <summary>
        /// Default reset duration after a punch animation. This reset is needed to be sure the animation's initial values are restored.
        /// </summary>
        public const float DEFAULT_DURATION_ONCOMPLETE = 0.1f;
        /// <summary>
        /// Default loop setup duration. This is the time a loop animation is setup for it's cycle to start.
        /// </summary>
        public const float DEFAULT_DURATION_INIT_LOOP = 0.2f;
        /// <summary>
        /// Default target reset. This is the time a 'target' (rectTransfrom) is reset to it's start values (runtime values).
        /// </summary>
        public const float DEFAULT_DURATION_RESET_TARGET = 0.1f;
        /// <summary>
        /// Type of ease an animation, loop or punch should use.
        /// </summary>
        public enum EaseType { Ease = 0, AnimationCurve = 1 }
        /// <summary>
        /// Returns the reverse of the given ease.
        /// </summary>
        public static Ease Reverse(Ease ease)
        {
            switch(ease)
            {
                case Ease.Unset: return Ease.Unset;
                case Ease.Linear: return Ease.Linear;
                case Ease.InSine: return Ease.OutSine;
                case Ease.OutSine: return Ease.InSine;
                case Ease.InOutSine: return Ease.InOutSine;
                case Ease.InQuad: return Ease.OutQuad;
                case Ease.OutQuad: return Ease.InQuad;
                case Ease.InOutQuad: return Ease.InOutQuad;
                case Ease.InCubic: return Ease.OutCubic;
                case Ease.OutCubic: return Ease.InCubic;
                case Ease.InOutCubic: return Ease.InOutCubic;
                case Ease.InQuart: return Ease.OutQuart;
                case Ease.OutQuart: return Ease.InQuart;
                case Ease.InOutQuart: return Ease.InOutQuart;
                case Ease.InQuint: return Ease.OutQuint;
                case Ease.OutQuint: return Ease.InQuint;
                case Ease.InOutQuint: return Ease.InOutQuint;
                case Ease.InExpo: return Ease.OutExpo;
                case Ease.OutExpo: return Ease.InExpo;
                case Ease.InOutExpo: return Ease.InOutExpo;
                case Ease.InCirc: return Ease.OutCirc;
                case Ease.OutCirc: return Ease.InCirc;
                case Ease.InOutCirc: return Ease.InOutCirc;
                case Ease.InElastic: return Ease.OutElastic;
                case Ease.OutElastic: return Ease.InElastic;
                case Ease.InOutElastic: return Ease.InOutElastic;
                case Ease.InBack: return Ease.OutBack;
                case Ease.OutBack: return Ease.InBack;
                case Ease.InOutBack: return Ease.InOutBack;
                case Ease.InBounce: return Ease.OutBounce;
                case Ease.OutBounce: return Ease.InBounce;
                case Ease.InOutBounce: return Ease.InOutBounce;
                case Ease.Flash: return Ease.Flash;
                case Ease.InFlash: return Ease.OutFlash;
                case Ease.OutFlash: return Ease.InFlash;
                case Ease.InOutFlash: return Ease.InOutFlash;
                case Ease.INTERNAL_Zero: return Ease.INTERNAL_Zero;
                case Ease.INTERNAL_Custom: return Ease.INTERNAL_Custom;
                default: return Ease.Linear;
            }
        }
        /// <summary>
        /// Used to map the tween ids.
        /// </summary>
        public enum TweenIdType { Move = 0, Rotate = 1, Scale = 2, Fade = 3 }
        /// <summary>
        /// Used to map the tween ids.
        /// </summary>
        public enum TweenIdAnimation { In = 0, Out = 1, Loop = 2, Punch = 3, State = 4 }

        /// <summary>
        /// Should the UI ignore game timescale and work in realtime? Default is true.
        /// </summary>
        public static bool isTimeScaleIndependent = true;

        /// <summary>
        /// Returns the tween id of the given target with the given idType and idAnimation. This is a quick id generator.
        /// </summary>
        public static string GetTweenId(RectTransform target, TweenIdType idType, TweenIdAnimation idAnimation)
        {
            return target.GetInstanceID() + idType.ToString() + idAnimation.ToString();
        }

        /// <summary>
        /// Resets the given target (RectTransform) to the given start parameters (position, rotation, scale and alpha). By default this is an instant reset, but you can override the DEFAULT_DURATION_RESET_TARGET value and set instantAnimation to false, in order to animate this reset (not recommended).
        /// </summary>
        /// <param name="target"></param>
        /// <param name="startPosition"></param>
        /// <param name="startRotation"></param>
        /// <param name="startScale"></param>
        /// <param name="startAlpha"></param>
        /// <param name="instantAnimation"></param>
        public static void ResetTarget(RectTransform target, Vector3 startPosition, Vector3 startRotation, Vector3 startScale, float startAlpha, bool instantAnimation = true)
        {
            if(target == null) { return; }
            CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
            if(canvasGroup != null)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            if(instantAnimation)
            {
                target.anchoredPosition3D = startPosition;
                target.localRotation = Quaternion.Euler(startRotation);
                target.localScale = startScale;
                if(canvasGroup != null) { canvasGroup.alpha = startAlpha; }
                return;
            }
            target.DOAnchorPos3D(startPosition, DEFAULT_DURATION_RESET_TARGET).Play();
            target.DOLocalRotate(startRotation, DEFAULT_DURATION_RESET_TARGET).Play();
            //target.DOScale(startScale, DEFAULT_DURATION_RESET_TARGET).Play();
            if(canvasGroup != null) { canvasGroup.DOFade(startAlpha, DEFAULT_DURATION_RESET_TARGET).Play(); }
        }

        #region Animations
        /// <summary>
        /// Moves in or out a RectTransform by animating the anchoredPosition3D value.
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="startPosition">The initial position of the target.</param>
        /// <param name="animation">The animation settings.</param>
        /// <param name="OnStart">Callback listener.</param>
        /// <param name="OnComplete">Callback listener.</param>
        /// <param name="instantAnimation">If true, the animation will happen instantly (without creating a tween).</param>
        /// <param name="forced">If true, it will initiate this animation, regardless if it's enabled or not.</param>
        public static void Move(RectTransform target, Vector3 startPosition, Anim animation, UnityAction OnStart, UnityAction OnComplete, bool instantAnimation = false, bool forced = false)
        {
            if(!animation.move.enabled) { if(!forced) { return; } }
            string tweenId = "";
            Vector3 targetPosition = Vector3.zero;
            switch(animation.animationType)
            {
                case Anim.AnimationType.In:
                    targetPosition = startPosition;
                    target.anchoredPosition3D = GetTargetPosition(target, startPosition, animation);
                    tweenId = GetTweenId(target, TweenIdType.Move, TweenIdAnimation.In);
                    break;
                case Anim.AnimationType.Out:
                    targetPosition = GetTargetPosition(target, startPosition, animation);
                    target.anchoredPosition3D = startPosition;
                    tweenId = GetTweenId(target, TweenIdType.Move, TweenIdAnimation.Out);
                    break;
                case Anim.AnimationType.State:
                    targetPosition = startPosition + animation.move.customPosition;
                    tweenId = GetTweenId(target, TweenIdType.Move, TweenIdAnimation.State);
                    break;
            }
            if(instantAnimation)
            {
                target.anchoredPosition3D = targetPosition;
                //OnComplete.Invoke();
                return;
            }
            Tweener tween =
            target.DOAnchorPos3D(targetPosition, animation.move.duration, false)
                  .SetDelay(animation.move.startDelay)
                  .SetUpdate(true)
                  .SetId(tweenId)
                  .OnStart(() => { if(OnStart != null) { OnStart.Invoke(); } })
                  .OnComplete(() => { if(OnComplete != null) { OnComplete.Invoke(); } });
            switch(animation.move.easeType)
            {
                case EaseType.Ease: tween.SetEase(animation.move.ease); break;
                case EaseType.AnimationCurve: tween.SetEase(animation.move.animationCurve); break;
            }
            tween.Play();
        }
        /// <summary>
        /// Returns Move targetPosition taking into account the MoveDirection.
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="startPosition">The initial position of the target.</param>
        /// <param name="animation">The animation settings.</param>
        private static Vector3 GetTargetPosition(RectTransform target, Vector3 startPosition, Anim animation)
        {
            Canvas rootCanvas = target.GetComponent<Canvas>().rootCanvas;
            Rect rootCanvasRect = rootCanvas.GetComponent<RectTransform>().rect;
            float xOffset = rootCanvasRect.width / 2 + target.rect.width * target.pivot.x;
            float yOffset = rootCanvasRect.height / 2 + target.rect.height * target.pivot.y;
            switch(animation.move.moveDirection)
            {
                case DoozyUI.Move.MoveDirection.Left: return new Vector3(-xOffset, startPosition.y, startPosition.z);
                case DoozyUI.Move.MoveDirection.Right: return new Vector3(xOffset, startPosition.y, startPosition.z);
                case DoozyUI.Move.MoveDirection.Top: return new Vector3(startPosition.x, yOffset, startPosition.z);
                case DoozyUI.Move.MoveDirection.Bottom: return new Vector3(startPosition.x, -yOffset, startPosition.z);
                case DoozyUI.Move.MoveDirection.TopLeft: return new Vector3(-xOffset, yOffset, startPosition.z);
                case DoozyUI.Move.MoveDirection.TopCenter: return new Vector3(0, yOffset, startPosition.z);
                case DoozyUI.Move.MoveDirection.TopRight: return new Vector3(xOffset, yOffset, startPosition.z);
                case DoozyUI.Move.MoveDirection.MiddleLeft: return new Vector3(-xOffset, 0, startPosition.z);
                case DoozyUI.Move.MoveDirection.MiddleCenter: return new Vector3(0, 0, startPosition.z);
                case DoozyUI.Move.MoveDirection.MiddleRight: return new Vector3(xOffset, 0, startPosition.z);
                case DoozyUI.Move.MoveDirection.BottomLeft: return new Vector3(-xOffset, -yOffset, startPosition.z);
                case DoozyUI.Move.MoveDirection.BottomCenter: return new Vector3(0, -yOffset, startPosition.z);
                case DoozyUI.Move.MoveDirection.BottomRight: return new Vector3(xOffset, -yOffset, startPosition.z);
                case DoozyUI.Move.MoveDirection.CustomPosition: return animation.move.customPosition;
                default: return Vector3.zero;
            }

        }
        /// <summary>
        /// Rotates in or out a RectTransform by animating the localRotation value.
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="startRotation">The initial rotation of the target.</param>
        /// <param name="animation">The animation settings.</param>
        /// <param name="OnStart">Callback listener.</param>
        /// <param name="OnComplete">Callback listener.</param>
        /// <param name="instantAnimation">If true, the animation will happen instantly (without creating a tween).</param>
        /// <param name="forced">If true, it will initiate this animation, regardless if it's enabled or not.</param>
        public static void Rotate(RectTransform target, Vector3 startRotation, Anim animation, UnityAction OnStart, UnityAction OnComplete, bool instantAnimation = false, bool forced = false)
        {
            if(!animation.rotate.enabled) { if(!forced) { return; } }
            string tweenId = "";
            Vector3 targetRotation = Vector3.zero;
            switch(animation.animationType)
            {
                case Anim.AnimationType.In:
                    targetRotation = startRotation;
                    target.localRotation = Quaternion.Euler(animation.rotate.rotation);
                    tweenId = GetTweenId(target, TweenIdType.Rotate, TweenIdAnimation.In);
                    break;
                case Anim.AnimationType.Out:
                    targetRotation = animation.rotate.rotation;
                    target.localRotation = Quaternion.Euler(startRotation);
                    tweenId = GetTweenId(target, TweenIdType.Rotate, TweenIdAnimation.Out);
                    break;
                case Anim.AnimationType.State:
                    targetRotation = startRotation + Quaternion.Euler(animation.rotate.rotation).eulerAngles;
                    target.localRotation = Quaternion.Euler(startRotation);
                    tweenId = GetTweenId(target, TweenIdType.Rotate, TweenIdAnimation.State);
                    break;
            }
            if(instantAnimation)
            {
                target.localRotation = Quaternion.Euler(targetRotation);
                //OnComplete.Invoke();
                return;
            }
            Tweener tween =
            target.DOLocalRotate(targetRotation, animation.rotate.duration, animation.rotate.rotateMode)
                  .SetDelay(animation.rotate.startDelay)
                  .SetUpdate(isTimeScaleIndependent)
                  .SetId(tweenId)
                  .OnStart(() => { if(OnStart != null) { OnStart.Invoke(); } })
                  .OnComplete(() => { if(OnComplete != null) { OnComplete.Invoke(); } });
            switch(animation.rotate.easeType)
            {
                case EaseType.Ease: tween.SetEase(animation.rotate.ease); break;
                case EaseType.AnimationCurve: tween.SetEase(animation.rotate.animationCurve); break;
            }
            tween.Play();
        }
        /// <summary>
        /// Scales in or out a RectTransform by animating the localScale value.
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="startScale">The initial scale of the target.</param>
        /// <param name="animation">The animation settings.</param>
        /// <param name="OnStart">Callback listener.</param>
        /// <param name="OnComplete">Callback listener.</param>
        /// <param name="instantAnimation">If true, the animation will happen instantly (without creating a tween).</param>
        /// <param name="forced">If true, it will initiate this animation, regardless if it's enabled or not.</param>
        public static void Scale(RectTransform target, Vector3 startScale, Anim animation, UnityAction OnStart, UnityAction OnComplete, bool instantAnimation = false, bool forced = false)
        {
            if(!animation.scale.enabled) { if(!forced) { return; } }
            string tweenId = "";
            Vector3 targetScale = Vector3.one;
            switch(animation.animationType)
            {
                case Anim.AnimationType.In:
                    targetScale = startScale;
                    target.localScale = animation.scale.scale;
                    tweenId = GetTweenId(target, TweenIdType.Scale, TweenIdAnimation.In);
                    break;
                case Anim.AnimationType.Out:
                    targetScale = animation.scale.scale;
                    target.localScale = startScale;
                    tweenId = GetTweenId(target, TweenIdType.Scale, TweenIdAnimation.Out);
                    break;
                case Anim.AnimationType.State:
                    targetScale = startScale + animation.scale.scale;
                    tweenId = GetTweenId(target, TweenIdType.Scale, TweenIdAnimation.State);
                    break;
            }
            if(instantAnimation)
            {
                target.localScale = targetScale;
                //OnComplete.Invoke();
                return;
            }
            Tweener tween =
            target.DOScale(targetScale, animation.scale.duration)
                  .SetDelay(animation.scale.startDelay)
                  .SetUpdate(isTimeScaleIndependent)
                  .SetId(tweenId)
                  .OnStart(() => { if(OnStart != null) { OnStart.Invoke(); } })
                  .OnComplete(() => { if(OnComplete != null) { OnComplete.Invoke(); } });
            switch(animation.scale.easeType)
            {
                case EaseType.Ease: tween.SetEase(animation.scale.ease); break;
                case EaseType.AnimationCurve: tween.SetEase(animation.scale.animationCurve); break;
            }
            tween.Play();
        }
        /// <summary>
        /// Fades in or out a RectTransform (and it's children) by animating the alpha value of it's attached CanvasGroup. If a CanvasGroup is not attached to the target then it will automatically attach one for you.
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="startAlpha">CanvasGroup's start alpha. This is the animation's center.</param>
        /// <param name="animation">The animation settings.</param>
        /// <param name="OnStart">Callback listener.</param>
        /// <param name="OnComplete">Callback listener.</param>
        /// <param name="instantAnimation">If true, the animation will happen instantly (without creating a tween).</param>
        /// <param name="forced">If true, it will initiate this animation, regardless if it's enabled or not.</param>
        public static void Fade(RectTransform target, float startAlpha, Anim animation, UnityAction OnStart, UnityAction OnComplete, bool instantAnimation = false, bool forced = false)
        {
            if(!animation.fade.enabled) { if(!forced) { return; } }
            string tweenId = "";
            CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>() != null ? target.GetComponent<CanvasGroup>() : target.gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = startAlpha;
            float targetAlpha = 0;
            switch(animation.animationType)
            {
                case Anim.AnimationType.In:
                    targetAlpha = startAlpha;
                    canvasGroup.alpha = 0;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                    tweenId = GetTweenId(target, TweenIdType.Fade, TweenIdAnimation.In);
                    break;
                case Anim.AnimationType.Out:
                    targetAlpha = 0;
                    canvasGroup.alpha = startAlpha;
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                    tweenId = GetTweenId(target, TweenIdType.Fade, TweenIdAnimation.Out);
                    break;
                case Anim.AnimationType.State:
                    targetAlpha = animation.fade.alpha;
                    tweenId = GetTweenId(target, TweenIdType.Fade, TweenIdAnimation.State);
                    break;
            }
            if(instantAnimation)
            {
                canvasGroup.alpha = targetAlpha;
                //OnComplete.Invoke();
                return;
            }
            Tweener tween =
            canvasGroup.DOFade(targetAlpha, animation.fade.duration)
                       .SetDelay(animation.fade.startDelay)
                       .SetUpdate(isTimeScaleIndependent)
                       .SetId(tweenId)
                       .OnStart(() => { if(OnStart != null) { OnStart.Invoke(); } })
                       .OnComplete(() => { if(OnComplete != null) { OnComplete.Invoke(); } });
            switch(animation.fade.easeType)
            {
                case EaseType.Ease: tween.SetEase(animation.fade.ease); break;
                case EaseType.AnimationCurve: tween.SetEase(animation.fade.animationCurve); break;
            }
            tween.Play();
        }
        /// <summary>
        /// Stops all the running animations In and Out on the target (RectTransform). It uses the GetTweenId generator in order to get valid ids.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="aType"></param>
        public static void StopAnimations(RectTransform target, Anim.AnimationType aType)
        {
            if(target == null) { return; }
            switch(aType)
            {
                case Anim.AnimationType.In:
                    DOTween.Kill(GetTweenId(target, TweenIdType.Move, TweenIdAnimation.In));
                    DOTween.Kill(GetTweenId(target, TweenIdType.Rotate, TweenIdAnimation.In));
                    DOTween.Kill(GetTweenId(target, TweenIdType.Scale, TweenIdAnimation.In));
                    DOTween.Kill(GetTweenId(target, TweenIdType.Fade, TweenIdAnimation.In));
                    break;
                case Anim.AnimationType.Out:
                    DOTween.Kill(GetTweenId(target, TweenIdType.Move, TweenIdAnimation.Out));
                    DOTween.Kill(GetTweenId(target, TweenIdType.Rotate, TweenIdAnimation.Out));
                    DOTween.Kill(GetTweenId(target, TweenIdType.Scale, TweenIdAnimation.Out));
                    DOTween.Kill(GetTweenId(target, TweenIdType.Fade, TweenIdAnimation.Out));
                    break;
                case Anim.AnimationType.State:
                    DOTween.Kill(GetTweenId(target, TweenIdType.Move, TweenIdAnimation.State));
                    DOTween.Kill(GetTweenId(target, TweenIdType.Rotate, TweenIdAnimation.State));
                    DOTween.Kill(GetTweenId(target, TweenIdType.Scale, TweenIdAnimation.State));
                    DOTween.Kill(GetTweenId(target, TweenIdType.Fade, TweenIdAnimation.State));
                    break;
            }
        }
        #endregion

        #region Loops
        /// <summary>
        /// Creates a move Loop animation, but it does not start automatically unless the loop's autoStart variable is set to true.
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="startPosition">The initial position of the target. This is the animation's center.</param>
        /// <param name="loop">The loop animation settings.</param>
        /// <param name="OnStart">Callback listener.</param>
        /// <param name="OnComplete">Callback listener.</param>
        /// <param name="id">Adds an extra string to the loop's tween id. Used to differentiate several loops animations created for the same target.</param>
        /// <param name="forced">If true, it will initiate this animation, regardless if it's enabled or not.</param>
        public static void LoopMove(RectTransform target, Vector3 startPosition, Loop loop, UnityAction OnStart, UnityAction OnComplete, string id = "", bool forced = false)
        {
            if(!loop.move.enabled) { if(!forced) { return; } }
            // positionA <---> startPosition <---> poistionB
            Vector3 positionA = startPosition - loop.move.movement;
            Vector3 poistionB = startPosition + loop.move.movement;
            target.DOAnchorPos(positionA, DEFAULT_DURATION_INIT_LOOP)
                  .SetDelay(loop.move.startDelay)
                  .SetUpdate(true)
                  .SetId(GetTweenId(target, TweenIdType.Move, TweenIdAnimation.Loop) + id)
                  .OnStart(() => { if(OnStart != null) { OnStart.Invoke(); } })
                  .OnComplete(() =>
                  {
                      Tweener tween =
                      target.DOAnchorPos3D(poistionB, loop.move.duration);
                      tween.SetUpdate(isTimeScaleIndependent);
                      tween.SetId(GetTweenId(target, TweenIdType.Move, TweenIdAnimation.Loop) + id);
                      tween.SetLoops(loop.move.loops, Loop.GetLoopType(loop.move.loopType));
                      switch(loop.move.easeType)
                      {
                          case EaseType.Ease: tween.SetEase(loop.move.ease); break;
                          case EaseType.AnimationCurve: tween.SetEase(loop.move.animationCurve); break;
                      }
                      tween.OnComplete(() => { if(OnComplete != null) { OnComplete.Invoke(); } });
                      tween.Play();
                  })
                  .Pause();
            if(loop.autoStart) { DOTween.Play(GetTweenId(target, TweenIdType.Move, TweenIdAnimation.Loop) + id); }
        }
        /// <summary>
        /// Creates a rotation Loop animation, but it does not start automatically unless the loop's autoStart variable is set to true.
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="startRotation">The initial rotation of the target. This is the animation's center.</param>
        /// <param name="loop">The loop animation settings.</param>
        /// <param name="OnStart">Callback listener.</param>
        /// <param name="OnComplete">Callback listener.</param>
        /// <param name="id">Adds an extra string to the loop's tween id. Used to differentiate several loops animations created for the same target.</param>
        /// <param name="forced">If true, it will initiate this animation, regardless if it's enabled or not.</param>
        public static void LoopRotate(RectTransform target, Vector3 startRotation, Loop loop, UnityAction OnStart, UnityAction OnComplete, string id = "", bool forced = false)
        {
            if(!loop.rotate.enabled) { if(!forced) { return; } }

            // rotationA <---> startRotation <---> rotationB
            Vector3 rotationA = startRotation - loop.rotate.rotation;
            Vector3 rotationB = startRotation + loop.rotate.rotation;
            //RotateMode rotateMode = loop.rotate.loopType == Loop.LoopType.Restart ? RotateMode.FastBeyond360 : RotateMode.Fast;
            RotateMode rotateMode = Loop.GetRotateMode(loop.rotate.rotateMode);

            target.DOLocalRotate(rotationA, DEFAULT_DURATION_INIT_LOOP)
                  .SetDelay(loop.rotate.startDelay)
                  .SetUpdate(isTimeScaleIndependent)
                  .SetId(GetTweenId(target, TweenIdType.Rotate, TweenIdAnimation.Loop) + id)
                  .OnStart(() => { if(OnStart != null) { OnStart.Invoke(); } })
                  .OnComplete(() =>
                  {
                      Tweener tween =
                      target.DOLocalRotate(rotationB, loop.rotate.duration, rotateMode);
                      tween.SetUpdate(true);
                      tween.SetId(GetTweenId(target, TweenIdType.Rotate, TweenIdAnimation.Loop) + id);
                      tween.SetLoops(loop.rotate.loops, Loop.GetLoopType(loop.rotate.loopType));
                      switch(loop.rotate.easeType)
                      {
                          case EaseType.Ease: tween.SetEase(loop.rotate.ease); break;
                          case EaseType.AnimationCurve: tween.SetEase(loop.rotate.animationCurve); break;
                      }
                      tween.OnComplete(() => { if(OnComplete != null) { OnComplete.Invoke(); } });
                      tween.Play();
                  })
                  .Pause();
            if(loop.autoStart) { DOTween.Play(GetTweenId(target, TweenIdType.Rotate, TweenIdAnimation.Loop) + id); }
        }
        /// <summary>
        /// Creates a scale Loop animation, but it does not start automatically unless the loop's autoStart variable is set to true.
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="startScale">The initial rotation of the target. This is the animation's center.</param>
        /// <param name="loop">The loop animation settings.</param>
        /// <param name="OnStart">Callback listener.</param>
        /// <param name="OnComplete">Callback listener.</param>
        /// <param name="id">Adds an extra string to the loop's tween id. Used to differentiate several loops animations created for the same target.</param>
        /// <param name="forced">If true, it will initiate this animation, regardless if it's enabled or not.</param>
        public static void LoopScale(RectTransform target, Vector3 startScale, Loop loop, UnityAction OnStart, UnityAction OnComplete, string id = "", bool forced = false)
        {
            if(!loop.scale.enabled) { if(!forced) { return; } }
            // loop.scale.min <---> startScale <---> loop.scale.max
            target.DOScale(loop.scale.min, DEFAULT_DURATION_INIT_LOOP)
                  .SetDelay(loop.scale.startDelay)
                  .SetUpdate(true)
                  .SetId(GetTweenId(target, TweenIdType.Scale, TweenIdAnimation.Loop) + id)
                  .OnStart(() => { if(OnStart != null) { OnStart.Invoke(); } })
                  .OnComplete(() =>
                  {
                      Tweener tween =
                      target.DOScale(loop.scale.max, loop.scale.duration);
                      tween.SetUpdate(isTimeScaleIndependent);
                      tween.SetId(GetTweenId(target, TweenIdType.Scale, TweenIdAnimation.Loop) + id);
                      tween.SetLoops(loop.scale.loops, Loop.GetLoopType(loop.scale.loopType));
                      switch(loop.scale.easeType)
                      {
                          case EaseType.Ease: tween.SetEase(loop.scale.ease); break;
                          case EaseType.AnimationCurve: tween.SetEase(loop.scale.animationCurve); break;
                      }
                      tween.OnComplete(() => { if(OnComplete != null) { OnComplete.Invoke(); } });
                      tween.Play();
                  })
                  .Pause();
            if(loop.autoStart) { DOTween.Play(GetTweenId(target, TweenIdType.Scale, TweenIdAnimation.Loop) + id); }
        }
        /// <summary>
        /// Creates a fade (alpha) Loop animation, but it does not start automatically unless the loop's autoStart variable is set to true.
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="startAlpha">The initial rotation of the target. This is the animation's center.</param>
        /// <param name="loop">The loop animation settings.</param>
        /// <param name="OnStart">Callback listener.</param>
        /// <param name="OnComplete">Callback listener.</param>
        /// <param name="id">Adds an extra string to the loop's tween id. Used to differentiate several loops animations created for the same target.</param>
        /// <param name="forced">If true, it will initiate this animation, regardless if it's enabled or not.</param>
        public static void LoopFade(RectTransform target, float startAlpha, Loop loop, UnityAction OnStart, UnityAction OnComplete, string id = "", bool blocksRaycasts = false, bool forced = false)
        {
            if(!loop.fade.enabled) { if(!forced) { return; } }
            CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>() != null ? target.GetComponent<CanvasGroup>() : target.gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = startAlpha;
            canvasGroup.blocksRaycasts = blocksRaycasts;
            // loop.fade.min <---> startAlpha <---> loop.fade.max
            canvasGroup.DOFade(loop.fade.min, DEFAULT_DURATION_INIT_LOOP)
                       .SetDelay(loop.fade.startDelay)
                       .SetUpdate(isTimeScaleIndependent)
                       .SetId(GetTweenId(target, TweenIdType.Fade, TweenIdAnimation.Loop) + id)
                       .OnStart(() => { if(OnStart != null) { OnStart.Invoke(); } })
                       .OnComplete(() =>
                       {
                           canvasGroup.alpha = loop.fade.min;
                           Tweener tween = canvasGroup.DOFade(loop.fade.max, loop.fade.duration);
                           tween.SetUpdate(true);
                           tween.SetId(GetTweenId(target, TweenIdType.Fade, TweenIdAnimation.Loop) + id);
                           tween.SetLoops(loop.fade.loops, Loop.GetLoopType(loop.fade.loopType));
                           switch(loop.scale.easeType)
                           {
                               case EaseType.Ease: tween.SetEase(loop.fade.ease); break;
                               case EaseType.AnimationCurve: tween.SetEase(loop.fade.animationCurve); break;
                           }
                           tween.OnComplete(() => { if(OnComplete != null) { OnComplete.Invoke(); } });
                           tween.Play();
                       })
                       .Pause();
            if(loop.autoStart) { DOTween.Play(GetTweenId(target, TweenIdType.Fade, TweenIdAnimation.Loop) + id); }
        }
        /// <summary>
        /// Creates all the loops and pauses them. It plays only the ones that are set to autoStart.
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="startPosition">RectTranform's start position. This is the animation's center.</param>
        /// <param name="startRotation">RectTranform's start rotation. This is the animation's center.</param>
        /// <param name="startScale">RectTranform's start scale. This is the animation's center.</param>
        /// <param name="startAlpha">CanvasGroup's start alpha. This is the animation's center.</param>
        /// <param name="loop">The loop animation settings.</param>
        /// <param name="OnStartMoveLoop">Callback listener.</param>
        /// <param name="OnCompleteMoveLoop">Callback listener.</param>
        /// <param name="OnStartRotateLoop">Callback listener.</param>
        /// <param name="OnCompleteRotateLoop">Callback listener.</param>
        /// <param name="OnStartScaleLoop">Callback listener.</param>
        /// <param name="OnCompleteScaleLoop">Callback listener.</param>
        /// <param name="OnStartFadeLoop">Callback listener.</param>
        /// <param name="OnCompleteFadeLoop">Callback listener.</param>
        /// <param name="id">Adds an extra string to the loop's tween id. Used to differentiate several loops animations created for the same target.</param>
        /// <param name="blocksRaycasts">Does the CanvasGroup (that is attached automatically to this target) block raycasting (allow collision). Or, in other words, false means that it ignores clicks (for UIElement) and true means that it registeres clicks (for UIButtons).</param>
        /// <param name="forced">If true, it will initiate this animation, regardless if it's enabled or not.</param>
        public static void SetupLoops(
            RectTransform target, Vector3 startPosition, Vector3 startRotation, Vector3 startScale, float startAlpha, Loop loop,
            UnityAction OnStartMoveLoop, UnityAction OnCompleteMoveLoop,
            UnityAction OnStartRotateLoop, UnityAction OnCompleteRotateLoop,
            UnityAction OnStartScaleLoop, UnityAction OnCompleteScaleLoop,
            UnityAction OnStartFadeLoop, UnityAction OnCompleteFadeLoop,
            string id = "", bool blocksRaycasts = false, bool forced = false)
        {
            LoopMove(target, startPosition, loop, OnStartMoveLoop, OnCompleteMoveLoop, id, forced);
            LoopRotate(target, startRotation, loop, OnStartRotateLoop, OnCompleteRotateLoop, id, forced);
            LoopScale(target, startScale, loop, OnStartScaleLoop, OnCompleteScaleLoop, id, forced);
            LoopFade(target, startAlpha, loop, OnStartFadeLoop, OnCompleteFadeLoop, id, blocksRaycasts, forced);
        }
        /// <summary>
        /// Plays all the loops that have been previously set up for the target RectTransform.
        /// This means that you should have called the SetupLoops method, for the target RectTransform, before you called this method.
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="id">Adds an extra string to the loop's tween id. Used to differentiate several loops animations created for the same target.</param>
        public static void PlayLoops(RectTransform target, string id = "")
        {
            if(target == null) { return; }
            DOTween.Play(GetTweenId(target, TweenIdType.Move, TweenIdAnimation.Loop) + id);
            DOTween.Play(GetTweenId(target, TweenIdType.Rotate, TweenIdAnimation.Loop) + id);
            DOTween.Play(GetTweenId(target, TweenIdType.Scale, TweenIdAnimation.Loop) + id);
            DOTween.Play(GetTweenId(target, TweenIdType.Fade, TweenIdAnimation.Loop) + id);
        }
        /// <summary>
        /// Stops (kills) all the loops that are playing on the target RectTransform.
        /// This means that you called the PlayLoops method, for the target RectTransform, before you called this method.
        /// <para>Note: Some loops might play even if PlayLoops was not called. This can happen if autoStart is true for those certain loops and the SetupLoops method was called.</para>
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="id">Adds an extra string to the loop's tween id. Used to differentiate several loops animations created for the same target.</param>
        public static void StopLoops(RectTransform target, string id = "")
        {
            if(target == null) { return; }
            DOTween.Kill(GetTweenId(target, TweenIdType.Move, TweenIdAnimation.Loop) + id);
            DOTween.Kill(GetTweenId(target, TweenIdType.Rotate, TweenIdAnimation.Loop) + id);
            DOTween.Kill(GetTweenId(target, TweenIdType.Scale, TweenIdAnimation.Loop) + id);
            DOTween.Kill(GetTweenId(target, TweenIdType.Fade, TweenIdAnimation.Loop) + id);

            //// Unity has a bug where the layouts don't update correctly (2017-04-07 / Unity 5.5.0p4)
            //// This bug has been around for awhile and may be around for the future
            //// This forces an update of at least the parent, this should be enough since we are only animating our local positions
            //if (target.parent)
            //{
            //    var layout = target.parent.GetComponent<UnityEngine.UI.LayoutGroup>();
            //    if (layout == null) { return; }
            //    if (layout && layout.enabled) { layout.enabled = !layout.enabled; layout.enabled = !layout.enabled; }
            //}
            ////---end of fix
        }
        #endregion

        #region Punches
        /// <summary>
        /// Punches a RectTransform's anchoredPosition towards the given direction and then
        /// back to the starting one as if it was connected to the starting position via
        /// an elastic. 
        /// <para>You can force an execution of this animation (regardless if it's enabled or not) by setting forced as true.</para>
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="startPosition">RectTranform's start position (target.anchoredPosition). This will also be its end position.</param>
        /// <param name="punch">The punch animation settings.</param>
        /// <param name="forced">If true, it will fire this animation, regardless if it's enabled or not.</param>
        public static void PunchMove(RectTransform target, Vector3 startPosition, Punch punch, UnityAction OnStart, UnityAction OnComplete, bool forced = false)
        {
            if(!punch.move.enabled) { if(!forced) { return; } }
            target.DOPunchAnchorPos(punch.move.punch, punch.move.duration, punch.move.vibrato, punch.move.elasticity)
                  .SetDelay(punch.move.startDelay)
                  .SetUpdate(isTimeScaleIndependent)
                  .OnStart(() => { if(OnStart != null) { OnStart.Invoke(); } })
                  .OnComplete(() =>
                  {
                      target.DOAnchorPos(startPosition, DEFAULT_DURATION_ONCOMPLETE).OnComplete(() =>
                                                                                               {
                                                                                                   if(OnComplete != null)
                                                                                                   {
                                                                                                       OnComplete.Invoke();
                                                                                                   }
                                                                                               }).Play();
                  })
                  .Play();
        }
        /// <summary>
        /// Punches a Transform's localRotation towards the given size and then back to the
        /// starting one as if it was connected to the starting rotation via an elastic.
        /// <para>You can force an execution of this animation (regardless if it's enabled or not) by setting forced as true.</para>
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="startRotation">RectTranform's start localRotation. This will also be its end localRotation.</param>
        /// <param name="punch">The punch animation settings.</param>
        /// <param name="forced">If true, it will fire this animation, regardless if it's enabled or not.</param>
        public static void PunchRotate(RectTransform target, Vector3 startRotation, Punch punch, UnityAction OnStart, UnityAction OnComplete, bool forced = false)
        {
            if(!punch.rotate.enabled) { if(!forced) { return; } }
            target.DOPunchRotation(punch.rotate.punch, punch.rotate.duration, punch.rotate.vibrato, punch.rotate.elasticity)
                  .SetDelay(punch.rotate.startDelay)
                  .SetUpdate(isTimeScaleIndependent)
                  .OnStart(() => { if(OnStart != null) { OnStart.Invoke(); } })
                  .OnComplete(() => { target.DOLocalRotate(startRotation, DEFAULT_DURATION_ONCOMPLETE).OnComplete(() => { if(OnComplete != null) { OnComplete.Invoke(); } }).Play(); })
                  .Play();
        }
        /// <summary>
        /// Punches a Transform's localScale towards the given size and then back to the
        /// starting one as if it was connected to the starting scale via an elastic.
        /// </summary>
        /// <param name="target">Target RectTransform.</param>
        /// <param name="startScale">RectTranform's start localScale. This will also be its end localScale.</param>
        /// <param name="punch">The punch animation settings.</param>
        /// <param name="forced">If true, it will fire this animation, regardless if it's enabled or not.</param>
        public static void PunchScale(RectTransform target, Vector3 startScale, Punch punch, UnityAction OnStart, UnityAction OnComplete, bool forced = false)
        {
            if(!punch.scale.enabled) { if(!forced) { return; } }
            target.DOPunchScale(punch.scale.punch, punch.scale.duration, punch.scale.vibrato, punch.scale.elasticity)
                  .SetDelay(punch.scale.startDelay)
                  .SetUpdate(isTimeScaleIndependent)
                  .OnStart(() => { if(OnStart != null) { OnStart.Invoke(); } })
                  .OnComplete(() => { target.DOScale(startScale, DEFAULT_DURATION_ONCOMPLETE).OnComplete(() => { if(OnComplete != null) { OnComplete.Invoke(); } }).Play(); })
                  .Play();
        }
        #endregion
    }

    #region Animation / Move / Rotate / Scale / Fade

    /// <summary>
    /// Base class for all the In and Out animations.
    /// </summary>
    [Serializable]
    public class Anim
    {
        /// <summary>
        /// Determines what type of animation this is. This changes the way the Animator percieves the set values.
        /// </summary>
        public AnimationType animationType;
        /// <summary>
        /// Movement animation settings.
        /// </summary>
        public Move move;
        /// <summary>
        /// Rotation animation settings.
        /// </summary>
        public Rotate rotate;
        /// <summary>
        /// Scale animation settings.
        /// </summary>
        public Scale scale;
        /// <summary>
        /// Fade (alpha) animation settings.
        /// </summary>
        public Fade fade;

#if UNITY_EDITOR
        public AnimBool moveIsExpanded = new AnimBool(false);
        public AnimBool rotateIsExpanded = new AnimBool(false);
        public AnimBool scaleIsExpanded = new AnimBool(false);
        public AnimBool fadeIsExpanded = new AnimBool(false);

        public void UpdateAnimBools()
        {
            moveIsExpanded.target = move.enabled;
            rotateIsExpanded.target = rotate.enabled;
            scaleIsExpanded.target = scale.enabled;
            fadeIsExpanded.target = fade.enabled;
        }
#endif

        /// <summary>
        /// Type of Animation. This changes the way the Animator percieves the set values.
        /// </summary>
        public enum AnimationType { In, Out, State }
        public static AnimationType Reverse(AnimationType animationType)
        {
            switch(animationType)
            {
                case AnimationType.In: return AnimationType.Out;
                case AnimationType.Out: return AnimationType.In;
                case AnimationType.State: return AnimationType.State;
                default: return AnimationType.In;
            }
        }

        public Anim(AnimationType aType)
        {
            animationType = aType;
            move = new Move(aType);
            rotate = new Rotate(aType);
            scale = new Scale(aType);
            fade = new Fade(aType);
        }
        public void Reset(AnimationType aType)
        {
            animationType = aType;
            move.Reset(aType);
            rotate.Reset(aType);
            scale.Reset(aType);
            fade.Reset(aType);
        }
        public void UpdateValues(Anim a)
        {
            animationType = a.animationType;
            move = a.move.Copy();
            rotate = a.rotate.Copy();
            scale = a.scale.Copy();
            fade = a.fade.Copy();
        }
        public Anim Copy()
        {
            Anim copy = new Anim(animationType)
            {
                animationType = animationType,
                move = move.Copy(),
                rotate = rotate.Copy(),
                scale = scale.Copy(),
                fade = fade.Copy()
            };
            return copy;
        }
        public Anim Reverse()
        {
            Anim reverse = new Anim(Reverse(animationType));
            reverse.animationType = Reverse(animationType);
            reverse.move = move.Reverse();
            reverse.rotate = rotate.Reverse();
            reverse.scale = scale.Reverse();
            reverse.fade = fade.Reverse();
            return reverse;
        }
        public bool Enabled { get { return move.enabled || rotate.enabled || scale.enabled || fade.enabled; } }
        public float TotalDuration
        {
            get
            {
                return Mathf.Max(move.enabled ? move.TotalDuration : 0,
                                 rotate.enabled ? rotate.TotalDuration : 0,
                                 scale.enabled ? scale.TotalDuration : 0,
                                 fade.enabled ? fade.TotalDuration : 0);
            }
        }
        public float StartDelay
        {
            get
            {
                if(!Enabled) { return 0; }
                return Mathf.Min(move.enabled ? move.startDelay : 10000,
                                 rotate.enabled ? rotate.startDelay : 10000,
                                 scale.enabled ? scale.startDelay : 10000,
                                 fade.enabled ? fade.startDelay : 10000);
            }
        }
    }

    /// <summary>
    /// Animation settings for Movement
    /// </summary>
    [Serializable]
    public class Move
    {
        public enum MoveDirection
        {
            Left = 0,
            Right = 1,
            Top = 2,
            Bottom = 3,
            TopLeft = 4,
            TopCenter = 5,
            TopRight = 6,
            MiddleLeft = 7,
            MiddleCenter = 8,
            MiddleRight = 9,
            BottomLeft = 10,
            BottomCenter = 11,
            BottomRight = 12,
            CustomPosition = 13
        }
        public static MoveDirection Reverse(MoveDirection moveDirection)
        {
            switch(moveDirection)
            {
                case MoveDirection.Left: return MoveDirection.Right;
                case MoveDirection.Right: return MoveDirection.Left;
                case MoveDirection.Top: return MoveDirection.Bottom;
                case MoveDirection.Bottom: return MoveDirection.Top;
                case MoveDirection.TopLeft: return MoveDirection.BottomRight;
                case MoveDirection.TopCenter: return MoveDirection.BottomCenter;
                case MoveDirection.TopRight: return MoveDirection.BottomLeft;
                case MoveDirection.MiddleLeft: return MoveDirection.MiddleRight;
                case MoveDirection.MiddleCenter: return MoveDirection.MiddleCenter;
                case MoveDirection.MiddleRight: return MoveDirection.MiddleLeft;
                case MoveDirection.BottomLeft: return MoveDirection.TopRight;
                case MoveDirection.BottomCenter: return MoveDirection.TopCenter;
                case MoveDirection.BottomRight: return MoveDirection.TopLeft;
                case MoveDirection.CustomPosition: return MoveDirection.CustomPosition;
                default: return MoveDirection.Left;
            }
        }

        /// <summary>
        /// If TRUE, this animation will get executed by the Animator when triggered, FALSE otherwise (default: false).
        /// </summary>
        public bool enabled = false;
        /// <summary>
        /// Select if this data is for an IN or an OUT animation.
        /// </summary>
        public Anim.AnimationType animationType = Anim.AnimationType.In;
        /// <summary>
        /// Depending on the animation type, the direction is considered either IN (eg. Move IN from Left) or OUT (eg. Move OUT to Left)
        /// </summary>
        public MoveDirection moveDirection = MoveDirection.Left;
        /// <summary>
        /// Depending on the animation type, this is considered either the TO or the FROM position (when moveDirection is set to CustomPosition).
        /// If it's a STATE animation type this is considered the move by variable.
        /// </summary>
        public Vector3 customPosition = Vector3.zero;
        /// <summary>
        /// Use an Ease or an AnimationCurve in order to calculate the rate of change of the animation over time.
        /// </summary>
        public UIAnimator.EaseType easeType = UIAnimator.EaseType.Ease;
        /// <summary>
        /// Sets the ease of the tween. Easing functions specify the rate of change of a parameter over time.
        /// <para>To see how default ease curves look, check out easings.net</para>
        /// </summary>
        public Ease ease = UIAnimator.DEFAULT_EASE;
        /// <summary>
        /// If the easeType is set to AnimationCurve, this will be used in order to calculate the rate of change of the animation over time.
        /// </summary>
        public AnimationCurve animationCurve = new AnimationCurve();
        /// <summary>
        /// Start delay for the animation.
        /// </summary>
        public float startDelay = UIAnimator.DEFAULT_START_DELAY;
        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public float duration = UIAnimator.DEFAULT_DURATION;

        public Move(Anim.AnimationType aType)
        {
            Reset(aType);
        }
        public void Reset(Anim.AnimationType aType, MoveDirection mDirection = MoveDirection.Left)
        {
            enabled = false;
            moveDirection = mDirection;
            animationType = aType;
            customPosition = Vector3.zero;
            easeType = UIAnimator.EaseType.Ease;
            ease = UIAnimator.DEFAULT_EASE;
            animationCurve = new AnimationCurve();
            startDelay = UIAnimator.DEFAULT_START_DELAY;
            duration = UIAnimator.DEFAULT_DURATION;
        }
        public void UpdateValues(Move m)
        {
            enabled = m.enabled;
            moveDirection = m.moveDirection;
            animationType = m.animationType;
            customPosition = m.customPosition;
            easeType = m.easeType;
            ease = m.ease;
            animationCurve = m.animationCurve;
            startDelay = m.startDelay;
            duration = m.duration;
        }
        public Move Copy()
        {
            Move copy = new Move(animationType)
            {
                enabled = enabled,
                moveDirection = moveDirection,
                animationType = animationType,
                customPosition = customPosition,
                easeType = easeType,
                ease = ease,
                animationCurve = animationCurve,
                startDelay = startDelay,
                duration = duration
            };
            return copy;
        }
        public Move Reverse()
        {
            Move reverse = new Move(animationType)
            {
                enabled = enabled,
                moveDirection = Reverse(moveDirection),
                animationType = Anim.Reverse(animationType),
                customPosition = customPosition * (-1),
                easeType = easeType,
                ease = UIAnimator.Reverse(ease),
                animationCurve = animationCurve,
                startDelay = startDelay,
                duration = duration
            };
            return reverse;
        }
        public float TotalDuration { get { return startDelay + duration; } }
    }

    /// <summary>
    /// Animation settings for Rotation
    /// </summary>
    [Serializable]
    public class Rotate
    {
        /// <summary>
        /// If TRUE, this animation will get executed by the Animator when triggered, FALSE otherwise (default: false).
        /// </summary>
        public bool enabled = false;
        /// <summary>
        /// Select if this data is for an IN or an OUT animation.
        /// </summary>
        public Anim.AnimationType animationType = Anim.AnimationType.In;
        /// <summary>
        /// Depending on the animation type, this is considered either the TO or the FROM rotation.
        /// </summary>
        public Vector3 rotation = Vector3.zero;
        /// <summary>
        /// What type of rotation should this animation have: Fast, FastBeyond360, LocalAxisAdd or WorldAxisAdd. Default is RotateMode.FastBeyond360.
        /// </summary>
        public RotateMode rotateMode = RotateMode.FastBeyond360;
        /// <summary>
        /// Use an Ease or an AnimationCurve in order to calculate the rate of change of the animation over time.
        /// </summary>
        public UIAnimator.EaseType easeType = UIAnimator.EaseType.Ease;
        /// <summary>
        /// Sets the ease of the tween. Easing functions specify the rate of change of a parameter over time.
        /// <para>To see how default ease curves look, check out easings.net</para>
        /// </summary>
        public Ease ease = UIAnimator.DEFAULT_EASE;
        /// <summary>
        /// If the easeType is set to AnimationCurve, this will be used in order to calculate the rate of change of the animation over time.
        /// </summary>
        public AnimationCurve animationCurve = new AnimationCurve();
        /// <summary>
        /// Start delay for the animation.
        /// </summary>
        public float startDelay = UIAnimator.DEFAULT_START_DELAY;
        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public float duration = UIAnimator.DEFAULT_DURATION;

        public Rotate(Anim.AnimationType aType)
        {
            Reset(aType);
        }
        public void Reset(Anim.AnimationType aType)
        {
            enabled = false;
            animationType = aType;
            rotation = Vector3.zero;
            rotateMode = RotateMode.FastBeyond360;
            easeType = UIAnimator.EaseType.Ease;
            ease = UIAnimator.DEFAULT_EASE;
            animationCurve = new AnimationCurve();
            startDelay = UIAnimator.DEFAULT_START_DELAY;
            duration = UIAnimator.DEFAULT_DURATION;
        }
        public void UpdateValues(Rotate r)
        {
            enabled = r.enabled;
            animationType = r.animationType;
            rotation = r.rotation;
            rotateMode = r.rotateMode;
            easeType = r.easeType;
            ease = r.ease;
            animationCurve = r.animationCurve;
            startDelay = r.startDelay;
            duration = r.duration;
        }
        public Rotate Copy()
        {
            Rotate copy = new Rotate(animationType)
            {
                enabled = enabled,
                animationType = animationType,
                rotation = rotation,
                rotateMode = rotateMode,
                easeType = easeType,
                ease = ease,
                animationCurve = animationCurve,
                startDelay = startDelay,
                duration = duration
            };
            return copy;
        }
        public Rotate Reverse()
        {
            Rotate reverse = new Rotate(animationType)
            {
                enabled = enabled,
                animationType = Anim.Reverse(animationType),
                rotation = Quaternion.Inverse(Quaternion.Euler(rotation)).eulerAngles,
                rotateMode = rotateMode,
                easeType = easeType,
                ease = UIAnimator.Reverse(ease),
                animationCurve = animationCurve,
                startDelay = startDelay,
                duration = duration
            };
            return reverse;
        }
        public float TotalDuration { get { return startDelay + duration; } }
    }

    /// <summary>
    /// Animation settings for Scale
    /// </summary>
    [Serializable]
    public class Scale
    {
        /// <summary>
        /// If TRUE, this animation will get executed by the Animator when triggered, FALSE otherwise (default: false).
        /// </summary>
        public bool enabled = false;
        /// <summary>
        /// Select if this data is for an IN or an OUT animation.
        /// </summary>
        public Anim.AnimationType animationType = Anim.AnimationType.In;
        /// <summary>
        /// Depending on the animation type, this is considered either the TO or the FROM scale.
        /// </summary>
        public Vector3 scale = Vector3.zero;
        /// <summary>
        /// Use an Ease or an AnimationCurve in order to calculate the rate of change of the animation over time.
        /// </summary>
        public UIAnimator.EaseType easeType = UIAnimator.EaseType.Ease;
        /// <summary>
        /// Sets the ease of the tween. Easing functions specify the rate of change of a parameter over time.
        /// <para>To see how default ease curves look, check out easings.net</para>
        /// </summary>
        public Ease ease = UIAnimator.DEFAULT_EASE;
        /// <summary>
        /// If the easeType is set to AnimationCurve, this will be used in order to calculate the rate of change of the animation over time.
        /// </summary>
        public AnimationCurve animationCurve = new AnimationCurve();
        /// <summary>
        /// Start delay for the animation.
        /// </summary>
        public float startDelay = UIAnimator.DEFAULT_START_DELAY;
        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public float duration = UIAnimator.DEFAULT_DURATION;

        public Scale(Anim.AnimationType aType)
        {
            Reset(aType);
        }
        public void Reset(Anim.AnimationType aType)
        {
            enabled = false;
            animationType = aType;
            scale = Vector3.zero;
            easeType = UIAnimator.EaseType.Ease;
            ease = UIAnimator.DEFAULT_EASE;
            animationCurve = new AnimationCurve();
            startDelay = UIAnimator.DEFAULT_START_DELAY;
            duration = UIAnimator.DEFAULT_DURATION;
        }
        public void UpdateValues(Scale s)
        {
            enabled = s.enabled;
            animationType = s.animationType;
            scale = s.scale;
            easeType = s.easeType;
            ease = s.ease;
            animationCurve = s.animationCurve;
            startDelay = s.startDelay;
            duration = s.duration;
        }
        public Scale Copy()
        {
            Scale copy = new Scale(animationType)
            {
                enabled = enabled,
                animationType = animationType,
                scale = scale,
                easeType = easeType,
                ease = ease,
                animationCurve = animationCurve,
                startDelay = startDelay,
                duration = duration
            };
            return copy;
        }
        public Scale Reverse()
        {
            Scale reverse = new Scale(animationType)
            {
                enabled = enabled,
                animationType = Anim.Reverse(animationType),
                scale = scale * (-1),
                easeType = easeType,
                ease = UIAnimator.Reverse(ease),
                animationCurve = animationCurve,
                startDelay = startDelay,
                duration = duration
            };
            return reverse;
        }
        public float TotalDuration { get { return startDelay + duration; } }
    }

    /// <summary>
    /// Animation settings for Fade (alpha value)
    /// </summary>
    [Serializable]
    public class Fade
    {
        /// <summary>
        /// If TRUE, this animation will get executed by the Animator when triggered, FALSE otherwise (default: false).
        /// </summary>
        public bool enabled = false;
        /// <summary>
        /// Select if this data is for an IN or an OUT animation.
        /// </summary>
        public Anim.AnimationType animationType = Anim.AnimationType.In;
        /// <summary>
        /// Depending on the animation type, this is considered either the TO or the FROM alpha.
        /// </summary>
        public float alpha = 1;
        /// <summary>
        /// Use an Ease or an AnimationCurve in order to calculate the rate of change of the animation over time.
        /// </summary>
        public UIAnimator.EaseType easeType = UIAnimator.EaseType.Ease;
        /// <summary>
        /// Sets the ease of the tween. Easing functions specify the rate of change of a parameter over time.
        /// <para>To see how default ease curves look, check out easings.net</para>
        /// </summary>
        public Ease ease = UIAnimator.DEFAULT_EASE;
        /// <summary>
        /// If the easeType is set to AnimationCurve, this will be used in order to calculate the rate of change of the animation over time.
        /// </summary>
        public AnimationCurve animationCurve = new AnimationCurve();
        /// <summary>
        /// Start delay for the animation.
        /// </summary>
        public float startDelay = UIAnimator.DEFAULT_START_DELAY;
        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public float duration = UIAnimator.DEFAULT_DURATION;

        public Fade(Anim.AnimationType aType)
        {
            Reset(aType);
        }
        public void Reset(Anim.AnimationType aType)
        {
            enabled = false;
            animationType = aType;
            alpha = 1;
            easeType = UIAnimator.EaseType.Ease;
            ease = UIAnimator.DEFAULT_EASE;
            animationCurve = new AnimationCurve();
            startDelay = UIAnimator.DEFAULT_START_DELAY;
            duration = UIAnimator.DEFAULT_DURATION;
        }
        public void UpdateValues(Fade f)
        {
            enabled = f.enabled;
            animationType = f.animationType;
            alpha = f.alpha;
            easeType = f.easeType;
            ease = f.ease;
            animationCurve = f.animationCurve;
            startDelay = f.startDelay;
            duration = f.duration;
        }
        public Fade Copy()
        {
            Fade copy = new Fade(animationType)
            {
                enabled = enabled,
                animationType = animationType,
                alpha = alpha,
                easeType = easeType,
                ease = ease,
                animationCurve = animationCurve,
                startDelay = startDelay,
                duration = duration
            };
            return copy;
        }
        public Fade Reverse()
        {
            Fade reverse = new Fade(animationType)
            {
                enabled = enabled,
                animationType = Anim.Reverse(animationType),
                alpha = alpha == 0 ? 1 : (alpha == 1 ? 0 : (1 - alpha)),
                easeType = easeType,
                ease = UIAnimator.Reverse(ease),
                animationCurve = animationCurve,
                startDelay = startDelay,
                duration = duration
            };
            return reverse;
        }
        public float TotalDuration { get { return startDelay + duration; } }
    }

    #endregion

    #region Loop / MoveLoop/ RotateLoop / ScaleLoop / FadeLoop

    /// <summary>
    /// Base class for all the Loop animations.
    /// </summary>
    [Serializable]
    public class Loop
    {
        /// <summary>
        /// This deternimes if the loop should start from the get go (after being initialized) or on demand.
        /// </summary>
        public bool autoStart = false;
        public MoveLoop move;
        public RotateLoop rotate;
        public ScaleLoop scale;
        public FadeLoop fade;

#if UNITY_EDITOR
        public AnimBool moveIsExpanded = new AnimBool(false);
        public AnimBool rotateIsExpanded = new AnimBool(false);
        public AnimBool scaleIsExpanded = new AnimBool(false);
        public AnimBool fadeIsExpanded = new AnimBool(false);

        public void UpdateAnimBools()
        {
            moveIsExpanded.target = move.enabled;
            rotateIsExpanded.target = rotate.enabled;
            scaleIsExpanded.target = scale.enabled;
            fadeIsExpanded.target = fade.enabled;
        }
#endif

        /// <summary>
        /// Rotation mode used with DORotate methods
        /// </summary>
        public enum RotateMode
        {
            /// <summary>
            /// Fastest way that never rotates beyond 360?
            /// </summary>
            Fast = 0,
            /// <summary>
            /// Fastest way that rotates beyond 360?
            /// </summary>
            FastBeyond360 = 1,
            /// <summary>
            /// Adds the given rotation to the transform using world axis and an advanced precision
            /// mode (like when using transform.Rotate(Space.World)).
            /// In this mode the end value is is always considered relative
            /// </summary>
            WorldAxisAdd = 2,
            /// <summary>
            /// Adds the given rotation to the transform's local axis (like when rotating an
            /// object with the "local" switch enabled in Unity's editor or using transform.Rotate(Space.Self)).
            /// In this mode the end value is is always considered relative
            /// </summary>
            LocalAxisAdd = 3
        }

        public static DG.Tweening.RotateMode GetRotateMode(RotateMode rotateMode)
        {
            switch(rotateMode)
            {
                case RotateMode.Fast: return DG.Tweening.RotateMode.Fast;
                case RotateMode.FastBeyond360: return DG.Tweening.RotateMode.FastBeyond360;
                case RotateMode.WorldAxisAdd: return DG.Tweening.RotateMode.WorldAxisAdd;
                case RotateMode.LocalAxisAdd: return DG.Tweening.RotateMode.WorldAxisAdd;
                default: return DG.Tweening.RotateMode.Fast;
            }
        }
        public static RotateMode GetRotateMode(DG.Tweening.RotateMode rotateMode)
        {
            switch(rotateMode)
            {
                case DG.Tweening.RotateMode.Fast: return RotateMode.Fast;
                case DG.Tweening.RotateMode.FastBeyond360: return RotateMode.FastBeyond360;
                case DG.Tweening.RotateMode.WorldAxisAdd: return RotateMode.WorldAxisAdd;
                case DG.Tweening.RotateMode.LocalAxisAdd: return RotateMode.LocalAxisAdd;
                default: return RotateMode.Fast;
            }
        }

        /// <summary>
        /// Types of loop
        /// </summary>
        public enum LoopType
        {
            /// <summary>
            /// Each loop cycle restarts from the beginning
            /// </summary>
            Restart = 0,
            /// <summary>
            /// The tween moves forward and backwards at alternate cycles
            /// </summary>
            Yoyo = 1,
            /// <summary>
            /// Continuously increments the tween at the end of each loop cycle (A to B, B to
            /// B+(A-B), and so on), thus always moving "onward".
            /// In case of String tweens works only if the tween is set as relative
            /// </summary>
            Incremental = 2
        }
        public static DG.Tweening.LoopType GetLoopType(LoopType loopType)
        {
            switch(loopType)
            {
                case LoopType.Restart: return DG.Tweening.LoopType.Restart;
                case LoopType.Yoyo: return DG.Tweening.LoopType.Yoyo;
                case LoopType.Incremental: return DG.Tweening.LoopType.Incremental;
                default: return DG.Tweening.LoopType.Yoyo;
            }
        }
        public static LoopType GetLoopType(DG.Tweening.LoopType loopType)
        {
            switch(loopType)
            {
                case DG.Tweening.LoopType.Restart: return LoopType.Restart;
                case DG.Tweening.LoopType.Yoyo: return LoopType.Yoyo;
                case DG.Tweening.LoopType.Incremental: return LoopType.Incremental;
                default: return LoopType.Yoyo;
            }
        }

        public Loop()
        {
            autoStart = false;
            move = new MoveLoop();
            rotate = new RotateLoop();
            scale = new ScaleLoop();
            fade = new FadeLoop();
        }
        public void Reset()
        {
            autoStart = false;
            move.Reset();
            rotate.Reset();
            scale.Reset();
            fade.Reset();
        }
        public Loop Copy()
        {
            Loop copy = new Loop()
            {
                move = move.Copy(),
                rotate = rotate.Copy(),
                scale = scale.Copy(),
                fade = fade.Copy()
            };
            return copy;
        }
        public bool Enabled { get { return move.enabled || rotate.enabled || scale.enabled || fade.enabled; } }
        public float TotalDuration
        {
            get
            {
                return Mathf.Max(move.enabled ? move.TotalDuration : 0,
                                 rotate.enabled ? rotate.TotalDuration : 0,
                                 scale.enabled ? scale.TotalDuration : 0,
                                 fade.enabled ? fade.TotalDuration : 0);
            }
        }
    }

    [Serializable]
    public class MoveLoop
    {
        /// <summary>
        /// If TRUE, this animation will get executed by the Animator when triggered, FALSE otherwise (default: false).
        /// </summary>
        public bool enabled = false;
        /// <summary>
        /// This movement is calculated startAnchoredPosition-movement for min and startAnchoredPosition+movment for max
        /// </summary>
        public Vector3 movement = Vector3.zero;
        /// <summary>
        /// Use an Ease or an AnimationCurve in order to calculate the rate of change of the animation over time.
        /// </summary>
        public UIAnimator.EaseType easeType = UIAnimator.EaseType.Ease;
        /// <summary>
        /// Sets the ease of the tween. Easing functions specify the rate of change of a parameter over time.
        /// <para>To see how default ease curves look, check out easings.net</para>
        /// </summary>
        public Ease ease = UIAnimator.DEFAULT_EASE;
        /// <summary>
        /// If the easeType is set to AnimationCurve, this will be used in order to calculate the rate of change of the animation over time.
        /// </summary>
        public AnimationCurve animationCurve = new AnimationCurve();
        /// <summary>
        /// Number of loops (-1 = infinite loops).
        /// </summary>
        public int loops = UIAnimator.DEFAULT_LOOPS;
        /// <summary>
        /// Types of loop.
        /// </summary>
        public Loop.LoopType loopType = Loop.LoopType.Yoyo;
        /// <summary>
        /// Delay is amount (seconds) that the animation will wait before beginning
        /// </summary>
        public float startDelay = UIAnimator.DEFAULT_START_DELAY;
        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public float duration = UIAnimator.DEFAULT_DURATION;

        public void Reset()
        {
            enabled = false;
            movement = Vector3.zero;
            easeType = UIAnimator.EaseType.Ease;
            ease = UIAnimator.DEFAULT_EASE;
            animationCurve = new AnimationCurve();
            loops = UIAnimator.DEFAULT_LOOPS;
            loopType = Loop.LoopType.Yoyo;
            startDelay = UIAnimator.DEFAULT_START_DELAY;
            duration = UIAnimator.DEFAULT_DURATION;
        }
        public MoveLoop Copy()
        {
            MoveLoop copy = new MoveLoop()
            {
                enabled = enabled,
                movement = movement,
                easeType = easeType,
                ease = ease,
                animationCurve = animationCurve,
                loops = loops,
                loopType = loopType,
                startDelay = startDelay,
                duration = duration
            };
            return copy;
        }
        public float TotalDuration { get { return startDelay + duration; } }
    }


    [Serializable]
    public class RotateLoop
    {
        /// <summary>
        /// If TRUE, this animation will get executed by the Animator when triggered, FALSE otherwise (default: false).
        /// </summary>
        public bool enabled = false;
        /// <summary>
        /// This rotation is calculated startRotation-rotation for min and startRotation+rotation for max
        /// </summary>
        public Vector3 rotation = Vector3.zero;
        /// <summary>
        /// Use an Ease or an AnimationCurve in order to calculate the rate of change of the animation over time.
        /// </summary>
        public UIAnimator.EaseType easeType = UIAnimator.EaseType.Ease;
        /// <summary>
        /// Sets the ease of the tween. Easing functions specify the rate of change of a parameter over time.
        /// <para>To see how default ease curves look, check out easings.net</para>
        /// </summary>
        public Ease ease = UIAnimator.DEFAULT_EASE;
        /// <summary>
        /// If the easeType is set to AnimationCurve, this will be used in order to calculate the rate of change of the animation over time.
        /// </summary>
        public AnimationCurve animationCurve = new AnimationCurve();
        /// <summary>
        /// Number of loops (-1 = infinite loops).
        /// </summary>
        public int loops = UIAnimator.DEFAULT_LOOPS;
        /// <summary>
        /// Types of loop.
        /// </summary>
        public Loop.LoopType loopType = Loop.LoopType.Yoyo;
        /// <summary>
        /// Rotation mode used for this loop.
        /// <para>Fast: Fastest way that never rotates beyond 360?/para>
        /// <para>FastBeyond360: Fastest way that rotates beyond 360?/para>
        /// <para>WorldAxisAdd: Adds the given rotation to the transform using world axis and an advanced precision mode (like when using transform.Rotate(Space.World)). In this mode the end value is is always considered relative </para>
        /// <para>LocalAxisAdd: Adds the given rotation to the transform's local axis (like when rotating an object with the "local" switch enabled in Unity's editor or using transform.Rotate(Space.Self)). In this mode the end value is is always considered relative</para>
        /// </summary>
        public Loop.RotateMode rotateMode = Loop.RotateMode.Fast;
        /// <summary>
        /// Delay is amount (seconds) that the animation will wait before beginning
        /// </summary>
        public float startDelay = UIAnimator.DEFAULT_START_DELAY;
        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public float duration = UIAnimator.DEFAULT_DURATION;

        public void Reset()
        {
            enabled = false;
            rotation = Vector3.zero;
            easeType = UIAnimator.EaseType.Ease;
            ease = UIAnimator.DEFAULT_EASE;
            animationCurve = new AnimationCurve();
            loops = UIAnimator.DEFAULT_LOOPS;
            loopType = Loop.LoopType.Yoyo;
            startDelay = UIAnimator.DEFAULT_START_DELAY;
            duration = UIAnimator.DEFAULT_DURATION;
        }
        public RotateLoop Copy()
        {
            RotateLoop copy = new RotateLoop()
            {
                enabled = enabled,
                rotation = rotation,
                easeType = easeType,
                ease = ease,
                animationCurve = animationCurve,
                loops = loops,
                loopType = loopType,
                startDelay = startDelay,
                duration = duration
            };
            return copy;
        }
        public float TotalDuration { get { return startDelay + duration; } }
    }

    [Serializable]
    public class ScaleLoop
    {
        public static Vector3 DEFAULT_MIN = new Vector3(1, 1, 1);
        public static Vector3 DEFAULT_MAX = new Vector3(1.05f, 1.05f, 1.05f);

        /// <summary>
        /// If TRUE, this animation will get executed by the Animator when triggered, FALSE otherwise (default: false).
        /// </summary>
        public bool enabled = false;
        /// <summary>
        /// The minimum values for the scale factor of the scale loop animation (default: 1).
        /// </summary>
        public Vector3 min = DEFAULT_MIN;
        /// <summary>
        /// The maximum values for the scale factor of the scale loop animation (default: 1.05).
        /// </summary>
        public Vector3 max = DEFAULT_MAX;
        /// <summary>
        /// Use an Ease or an AnimationCurve in order to calculate the rate of change of the animation over time.
        /// </summary>
        public UIAnimator.EaseType easeType = UIAnimator.EaseType.Ease;
        /// <summary>
        /// Sets the ease of the tween. Easing functions specify the rate of change of a parameter over time.
        /// <para>To see how default ease curves look, check out easings.net</para>
        /// </summary>
        public Ease ease = UIAnimator.DEFAULT_EASE;
        /// <summary>
        /// If the easeType is set to AnimationCurve, this will be used in order to calculate the rate of change of the animation over time.
        /// </summary>
        public AnimationCurve animationCurve = new AnimationCurve();
        /// <summary>
        /// Number of loops (-1 = infinite loops).
        /// </summary>
        public int loops = UIAnimator.DEFAULT_LOOPS;
        /// <summary>
        /// Types of loop.
        /// </summary>
        public Loop.LoopType loopType = Loop.LoopType.Yoyo;
        /// <summary>
        /// Delay is amount (seconds) that the animation will wait before beginning
        /// </summary>
        public float startDelay = UIAnimator.DEFAULT_START_DELAY;
        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public float duration = UIAnimator.DEFAULT_DURATION;

        public void Reset()
        {
            enabled = false;
            min = DEFAULT_MIN;
            max = DEFAULT_MAX;
            easeType = UIAnimator.EaseType.Ease;
            ease = UIAnimator.DEFAULT_EASE;
            animationCurve = new AnimationCurve();
            loops = UIAnimator.DEFAULT_LOOPS;
            loopType = Loop.LoopType.Yoyo;
            startDelay = UIAnimator.DEFAULT_START_DELAY;
            duration = UIAnimator.DEFAULT_DURATION;
        }
        public ScaleLoop Copy()
        {
            ScaleLoop copy = new ScaleLoop()
            {
                enabled = enabled,
                min = min,
                max = max,
                easeType = easeType,
                ease = ease,
                animationCurve = animationCurve,
                loops = loops,
                loopType = loopType,
                startDelay = startDelay,
                duration = duration
            };
            return copy;
        }
        public float TotalDuration { get { return startDelay + duration; } }
    }

    [Serializable]
    public class FadeLoop
    {
        public static float DEFAULT_MIN = 0;
        public static float DEFAULT_MAX = 1;

        /// <summary>
        /// If TRUE, this animation will get executed by the Animator when triggered, FALSE otherwise (default: false).
        /// </summary>
        public bool enabled = false;
        /// <summary>
        /// The minimum alpha value for the fade animation loop (default: 0).
        /// </summary>
        public float min = DEFAULT_MIN;
        /// <summary>
        /// The maximum alpha value for the fade animation loop (default: 1).
        /// </summary>
        public float max = DEFAULT_MAX;
        /// <summary>
        /// Use an Ease or an AnimationCurve in order to calculate the rate of change of the animation over time.
        /// </summary>
        public UIAnimator.EaseType easeType = UIAnimator.EaseType.Ease;
        /// <summary>
        /// Sets the ease of the tween. Easing functions specify the rate of change of a parameter over time.
        /// <para>To see how default ease curves look, check out easings.net</para>
        /// </summary>
        public Ease ease = UIAnimator.DEFAULT_EASE;
        /// <summary>
        /// If the easeType is set to AnimationCurve, this will be used in order to calculate the rate of change of the animation over time.
        /// </summary>
        public AnimationCurve animationCurve = new AnimationCurve();
        /// <summary>
        /// Number of loops (-1 = infinite loops).
        /// </summary>
        public int loops = UIAnimator.DEFAULT_LOOPS;
        /// <summary>
        /// Types of loop.
        /// </summary>
        public Loop.LoopType loopType = Loop.LoopType.Yoyo;
        /// <summary>
        /// Delay is amount (seconds) that the animation will wait before beginning
        /// </summary>
        public float startDelay = UIAnimator.DEFAULT_START_DELAY;
        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public float duration = UIAnimator.DEFAULT_DURATION;

        public void Reset()
        {
            enabled = false;
            min = DEFAULT_MIN;
            max = DEFAULT_MAX;
            easeType = UIAnimator.EaseType.Ease;
            ease = UIAnimator.DEFAULT_EASE;
            animationCurve = new AnimationCurve();
            loops = UIAnimator.DEFAULT_LOOPS;
            loopType = Loop.LoopType.Yoyo;
            startDelay = UIAnimator.DEFAULT_START_DELAY;
            duration = UIAnimator.DEFAULT_DURATION;
        }
        public FadeLoop Copy()
        {
            FadeLoop copy = new FadeLoop()
            {
                enabled = enabled,
                min = min,
                max = max,
                easeType = easeType,
                ease = ease,
                animationCurve = animationCurve,
                loops = loops,
                loopType = loopType,
                startDelay = startDelay,
                duration = duration
            };
            return copy;
        }
        public float TotalDuration { get { return startDelay + duration; } }
    }

    #endregion

    #region Punch

    /// <summary>
    /// Base class for all the Punch animations.
    /// </summary>
    [Serializable]
    public class Punch
    {
        public static Vector3 DEFAULT_PUNCH_MOVE_PUNCH = new Vector3(0, 20, 0);
        public static Vector3 DEFAULT_PUNCH_ROTATE_PUNCH = new Vector3(0, 0, 30);
        public static Vector3 DEFAULT_PUNCH_SCALE_PUNCH = new Vector3(0.2f, 0.2f, 0);
        public const float DEFAULT_PUNCH_START_DELAY = 0f;
        public const float DEFAULT_PUNCH_DURATION = 0.3f;
        public const int DEFAULT_PUNCH_VIBRATO = 4;
        public const float DEFAULT_PUNCH_ELASTICITY = 0.5f;

        public PunchMove move;
        public PunchRotate rotate;
        public PunchScale scale;

#if UNITY_EDITOR
        public AnimBool moveIsExpanded = new AnimBool(false);
        public AnimBool rotateIsExpanded = new AnimBool(false);
        public AnimBool scaleIsExpanded = new AnimBool(false);

        public void UpdateAnimBools()
        {
            moveIsExpanded.target = move.enabled;
            rotateIsExpanded.target = rotate.enabled;
            scaleIsExpanded.target = scale.enabled;
        }
#endif

        public Punch()
        {
            move = new PunchMove();
            rotate = new PunchRotate();
            scale = new PunchScale();
        }
        public void Reset()
        {
            move.Reset();
            rotate.Reset();
            scale.Reset();
        }
        public Punch Copy()
        {
            Punch copy = new Punch()
            {
                move = move.Copy(),
                rotate = rotate.Copy(),
                scale = scale.Copy()
            };
            return copy;
        }
        public bool Enabled { get { return move.enabled || rotate.enabled || scale.enabled; } }
        public float TotalDuration
        {
            get
            {
                return Mathf.Max(move.enabled ? move.TotalDuration : 0,
                                 rotate.enabled ? rotate.TotalDuration : 0,
                                 scale.enabled ? scale.TotalDuration : 0);
            }
        }
    }

    /// <summary>
    /// Punches a Transform's anchoredPosition towards the given direction and then back to the starting one as if it was connected to the starting scale via an elastic.
    /// </summary>
    [Serializable]
    public class PunchMove
    {
        /// <summary>
        /// If TRUE, this animation will get executed by the Animator when triggered, FALSE otherwise (default: false).
        /// </summary>
        public bool enabled = false;
        /// <summary>
        /// The punch strength (added to the Transform's current position).
        /// </summary>
        public Vector3 punch = Punch.DEFAULT_PUNCH_MOVE_PUNCH;
        /// <summary>
        /// Start delay for the animation.
        /// </summary>
        public float startDelay = Punch.DEFAULT_PUNCH_START_DELAY;
        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public float duration = Punch.DEFAULT_PUNCH_DURATION;
        /// <summary>
        /// Indicates how much will the punch vibrate.
        /// </summary>
        public int vibrato = Punch.DEFAULT_PUNCH_VIBRATO;
        /// <summary>
        /// Represents how much (0 to 1) the vector will go beyond the starting values when bouncing backwards. 
        /// 1 creates a full oscillation between the punch position and the opposite position, while 0 oscillates only between the punch position and the start position.
        /// </summary>
        public float elasticity = Punch.DEFAULT_PUNCH_ELASTICITY;

        public PunchMove()
        {
            Reset();
        }
        public void Reset()
        {
            enabled = false;
            punch = Punch.DEFAULT_PUNCH_MOVE_PUNCH;
            startDelay = Punch.DEFAULT_PUNCH_START_DELAY;
            duration = Punch.DEFAULT_PUNCH_DURATION;
            vibrato = Punch.DEFAULT_PUNCH_VIBRATO;
            elasticity = Punch.DEFAULT_PUNCH_ELASTICITY;
        }
        public void UpdateValues(PunchMove p)
        {
            enabled = p.enabled;
            punch = p.punch;
            startDelay = p.startDelay;
            duration = p.duration;
            vibrato = p.vibrato;
            elasticity = p.elasticity;

        }
        public PunchMove Copy()
        {
            PunchMove copy = new PunchMove()
            {
                enabled = enabled,
                punch = punch,
                duration = duration,
                startDelay = startDelay,
                vibrato = vibrato,
                elasticity = elasticity
            };
            return copy;
        }
        public float TotalDuration { get { return startDelay + duration; } }
    }

    /// <summary>
    /// Punches a Transform's localRotation towards the given rotation and then back to the starting one as if it was connected to the starting scale via an elastic.
    /// </summary>
    [Serializable]
    public class PunchRotate
    {
        /// <summary>
        /// If TRUE, this animation will get executed by the Animator when triggered, FALSE otherwise (default: false).
        /// </summary>
        public bool enabled = false;
        /// <summary>
        /// The punch strength (added to the Transform's current rotation).
        /// </summary>
        public Vector3 punch = Punch.DEFAULT_PUNCH_ROTATE_PUNCH;
        /// <summary>
        /// Start delay for the animation.
        /// </summary>
        public float startDelay = Punch.DEFAULT_PUNCH_START_DELAY;
        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public float duration = Punch.DEFAULT_PUNCH_DURATION;
        /// <summary>
        /// Indicates how much will the punch vibrate.
        /// </summary>
        public int vibrato = Punch.DEFAULT_PUNCH_VIBRATO;
        /// <summary>
        /// Represents how much (0 to 1) the vector will go beyond the starting values when bouncing backwards. 
        /// 1 creates a full oscillation between the punch rotation and the opposite rotation, while 0 oscillates only between the punch rotation and the start rotation.
        /// </summary>
        public float elasticity = Punch.DEFAULT_PUNCH_ELASTICITY;

        public PunchRotate()
        {
            Reset();
        }
        public void Reset()
        {
            enabled = false;
            punch = Punch.DEFAULT_PUNCH_ROTATE_PUNCH;
            startDelay = Punch.DEFAULT_PUNCH_START_DELAY;
            duration = Punch.DEFAULT_PUNCH_DURATION;
            vibrato = Punch.DEFAULT_PUNCH_VIBRATO;
            elasticity = Punch.DEFAULT_PUNCH_ELASTICITY;
        }
        public void UpdateValues(PunchRotate r)
        {
            enabled = r.enabled;
            punch = r.punch;
            startDelay = r.startDelay;
            duration = r.duration;
            vibrato = r.vibrato;
            elasticity = r.elasticity;

        }
        public PunchRotate Copy()
        {
            PunchRotate copy = new PunchRotate()
            {
                enabled = enabled,
                punch = punch,
                duration = duration,
                startDelay = startDelay,
                vibrato = vibrato,
                elasticity = elasticity
            };
            return copy;
        }
        public float TotalDuration { get { return startDelay + duration; } }
    }

    /// <summary>
    /// Punches a Transform's localScale towards the given size and then back to the starting one as if it was connected to the starting scale via an elastic.
    /// </summary>
    [Serializable]
    public class PunchScale
    {
        /// <summary>
        /// If TRUE, this animation will get executed by the Animator when triggered, FALSE otherwise (default: false).
        /// </summary>
        public bool enabled = false;
        /// <summary>
        /// The punch strength (added to the Transform's current scale).
        /// </summary>
        public Vector3 punch = Punch.DEFAULT_PUNCH_SCALE_PUNCH;
        /// <summary>
        /// Start delay for the animation.
        /// </summary>
        public float startDelay = Punch.DEFAULT_PUNCH_START_DELAY;
        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public float duration = Punch.DEFAULT_PUNCH_DURATION;
        /// <summary>
        /// Indicates how much will the punch vibrate.
        /// </summary>
        public int vibrato = Punch.DEFAULT_PUNCH_VIBRATO;
        /// <summary>
        /// Represents how much (0 to 1) the vector will go beyond the starting values when bouncing backwards. 
        /// 1 creates a full oscillation between the punch scale and the opposite scale, while 0 oscillates only between the punch scale and the start scale.
        /// </summary>
        public float elasticity = Punch.DEFAULT_PUNCH_ELASTICITY;

        public PunchScale()
        {
            Reset();
        }
        public void Reset()
        {
            enabled = false;
            punch = Punch.DEFAULT_PUNCH_SCALE_PUNCH;
            startDelay = Punch.DEFAULT_PUNCH_START_DELAY;
            duration = Punch.DEFAULT_PUNCH_DURATION;
            vibrato = Punch.DEFAULT_PUNCH_VIBRATO;
            elasticity = Punch.DEFAULT_PUNCH_ELASTICITY;
        }
        public void UpdateValues(PunchScale s)
        {
            enabled = s.enabled;
            punch = s.punch;
            startDelay = s.startDelay;
            duration = s.duration;
            vibrato = s.vibrato;
            elasticity = s.elasticity;

        }
        public PunchScale Copy()
        {
            PunchScale copy = new PunchScale()
            {
                enabled = enabled,
                punch = punch,
                duration = duration,
                startDelay = startDelay,
                vibrato = vibrato,
                elasticity = elasticity
            };
            return copy;
        }
        public float TotalDuration { get { return startDelay + duration; } }
    }

    #endregion
}
