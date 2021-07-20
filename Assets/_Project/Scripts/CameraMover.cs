using UnityEngine;
using DG.Tweening;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform[] _movementPoints;
    
    private Transform _transform;

    public int CurrentPointIndex { get; private set; } = 0;
    public Camera Camera { get; private set; }

    private void Awake()
    {
        Camera = GetComponent<Camera>();
        _transform = GetComponent<Transform>();
    }

    public void MoveCameraToPoint(int pointIndex)
    {
        if (pointIndex < 0 || pointIndex >= _movementPoints.Length || CurrentPointIndex == pointIndex) return;

        CurrentPointIndex = pointIndex;
        _transform.DOMove(_movementPoints[pointIndex].position, 1f);
        _transform.DORotate(_movementPoints[pointIndex].eulerAngles, 1f);
    }
}
