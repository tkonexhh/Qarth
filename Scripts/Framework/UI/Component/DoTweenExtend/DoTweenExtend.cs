using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

using UnityEngine.UI;
using DG.Tweening;

namespace Qarth
{
    public class TransformHandler
    {
        private Transform m_Target;

        private float m_DegreeValue;

        public float degreeValue
        {
            get { return m_DegreeValue; }
            set
            {
                m_DegreeValue = value;
                m_Target.localRotation = Degree2Quaternion(m_DegreeValue);
            }
        }

        protected Quaternion Degree2Quaternion(float degree)
        {
            return Quaternion.Euler(new Vector3(0, 0, degree));
        }

        public TransformHandler(Transform target)
        {
            m_Target = target;
        }
    }


    public static class DoTweenExtend
    {
        public delegate float FloatGetter();
        public delegate void FloatSetter(float x);

        public static Tweener DoScrollHorizontal(this ScrollRect target, float endValue, float duration)
        {
            DG.Tweening.Core.DOSetter<float> setter = (x) => { target.horizontalNormalizedPosition = x; };

            return DOTween.To(setter, target.horizontalNormalizedPosition, endValue, duration);
        }

        public static Tweener DoScrollVertical(this ScrollRect target, float endValue, float duration)
        {
            DG.Tweening.Core.DOSetter<float> setter = (x) => { target.verticalNormalizedPosition = x; };

            return DOTween.To(setter, target.verticalNormalizedPosition, endValue, duration);
        }

        public static Tweener DoCustomRotate(this TransformHandler target, float endValue, float duration)
        {
            DG.Tweening.Core.DOGetter<float> getter = () => { return target.degreeValue; };
            DG.Tweening.Core.DOSetter<float> setter = (value) => { target.degreeValue = value; };
            return DOTween.To(getter, setter, endValue, duration);
        }
    }
}
