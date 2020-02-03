using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    //v1.0不处理分区
    public class VectorEngine
    {
        private List<VectorData> m_SourceDataList;
        private int m_Dimension;

        private float[] m_DiamensionWeight;
        private float m_CalculateTemp;

        public VectorEngine(int dimension, float[] weight = null)
        {
            m_SourceDataList = new List<VectorData>();
            m_Dimension = dimension;
            if (weight == null)
            {
                weight = new float[m_Dimension];
                for (int i = 0; i < m_Dimension; ++i)
                {
                    weight[i] = 1;
                }
            }

            SetDiamensionWeight(weight);
        }

        public void SetDiamensionWeight(float[] weight)
        {
            m_DiamensionWeight = weight;
        }

        public void AddSourceData(VectorData data)
        {
            m_SourceDataList.Add(data);
        }

        public void AddSourceData(VectorData[] data)
        {
            m_SourceDataList.AddRange(data);
        }

        public VectorData Query(VectorData target)
        {
            if (m_SourceDataList.Count == 0)
            {
                return null;
            }

            for (int i = m_SourceDataList.Count - 1; i >= 0; --i)
            {
                float score = SqrMagnitude(target, m_SourceDataList[i]);
                m_SourceDataList[i].vectorScore = score;
            }

            m_SourceDataList.Sort(VectorDataComparer);
            return m_SourceDataList[0];
        }

        public void SortData(VectorData target)
        {
            if (m_SourceDataList.Count == 0)
            {
                return;
            }

            for (int i = m_SourceDataList.Count - 1; i >= 0; --i)
            {
                float score = SqrMagnitude(target, m_SourceDataList[i]);
                m_SourceDataList[i].vectorScore = score;
            }

            m_SourceDataList.Sort(VectorDataComparer);
        }

        public VectorData GetData(int index)
        {
            if (index < 0 || index >= m_SourceDataList.Count)
            {
                return null;
            }

            return m_SourceDataList[index];
        }

        protected int VectorDataComparer(VectorData a, VectorData b)
        {
            if (a.vectorScore > b.vectorScore)
            {
                return 1;
            }
            else if (a.vectorScore < b.vectorScore)
            {
                return -1;
            }

            return 0;
        }

        public float SqrMagnitude(VectorData target, VectorData source)
        {
            float[] targetData = target.vectorRawData;
            float[] sourceData = source.vectorRawData;

            float result = 0;

            int dimension = target.vectorDimension;

            for (int i = 0; i < dimension; ++i)
            {
                m_CalculateTemp = targetData[i] - sourceData[i];

                result += m_CalculateTemp * m_CalculateTemp * m_DiamensionWeight[i];
            }

            return result;
        }

        public float SqrMagnitude2(VectorData target, VectorData source)
        {
            float[] targetData = target.vectorRawData;
            float[] sourceData = source.vectorRawData;

            float result = 0;

            int dimension = target.vectorDimension;

            for (int i = 0; i < dimension; ++i)
            {
                float p = targetData[i] / sourceData[i];
                if (p < 0.8f)
                {
                    continue;
                }
                else if (p < 1.0f)
                {
                    result += p * 100;
                }
                else if (p < 1.2f)
                {
                    result += 100 + (p - 1) * 1000;
                }
                else
                {
                    result += 200 - (p - 1.5f) * 50;
                }

                m_CalculateTemp = targetData[i] - sourceData[i];

                result += m_CalculateTemp * m_CalculateTemp * m_DiamensionWeight[i];
            }

            return result;
        }

        private float CalculateSingleAxis(float target, float source)
        {
            float p = target / source;
            if (p < 0.8f)
            {
                return 0;
            }
            else if (p < 1.0f)
            {
                return p * 100;
            }
            else if (p < 1.2f)
            {
                return 100 + (p - 1) * 1000;
            }
            else if (p < 2)
            {
                return 300 - (p - 1.2f) * 1000;
            }
            else
            {
                return 100;
            }
        }
    }
}
