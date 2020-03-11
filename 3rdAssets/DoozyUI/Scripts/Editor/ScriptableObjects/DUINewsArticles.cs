// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEngine.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoozyUI.Internal
{
    public class DUINewsArticles : ScriptableObject
    {
        public static DUINewsArticles _instance;
        public static DUINewsArticles Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = Q.GetResource<DUINewsArticles>(DUI.RESOURCES_PATH_NEWS_ARTICLES, "DUINewsArticles");

#if UNITY_EDITOR
                    if(_instance == null)
                    {
                        _instance = Q.CreateAsset<DUINewsArticles>(DUI.RELATIVE_PATH_NEWS_ARTICLES, "DUINewsArticles");
                    }
#endif
                }
                return _instance;
            }
        }

        public List<NewsArticleData> articles = new List<NewsArticleData>();

    }

    [Serializable]
    public class NewsArticleData
    {
        public string title;
        public string content;

        public NewsArticleData()
        {
            Reset();
        }

        public NewsArticleData(string title, string content)
        {
            this.title = title;
            this.content = content;
        }

        public NewsArticleData(NewsArticleData newsArticleData)
        {
            this.title = newsArticleData.title;
            this.content = newsArticleData.content;
        }

        public void Reset()
        {
            this.title = "";
            this.content = "";
        }
    }

}
