using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class VectorData : IVectorData
    {
        protected float[] m_VectorRawData;
        protected float m_VectorScore;
        protected int m_VectorDimension;

        public VectorData(int vec)
        {
            m_VectorDimension = vec;
            m_VectorRawData = new float[vectorDimension];
        }

        public VectorData()
        {
            m_VectorDimension = 2;
            m_VectorRawData = new float[vectorDimension];
        }

        public virtual int vectorDimension
        {
            get
            {
                return m_VectorDimension;
            }
        }

        public float this[int index]
        {
            get
            {
                return m_VectorRawData[index];
            }

            set
            {
                m_VectorRawData[index] = value;
            }
        }

        public float[] vectorRawData
        {
            get { return m_VectorRawData; }
            set { m_VectorRawData = value; }
        }

        public float vectorScore
        {
            get
            {
                return m_VectorScore;
            }
            set
            {
                m_VectorScore = AdjustValue(value);
            }
        }

        protected virtual float AdjustValue(float value)
        {
            return value;
        }
    }
}
