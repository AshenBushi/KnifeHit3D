#pragma warning disable 0649

using UnityEngine;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    private bool isOpened = false;
    public bool IsOpened
    {
        get { return isOpened; }
    }

    public void SetState(bool state)
    {
        isOpened = state;
        panel.SetActive(state);
    }
}
