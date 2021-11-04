using DG.Tweening;
using UnityEngine;

public class DailyGiftArrows : MonoBehaviour
{
    private RectTransform _rectTransform;
    private float _yTarget;
    private bool _isFirstMoving = true;

    public void SetPosition(float yTarget)
    {
        _rectTransform = GetComponent<RectTransform>();
        _yTarget = yTarget;

        if (!_isFirstMoving)
        {
            _rectTransform.anchoredPosition = new Vector3(_rectTransform.anchoredPosition.x, _yTarget);
            return;
        }

        DailyGiftScreen.IsScrollDisable?.Invoke();
        _rectTransform.DOAnchorPosY(yTarget, 0.8f).OnComplete(DailyGiftScreen.IsScrollEnable.Invoke).SetLink(gameObject);
    }

    public void AllowMove()
    {
        _isFirstMoving = true;
    }

    public void DisallowMove()
    {
        _isFirstMoving = false;
    }

    public void SetParent(Transform parent)
    {
        transform.parent = parent;
    }
}
