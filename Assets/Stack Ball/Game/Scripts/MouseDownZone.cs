using UnityEngine;
using UnityEngine.EventSystems;
using Watermelon;

public class MouseDownZone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerController playerController;
    public SettingsPanel settingsPanel;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(playerController.IsGameStarted)
        {
            playerController.MouseDown();
        }
        else
        {
            if (!settingsPanel.IsActive)
            {
                playerController.MouseDown();
            }
            else
            {
                settingsPanel.Hide(true);

                playerController.MouseDown();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (playerController.IsGameStarted)
        {
            playerController.MouseUp();
        }
    }
}
