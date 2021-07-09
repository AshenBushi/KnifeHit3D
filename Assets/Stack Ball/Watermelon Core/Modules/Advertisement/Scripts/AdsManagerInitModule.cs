using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    public class AdsManagerInitModule : InitModule
    {
        public AdsData settings;
        public GameObject dummyCanvasPrefab;

        public AdsManagerInitModule()
        {
            moduleName = "Ads Manager";
        }

        public override void CreateComponent(Initialiser Initialiser)
        {
            AdsManager adsManager = new AdsManager();
            adsManager.Init(this);
        }
    }
}

// -----------------
// Advertisement v 0.3
// -----------------