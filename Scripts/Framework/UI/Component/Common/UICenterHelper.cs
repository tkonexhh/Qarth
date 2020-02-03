using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public class UICenterHelper
    {

        public static void CenterImageWithText(Image img, Text text, float offset)
        {
            float texWidth = text.preferredWidth;
            float imgWidth = img.preferredWidth;
            float totalWidth = texWidth + imgWidth + offset;

            float start = -totalWidth * 0.5f;
            Vector3 imgPos = img.transform.localPosition;
            imgPos.x = start + imgWidth * 0.5f;
            img.transform.localPosition = imgPos;

            Vector3 texPos = text.transform.localPosition;
            texPos.x = start + imgWidth + offset;// + texWidth;// * 0.5f;

            text.transform.localPosition = texPos;
        }
    }
}
