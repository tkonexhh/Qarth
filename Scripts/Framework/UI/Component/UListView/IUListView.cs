//  Desc:        Framework For Game Develop with Unity3d
//  Copyright:   Copyright (C) 2017 SnowCold. All rights reserved.
//  WebSite:     https://github.com/SnowCold/Qarth
//  Blog:        http://blog.csdn.net/snowcoldgame
//  Author:      SnowCold
//  E-mail:      snowcold.ouyang@gmail.com
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Qarth
{	
	public enum Layout
	{
		Vertical,
		Horizontal
	}


	public class CellHandler
	{
		private GameObject m_HandlerObject;
		private RectTransform m_Root;
		private int m_DataIndex;

		public CellHandler(GameObject obj)
		{
			m_HandlerObject = obj;
			m_Root = m_HandlerObject.transform as RectTransform;
		}

		public int dataIndex
		{
			get { return m_DataIndex; }
			set { m_DataIndex = value;}
		}

		public RectTransform root
		{
			get { return m_Root; }
		}

		public GameObject handlerObjcet
		{
			set { m_HandlerObject = value; }
		}

		public void UpdatePosition(Vector3 position)
		{
			m_Root.pivot = m_Root.anchorMin = m_Root.anchorMax = new Vector2(0.5f, 0.5f);
			m_Root.anchoredPosition = position;
			m_Root.localScale = Vector3.one;
		}

		public void UpdateActivityState()
		{
			if (m_DataIndex >= 0)
			{
				ChangeGameObjectState(true);
			}
			else
			{
				ChangeGameObjectState(false);
			}
		}

		private void ChangeGameObjectState(bool state)
		{
			if (m_HandlerObject.activeSelf != state)
			{
				m_HandlerObject.SetActive(state);
			}
		}
	}

	[RequireComponent(typeof(ScrollRect))]
	public abstract class IUListView : MonoBehaviour, IPointerClickHandler//, IBeginDragHandler, IEndDragHandler
    {
        public delegate void CellRenderer(Transform root, int index);

		[Tooltip("List Item Object, must set")]
		public GameObject			itemPrefab;
		public Layout 				layout;
        public Vector2 				padding;
		public Vector2				spacing;
		public bool					needMask;

		protected ScrollRect 		m_ScrollRect;
		protected bool				initialized = false;

        public RectTransform		content;
        public RectTransform        viewPort;
		protected Vector2			scrollRectSize;
		protected int				lastStartInex = 0;
        protected int               lstCount;
		protected Vector2			leftTopCorner = Vector2.zero;
		private bool				leftTopCornerInit = false;
		private List<CellHandler> 	m_CellHandlerList;

        private CellRenderer m_CellRenderer;

        public void SetCellRenderer(CellRenderer renderer)
        {
            m_CellRenderer = renderer;
        }

        public Vector2 normalizedPosition
        {
            get { return m_ScrollRect.normalizedPosition; }
        }

		public virtual void Init()
		{
            if (m_ScrollRect == null)
            {
                m_ScrollRect = GetComponent<ScrollRect>();
            }

			m_CellHandlerList = new List<CellHandler>();
            // set attributes of scrollrect
            m_ScrollRect.onValueChanged.AddListener(OnValueChanged);

			// set the scroll direction
			switch (layout) 
			{
			case Layout.Horizontal:
				m_ScrollRect.horizontal = true;
				m_ScrollRect.vertical = false;
				break;
			case Layout.Vertical:
				m_ScrollRect.horizontal = false;
				m_ScrollRect.vertical = true;
				break;
			}

            // add a scrollrect content
            if (content == null)
            {
                GameObject go = new GameObject();
                go.name = "content";
                content =  go.AddComponent<RectTransform>();//go.AddComponent(typeof(RectTransform)) as RectTransform;
                content.SetParent(transform);
                go.layer = LayerDefine.LAYER_UI;
            }

			content.pivot = new Vector2(0, 1);
			content.anchorMin = content.anchorMax = content.pivot;
			content.anchoredPosition = Vector2.zero;
            Vector3 localPos = content.transform.localPosition;
            localPos.z = 0;
            content.transform.localPosition = localPos;
			content.localScale = Vector3.one;
			m_ScrollRect.content = content;

			// record some sizes
			RectTransform scrollRectTransform = m_ScrollRect.transform as RectTransform;
			scrollRectSize = scrollRectTransform.rect.size;

			// add mask
			if (needMask) 
			{
				Image image = gameObject.AddComponent(typeof(Image)) as Image;
				image.color = new Color32(0, 0, 0, 5);
				gameObject.AddComponent(typeof(Mask));
			}
		}

        public void Jump2Index(int index)
        {
            int dataCount = GetDataCount();

            dataCount = dataCount > 0 ? dataCount : 0;

            float precent = (float)index / dataCount;
            precent = Mathf.Min(0.999f, precent);
            precent = Mathf.Max(0.001f, precent);
            Vector2 oldPrecent = m_ScrollRect.normalizedPosition;
            oldPrecent.y = precent;
            m_ScrollRect.normalizedPosition = oldPrecent;
        }

        public void SetDataCount(int count)
        {
            if (false == initialized)
            {
                Init();
                initialized = true;
            }

            m_CellPrecent = 1.0f / (count - 1);

            ResetCellHandle();

            lastStartInex = -1;
            this.lstCount = count;
            //this.lstData = lstData;
            RefreshListView();
        }


        public void ForceUpdateList()
        {
            if (false == initialized)
            {
                return;
            }

            int startIndex = GetStartIndex();
            RefreshListView();
            lastStartInex = startIndex;
        }

		private void OnValueChanged(Vector2 pos)
		{
			int startIndex = GetStartIndex();
			if (startIndex != lastStartInex 
			    && startIndex >= 0)
			{
				RefreshListView();
				lastStartInex = startIndex;
			}
		}

        protected virtual void AdjustViewportSize(Vector2 contentSize)
        {

        }

        private int m_PreDataIndexMin = int.MaxValue;
        private int m_PreDataIndexMax = int.MinValue;

        public RectTransform GetCellRootByIndex(int index)
        {
            if (m_CellHandlerList == null)
            {
                return null;
            }

            for (int i = 0; i < m_CellHandlerList.Count; ++i)
            {
                if (m_CellHandlerList[i].dataIndex == index)
                {
                    return m_CellHandlerList[i].root;
                }
            }

            return null;
        }

		protected void UpdateCellActivityState()
		{
			for (int i = 0; i < m_CellHandlerList.Count; ++i)
			{
				m_CellHandlerList[i].UpdateActivityState();
			}
		}

		protected void ResetUnUsedCell(int startindex, int endIndex)
		{
			for (int i = 0; i < m_CellHandlerList.Count; ++i)
			{
				if (!IsIntInRange(m_CellHandlerList[i].dataIndex, startindex, endIndex))
				{
					m_CellHandlerList[i].dataIndex = -1;
				}
			}
		}

        protected void ResetCellHandle()
        {
            for (int i = 0; i < m_CellHandlerList.Count; ++i)
            {
                m_CellHandlerList[i].dataIndex = -1;
            }

            m_PreDataIndexMin = int.MaxValue;
            m_PreDataIndexMax = int.MinValue;
    }

        protected CellHandler GetUnUseCell()
		{
			for (int i = 0; i < m_CellHandlerList.Count; ++i)
			{
				if (m_CellHandlerList[i].dataIndex < 0)
				{
					return m_CellHandlerList[i];
				}
			}

			CellHandler handler = new CellHandler(GameObject.Instantiate(itemPrefab, content, false) as GameObject);
			m_CellHandlerList.Add(handler);
			return handler;
		}

		protected bool IsIntInRange(int v, int startIndex, int endIndex)
		{
			return v >= startIndex && v < endIndex;
		}

        protected void RefreshListView()
		{
			// set the content size
			Vector2 size = GetContentSize ();
			RectTransform contentRectTransform = content.transform as RectTransform;

            AdjustViewportSize(size);

            contentRectTransform.sizeDelta = size;

            // set the item postion and data
            int startIndex = GetStartIndex();

			if (startIndex < 0)	startIndex = 0;

            int showItemNum = GetCurrentShowItemNum();

			ResetUnUsedCell(startIndex, startIndex + showItemNum);

			for (int i = 0; i < showItemNum; ++i)
			{
                int dataIndex = startIndex + i;
                if (dataIndex >= lstCount)
                {
                    break;
                }

                //Opt
                if (dataIndex >= m_PreDataIndexMin && dataIndex <= m_PreDataIndexMax)
                {
                    continue;
                }

				///////////////
				CellHandler cell = GetUnUseCell();
				cell.dataIndex = dataIndex;
				cell.UpdatePosition(GetItemAnchorPos(dataIndex));
				///////////////

/*
				GameObject go = GetItemGameObject(content, i);
                if (false == go.activeSelf)
                {
                    go.SetActive(true);
                }
                RectTransform trans = go.transform as RectTransform;
				trans.pivot = trans.anchorMin = trans.anchorMax = new Vector2(0.5f, 0.5f);
				trans.anchoredPosition = GetItemAnchorPos(dataIndex);
				trans.localScale = Vector3.one;
*/
                if (m_CellRenderer != null)
                {
                    m_CellRenderer(cell.root, dataIndex);
                }
			}

            m_PreDataIndexMin = startIndex;
            m_PreDataIndexMax = startIndex + showItemNum - 1;

			UpdateCellActivityState();
            // dont show the extra items shown before
            //HideNonuseableItems ();

			// set the progress: 
			int dataCount = GetDataCount();
			dataCount -= (GetMaxShowItemNum ()-2);
			if (dataCount < 1) dataCount = 1;
			float progress = (startIndex + 1)/(float)dataCount;
			progress = Mathf.Clamp01(progress);
			OnProgress(progress);
		}

		/// <summary>
		/// Gets the current show item number.
		/// </summary>
		/// <returns>The current show item number.</returns>
		protected int GetCurrentShowItemNum()
		{
			int startIndex = GetStartIndex ();
			int maxShowNum = GetMaxShowItemNum ();
			int maxItemNum = lstCount - startIndex;
			return maxShowNum < maxItemNum ? maxShowNum : maxItemNum;
		}

		/// <summary>
		/// Gets the top left corner screen point.
		/// </summary>
		/// <returns>The top left corner screen point.</returns>
		private Vector2 GetTopLeftCornerScreenPoint()
		{
			if (false == leftTopCornerInit) 
			{
				RectTransform rectTrans = m_ScrollRect.transform as RectTransform;
				Vector3[] corners = new Vector3[4];
				rectTrans.GetWorldCorners (corners);
				Canvas canvas = GetComponentInParent<Canvas> ();
				if (null != canvas && null != canvas.worldCamera && RenderMode.ScreenSpaceOverlay != canvas.renderMode) {
					Camera cam = canvas.worldCamera;
					corners [1] = cam.WorldToScreenPoint (corners [1]);
				}
				leftTopCorner = new Vector2 (corners [1].x, corners [1].y);
			}
			return leftTopCorner;
		}

		/// <summary>
		/// Raises the pointer enter event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerClick(PointerEventData eventData)
		{
            //get the pos relative to left-top corner of the scrollview
            Vector2 clickPos =  eventData.position - GetTopLeftCornerScreenPoint();
			Vector2 anchorPosition = -content.anchoredPosition;

			anchorPosition += clickPos;
			anchorPosition.x -= content.rect.size.x / 2;
			anchorPosition.y += content.rect.size.y / 2;

			// set the item postion and data
			int startIndex = GetStartIndex ();
			if (startIndex < 0)	startIndex = 0;
			
			for (int i=0; i<GetCurrentShowItemNum(); ++i)
			{
				Vector2 itemAnchorPos = GetItemAnchorPos(startIndex + i);
				Vector2 itemSize = GetItemSize(startIndex + i);
				if(Mathf.Abs(anchorPosition.x - itemAnchorPos.x) <= itemSize.x/2 &&
				   Mathf.Abs(anchorPosition.y - itemAnchorPos.y) <= itemSize.y/2)
				{
					OnClick(startIndex + i);
					break;
				}
			}
		}

        private DG.Tweening.Tweener m_AdjustTween;

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!m_IsAutoAdjustPosition)
            {
                return;
            }

            switch (layout)
            {
                case Layout.Horizontal:
                    {
                        float currentValue = m_ScrollRect.horizontalNormalizedPosition;
                        Move2Target(currentValue);
                    }
                    break;
                case Layout.Vertical:
                    {
                        float currentValue = m_ScrollRect.horizontalNormalizedPosition;
                        Move2Target(currentValue);
                    }
                    break;
            }
        }



        protected void Move2Target(float precent)
        {
            switch (layout)
            {
                case Layout.Horizontal:
                    {
                        precent = Mathf.Max(0, Mathf.Min(1.0f, precent));
                        float xV = precent * (lstCount - 1);
                        int xxv = Mathf.RoundToInt(xV);
                        float target = xxv * m_CellPrecent;
                        m_ScrollRect.DoScrollHorizontal(target, 0.25f);
                    }
                    break;
                case Layout.Vertical:
                    {
                        precent = Mathf.Max(0, Mathf.Min(1.0f, precent));
                        float xV = precent * (lstCount - 1);
                        int xxv = Mathf.RoundToInt(xV);
                        float target = xxv * m_CellPrecent;
                        m_ScrollRect.DoScrollVertical(target, 0.25f);
                    }
                    break;
            }
        }

        public void MoveToTarget(float precent)
        {
            Move2Target(precent);
        }

		public void MoveByIndex(int index)
		{
			float targetValue = 0;

			targetValue += index * 	m_CellPrecent;

			Move2Target(targetValue);
		}

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (m_AdjustTween != null)
            {
                m_AdjustTween.Kill();
                m_AdjustTween = null;
            }
        }

        protected bool m_IsAutoAdjustPosition;
        private float m_CellPrecent;

        public bool autoAdjustPosition
        {
            get { return m_IsAutoAdjustPosition; }
            set { m_IsAutoAdjustPosition = value; }
        }

		public float cellPrecent
		{
			get
			{
				return m_CellPrecent;
			}
		}

        public void Move2Next()
        {
            float targetValue = 0;

            switch (layout)
            {
                case Layout.Horizontal:
                    targetValue = m_ScrollRect.horizontalNormalizedPosition;
                    break;
                case Layout.Vertical:
                    targetValue = m_ScrollRect.verticalNormalizedPosition;
                    break;
            }

            targetValue += m_CellPrecent;

            Move2Target(targetValue);
        }

        public void Move2Pre()
        {
            float targetValue = 0;

            switch (layout)
            {
                case Layout.Horizontal:
                    targetValue = m_ScrollRect.horizontalNormalizedPosition;
                    break;
                case Layout.Vertical:
                    targetValue = m_ScrollRect.verticalNormalizedPosition;
                    break;
            }

            targetValue -= m_CellPrecent;

            Move2Target(targetValue);
        }

        /// <summary>
        /// Raises the click event.
        /// </summary>
        /// <param name="pos">Position.</param>
        public virtual void OnClick(int index)
		{
		}

		/// <summary>
		/// Raises the progress event when progress change
		/// </summary>
		/// <param name="progress">Progress.</param>
		public virtual void OnProgress(float progress)
		{
		}

		/// <summary>
		/// Gets the data count.
		/// default return 1
		/// </summary>
		/// <returns>The data count.</returns>
		public virtual int GetDataCount()
		{
            return lstCount;
		}

		/// <summary>
		/// Gets the max show item number.
		/// </summary>
		/// <returns>The max show item number.</returns>
		public abstract int			GetMaxShowItemNum();
		/// <summary>
		/// Gets the rect tranform size of the content of the ScrollRect
		/// </summary>
		/// <returns>The content size.</returns>
		public abstract Vector2 	GetContentSize();
		/// <summary>
		/// Gets the anchor position of the item indexed index
		/// Assume that anchorMin = anchorMax = pivot = new Vector(0.5f, 0.5f)
		/// </summary>
		/// <returns>The item anchor position.</returns>
		/// <param name="index">index of item</param>
		public abstract Vector2 	GetItemAnchorPos(int index);
		/// <summary>
		/// Gets the start index of the item to be shown
		/// </summary>
		/// <returns>The start index(start from 0)</returns>
		public abstract int 		GetStartIndex();
		/// <summary>
		/// Gets the item game object.
		/// </summary>
		/// <returns>The item game object.</returns>
		/// <param name="index">Index.</param>
		public abstract GameObject	GetItemGameObject(Transform content, int index);
		/// <summary>
		/// Hides the nonuseable items.
		/// </summary>
		public abstract	void		HideNonuseableItems();
		/// <summary>
		/// Gets the size of the item of specified index
		/// </summary>
		/// <returns>The item size.</returns>
		/// <param name="index">Index.</param>
		public abstract Vector2		GetItemSize(int index);
	}
}
