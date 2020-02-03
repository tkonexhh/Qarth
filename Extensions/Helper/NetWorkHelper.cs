using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class NetWorkHelper
    {
        /// <summary>
        /// 是否有网络
        /// </summary>
        /// <returns></returns>
        public static bool IsNotNetwork()
        {
            return Application.internetReachability == NetworkReachability.NotReachable;
        }


        public static bool IsWifiNetwork()
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }

        public static bool IsReachableViaCarrierDataNetwork()
        {
            return Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork;
        }
    }

}