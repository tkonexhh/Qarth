using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_2018
using UnityEngine.Networking;
#endif

namespace Qarth
{
#if UNITY_2018
    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            //Simply return true no matter what
            return true;
        }
    }
#endif
}