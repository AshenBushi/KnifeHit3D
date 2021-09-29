using UnityEngine;
using UnityEngine.EventSystems;

namespace KnifeFest
{
    public class CursorTracker : Singleton<CursorTracker>, IPointerDownHandler, IDragHandler
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

        public void DisableCanvas()
        {
            transform.parent.gameObject.SetActive(false);
        }

        public void EnableCanvas()
        {
            transform.parent.gameObject.SetActive(true);
        }
    }
}