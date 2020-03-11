// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using UnityEditor;

namespace DoozyUI.Internal
{
    [InitializeOnLoad]
    public class DUISymbolLoader
    {
        static DUISymbolLoader()
        {
            EditorApplication.update += RunOnce;
        }

        static void RunOnce()
        {
            EditorApplication.update -= RunOnce;
            LoadSymbol();
        }

        static void LoadSymbol()
        {
            QUtils.AddScriptingDefineSymbol(DUI.SYMBOL_DOOZYUI);
        }
    }
}
