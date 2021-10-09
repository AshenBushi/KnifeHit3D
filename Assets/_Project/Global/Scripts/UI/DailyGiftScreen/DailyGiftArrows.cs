using DG.Tweening;
using UnityEngine;

public class DailyGiftArrows : MonoBehaviour
{
    private float _yTarget;
    private bool _isFirstMoving = true;

    //private void FixedUpdate()
    //{
    //    var yPos = Mathf.Lerp(transform.position.y, _yTarget, 5.5f * Time.fixedDeltaTime);
    //    transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    //}

    public void SetPosition(float yTarget)
    {
        if (!_isFirstMoving)
        {
            transform.position = new Vector3(transform.position.x, yTarget, transform.position.z);
            return;
        }

        transform.DOMoveY(yTarget, 0.7f).OnComplete(DisallowMove);
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
