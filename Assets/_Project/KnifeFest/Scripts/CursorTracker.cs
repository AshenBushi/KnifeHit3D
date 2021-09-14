using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KnifeFest
{
    public class CursorTracker : MonoBehaviour, IDragHandler
    {
        public float XDelta { get; private set; }

        public void OnDrag(PointerEventData eventData)
        {
            XDelta = eventData.delta.x;
        }
    }
}