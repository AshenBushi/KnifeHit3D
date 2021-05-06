using UnityEngine;

public class MetricaManager : MonoBehaviour
{
    private static IYandexAppMetrica _metrica;
    
    private void Awake()
    {
        _metrica = AppMetrica.Instance;
    }

    public static void SendEvent(string eventName)
    {
        _metrica.ReportEvent(eventName);
        _metrica.SendEventsBuffer();
    }
}
