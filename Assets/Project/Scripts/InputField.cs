using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputField : MonoBehaviour, IPointerDownHandler
{
    public event UnityAction OnPlayerTap;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPlayerTap?.Invoke();
    }
}
