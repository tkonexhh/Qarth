/************************
	FileName:/Qarth/Extensions/GameKit/RedPointSystem/Demo/RedPointDemo.cs
	CreateAuthor:xuhonghua
	CreateTime:3/20/2020 3:13:18 PM
************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFrame.Demo
{
    public class RedPointDemo : MonoBehaviour
    {
        [SerializeField] private DemoNode m_NodeRoot;
        [SerializeField] private DemoNode m_Node1;
        [SerializeField] private DemoNode m_Node2;
        [SerializeField] private DemoNode m_Node3;

        [SerializeField] private DemoNode m_Node1_1;
        [SerializeField] private DemoNode m_Node1_2;
        void Start()
        {
            var demoModule = new DemoRedPointModule();
            RedPointSystem.S.AddModule("Demo", demoModule);


            RedPointNode node1 = new RedPointNode();
            RedPointNode node2 = new RedPointNode();
            RedPointNode node3 = new RedPointNode();


            RedPointNode node1_1 = new RedPointNode();
            RedPointNode node1_2 = new RedPointNode();
            // RedPointNode node1_3 = new RedPointNode();
            node1.AddNode(node1_1);
            node1.AddNode(node1_2);
            // node1.AddNode(node1_3);
            demoModule.RootNode.AddNode(node1);
            demoModule.RootNode.AddNode(node2);
            demoModule.RootNode.AddNode(node3);
            RedPointSystem.S.Refesh();

            m_NodeRoot.BindNode(demoModule.RootNode);
            m_Node1.BindNode(node1);
            m_Node2.BindNode(node2);
            m_Node3.BindNode(node3);
            m_Node1_1.BindNode(node1_1);
            m_Node1_2.BindNode(node1_2);

        }
    }

}