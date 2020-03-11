// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using System.Collections.Generic;

namespace DoozyUI
{
    public partial class ControlPanelWindow : QWindow
    {
        List<LinkButtonData> linkButtons = new List<LinkButtonData>
        {
            new LinkButtonData()
            {
                text = "Online PDF Manual",
                url = "https://goo.gl/yD152s",
                linkButton = Style.LinkButton.Manual
            },

            new LinkButtonData()
            {
                text = "YouTube Playlist - Video Tutorials for version 2.9 and up",
                url = "https://goo.gl/Y2Qifg",
                linkButton = Style.LinkButton.YouTube
            },

            new LinkButtonData()
            {
                text = "Help Center - Documentation, How-To & FAQ",
                url = "https://doozyentertainment.zendesk.com/",
                linkButton = Style.LinkButton.Link
            },

            new LinkButtonData()
            {
                text = "Community Forum - Feature Requests & General Discussions",
                url = "https://doozyentertainment.zendesk.com/hc/en-us/community/topics",
                linkButton = Style.LinkButton.Link
            },

             new LinkButtonData()
            {
                text = "Support Request - Open a Support Ticket",
                url = "https://doozyentertainment.zendesk.com/hc/en-us/requests/new",
                linkButton = Style.LinkButton.Link
            },
        };

        void InitPageHelp()
        {

        }

        void DrawPageHelp()
        {
            DrawPageHeader("Help", QColors.Blue, "", QUI.IsProSkin ? QColors.UnityLight : QColors.UnityMild, DUIResources.pageIconHelp);
            QUI.Space(SPACE_16);

            QUI.DrawLinkButtonsList(linkButtons, 0, WindowSettings.CurrentPageContentWidth);
        }
    }
}