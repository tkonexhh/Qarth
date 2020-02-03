using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class VectorEngineTester : MonoBehaviour
    {
        [SerializeField]
        private bool m_Debug = false;
        [SerializeField]
        private Vector3 m_TargetValue;

        VectorEngine m_Engine;

        private void Awake()
        {
            m_Engine = new VectorEngine(3);
            AddSourceData();
        }

        private void OnValidate()
        {
            if (m_Debug)
            {
                m_Debug = false;
                VectorDataTester target = new VectorDataTester("Target", m_TargetValue.x, m_TargetValue.y, m_TargetValue.z);
                m_Engine.SortData(target);

                for (int i = 0; i < 3; ++i)
                {
                    VectorDataTester data = m_Engine.GetData(i) as VectorDataTester;
                    Log.i("Result:" + data.name);
                }
            }
        }

        private void AddSourceData()
        {
            m_Engine.AddSourceData(new VectorDataTester("X1", 1, 0, 0));
            m_Engine.AddSourceData(new VectorDataTester("X2", 2, 0, 0));
            m_Engine.AddSourceData(new VectorDataTester("X3", 3, 0, 0));
            m_Engine.AddSourceData(new VectorDataTester("X4", 4, 0, 0));
            m_Engine.AddSourceData(new VectorDataTester("X5", 5, 0, 0));

            m_Engine.AddSourceData(new VectorDataTester("Y1", 0, 1, 0));
            m_Engine.AddSourceData(new VectorDataTester("Y2", 0, 2, 0));
            m_Engine.AddSourceData(new VectorDataTester("Y3", 0, 3, 0));
            m_Engine.AddSourceData(new VectorDataTester("Y4", 0, 4, 0));
            m_Engine.AddSourceData(new VectorDataTester("Y5", 0, 5, 0));

            m_Engine.AddSourceData(new VectorDataTester("X1Y1", 1, 1, 0));
            m_Engine.AddSourceData(new VectorDataTester("X2Y2", 2, 2, 0));
            m_Engine.AddSourceData(new VectorDataTester("X3Y3", 3, 3, 0));
            m_Engine.AddSourceData(new VectorDataTester("X4Y4", 4, 4, 0));
            m_Engine.AddSourceData(new VectorDataTester("X5Y5", 5, 5, 0));
        }
    }

    public class VectorDataTester : VectorData
    {
        public string name;

        public override int vectorDimension
        {
            get
            {
                return 3;
            }
        }

        public VectorDataTester(string name, float a, float b, float c) : base()
        {
            this.name = name;

            m_VectorRawData[0] = a;
            m_VectorRawData[1] = b;
            m_VectorRawData[2] = c;
        }
    }
}
