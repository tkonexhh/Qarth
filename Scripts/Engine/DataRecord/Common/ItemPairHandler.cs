using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class ItemPairHandler : DataDirtyHandler
    {
        public List<ItemPair> a;

        private Dictionary<int, ItemPair> m_ItemMap;

        public Dictionary<int, ItemPair> GetItemMap()
        {

            if (m_ItemMap == null)
            {
                m_ItemMap = new Dictionary<int, ItemPair>();
                if (a == null)
                {
                    a = new List<ItemPair>();
                }
                else
                {
                    for (int i = a.Count - 1; i >= 0; --i)
                    {
                        a[i].SetDirtyRecorder(m_Recorder);
                        m_ItemMap.Add(a[i].GetItemID(), a[i]);
                    }
                }
            }

            return m_ItemMap;
        }

        public ItemPair GetItemPair(int itemID)
        {
            ItemPair pair = null;

            GetItemMap().TryGetValue(itemID, out pair);

            return pair;
        }

        public int GetItemCount(int itemID)
        {
            ItemPair pair = GetItemPair(itemID);

            if (pair == null)
            {
                return 0;
            }

            return pair.GetItemCount();
        }

        public void AddItemCount(int itemID, int count)
        {
            ItemPair pair = GetItemPair(itemID);
            if (pair == null)
            {
                pair = AddItemPair(itemID);
            }

            pair.AddItemCount(count);
        }

        protected ItemPair AddItemPair(int itemID)
        {
            ItemPair pair = new ItemPair();
            pair.SetDirtyRecorder(m_Recorder);
            pair.SetItemID(itemID);

            GetItemMap().Add(itemID, pair);
            a.Add(pair);
            SetDataDirty();
            return pair;
        }
    }
}
