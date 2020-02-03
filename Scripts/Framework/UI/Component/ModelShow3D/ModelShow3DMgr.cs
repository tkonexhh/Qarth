using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class ModelShow3DMgr : TMonoSingleton<ModelShow3DMgr>
    {
        [SerializeField]
        protected int m_OffsetX = 100;

        private int m_NextCount = 0;

        public Vector3 RequestNextPosition()
        {
            int childCount = transform.childCount;

            if (childCount == 1)
            {
                m_NextCount = 0;
            }
            return new Vector3(m_OffsetX * (++m_NextCount), 0, 0);
        }

        public void AddModelShow(ModelShow3D target)
        {
            Transform cameraRoot = target.cameraRoot;
            cameraRoot.SetParent(transform, true);

            Vector3 nextPosition = RequestNextPosition();
            cameraRoot.transform.position = nextPosition;
            cameraRoot.transform.localScale = Vector3.one;

            UITools.SetGameObjectLayer(gameObject, LayerDefine.LAYER_AVATAR_SHOW);
        }
    }
}
