using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GFrame.Drama.Demo
{
    public class DramaPanel : MonoBehaviour
    {
        [SerializeField] private Button m_BtnBg;
        [SerializeField] private Text m_TxtTitle;
        [SerializeField] private Text m_TxtContent;
        [SerializeField] private GameObject m_ChooseRoot;
        [SerializeField] private VerticalLayoutGroup m_ChooseBtnRoot;
        [SerializeField] private DramaChooseBtn m_BtnPrefab;

        [SerializeField] private DialogueBluePrint m_BluePrint;
        private DialoguePlayer m_Player;

        private void Awake()
        {
            m_BtnBg.onClick.AddListener(() =>
            {
                m_Player.Next();
            });
            m_ChooseRoot.gameObject.SetActive(false);
            m_Player = new DialoguePlayer();

            m_Player.OnDoChoose = (choose) =>
           {
               m_ChooseRoot.gameObject.SetActive(true);
               m_ChooseBtnRoot.transform.RemoveAllChild();
               m_TxtTitle.text = choose.GetTitle();
               m_TxtContent.text = choose.GetContent();
               var chooses = choose.Chooses;
               for (int i = 0; i < chooses.Count; i++)
               {
                   var choosePicked = chooses[i];
                   var go = Instantiate(m_BtnPrefab.gameObject, Vector3.zero, Quaternion.identity, m_ChooseBtnRoot.transform);
                   var chooseBtn = go.GetComponent<DramaChooseBtn>();
                   chooseBtn.Txt.text = chooses[i].Title;
                   chooseBtn.Btn.onClick.AddListener(() =>
                   {
                       m_Player.PickChoose(choosePicked);
                       m_ChooseBtnRoot.transform.RemoveAllChild();
                       m_ChooseRoot.gameObject.SetActive(false);
                   });
               }
           };

            m_Player.OnDoContent = (content) =>
            {
                m_TxtTitle.text = content.GetTitle();
                m_TxtContent.text = content.GetContent();
            };

            m_Player.OnDoFinish = (finish) =>
            {
                m_TxtTitle.text = "";
                m_TxtContent.text = "";
                gameObject.SetActive(false);
            };

            m_Player.PlayDialogue(m_BluePrint);
        }




    }

}