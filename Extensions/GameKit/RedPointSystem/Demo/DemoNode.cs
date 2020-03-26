/************************
	FileName:/Qarth/Extensions/GameKit/RedPointSystem/Demo/DemoNode.cs
	CreateAuthor:xuhonghua
	CreateTime:3/20/2020 4:42:44 PM
************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GFrame.Demo
{
    public class DemoNode : MonoBehaviour
    {
        [SerializeField] private Text m_TxtNum;
        [SerializeField] private InputField m_InputNum;
        [SerializeField] private GameObject m_ObjRed;
        [SerializeField] private Button m_BtnAdd;
        [SerializeField] private Button m_BtnRemove;
        private RedPointNode m_Node;

        private void Awake()
        {
            m_TxtNum = GetComponentInChildren<Text>();
            m_InputNum.onEndEdit.AddListener(OnEndEdit);
            m_BtnAdd.onClick.AddListener(OnClickAdd);
            m_BtnRemove.onClick.AddListener(OnClickRemove);

        }

        public void BindNode(RedPointNode node)
        {
            m_Node = node;
            m_Node.OnNumChange += UpdateNum;
            UpdateUI();
            UpdateNum();
        }

        private void UpdateUI()
        {
            m_BtnRemove.gameObject.SetActive(m_Node.IsEnd());
            m_BtnAdd.gameObject.SetActive(m_Node.IsEnd());
        }
        private void UpdateNum()
        {
            m_InputNum.text = m_Node.PointNum.ToString();
        }

        private void OnEndEdit(string value)
        {

        }

        private void OnClickAdd()
        {
            m_Node.PointNum++;
            UpdateNum();
        }

        private void OnClickRemove()
        {
            m_Node.PointNum--;
            UpdateNum();
        }
    }

}