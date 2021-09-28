using UnityEngine;
using UnityEngine.EventSystems;

namespace KnifeFest
{
    public class CursorTracker : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        public float XDelta { get; set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            PlayerInput.Instance.OnPointerDown(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            XDelta = eventData.delta.x;
        }
    }
}