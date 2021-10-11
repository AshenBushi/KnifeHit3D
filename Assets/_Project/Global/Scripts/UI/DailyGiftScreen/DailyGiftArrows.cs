using DG.Tweening;
using UnityEngine;

public class DailyGiftArrows : MonoBehaviour
{
    private float _yTarget;
    private bool _isFirstMoving = true;

    public void SetPosition(float yTarget)
    {
        _yTarget = yTarget;

        if (!_isFirstMoving)
        {
            transform.position = new Vector3(transform.position.x, _yTarget, transform.position.z);
            return;
        }

        transform.DOMoveY(yTarget, 0.7f).SetLink(gameObject);
    }

    public void AllowMove()
    {
        _isFirstMoving = true;
    }

    public void DisallowMove()
    {
        _isFirstMoving = false;
    }
}
