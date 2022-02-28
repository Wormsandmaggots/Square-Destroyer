using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenAdBanner : MonoBehaviour
{
    private bool debug = false;
    private void OnEnable()
    {
        if (debug)
        {
            BannerAd.ad.ShowBannerAd();
        }

        debug = true;
    }
}
