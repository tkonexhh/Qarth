using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.Text;

namespace Qarth
{
    public class BigIntHelper
    {
        private static StringBuilder m_Builder = new StringBuilder();

        public static string Convert2CurrencyFormat(string msg, int offset = 3)
        {
            m_Builder.Remove(0, m_Builder.Length);
            char[] ca = msg.ToCharArray();
            int length = ca.Length;
            for (int i = 0; i < length; ++i)
            {
                m_Builder.Append(ca[i]);
                int index = length - i - 1;
                if (index % offset == 0 && index > 1)
                {
                    m_Builder.Append(',');
                }
            }

            return m_Builder.ToString();
        }
    }
}
