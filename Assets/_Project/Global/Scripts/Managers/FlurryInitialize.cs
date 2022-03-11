using FlurrySDK;
using UnityEngine;

public class FlurryInitialize : MonoBehaviour
{
#if UNITY_ANDROID
    private readonly string FLURRY_API_KEY = "SZ9NTM9JF9S4TD2SFQ8Q";
#elif UNITY_IPHONE
    private readonly string FLURRY_API_KEY = "";
#else
    private readonly string FLURRY_API_KEY = null;
#endif

    void Start()
    {
        // Note: When enabling Messaging, Flurry Android should be initialized by using AndroidManifest.xml.
        // Initialize Flurry once.
        new Flurry.Builder()
                  .WithCrashReporting(true)
                  .WithLogEnabled(true)
                  .WithLogLevel(Flurry.LogLevel.VERBOSE)
                  .WithMessaging(true)
                  .WithPerformanceMetrics(Flurry.Performance.ALL)
                  .Build(FLURRY_API_KEY);
    }
}
