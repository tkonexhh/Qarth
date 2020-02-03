using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public interface IVectorData
    {
        int vectorDimension
        {
            get;
        }

        float[] vectorRawData
        {
            get;
        }
        
        float vectorScore
        {
            get;
            set;
        }
    }
}
