using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class ItemPair : DataDirtyHandler
    {
        public int a;
        public EInt b;

        public int GetItemID()
        {
            return a;
        }

        public void SetItemID(int id)
        {
            a = id;
            SetDataDirty();
        }

        public int GetItemCount()
        {
            return b;
        }

        public void AddItemCount(int count)
        {
            b += count;
            SetDataDirty();
        }

        public void SetItemCount(int count)
        {
            b = count;
            SetDataDirty();
        }

        public bool GetIsEmpty()
        {
            return b <= 0;
        }
    }
}
