using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class TargetMover : MonoBehaviour
{
    private const float SpawnStep = 25;
    private  const float AnimationDuration = 1;
    
    [SerializeField] private List<Vector3> _cubeRotatePoints;
    
    private Tween _mover;

    public event UnityAction IsTargetChanged;

    public void MoveTargets(List<Target> targets)
    {
        foreach (var target in targets)
        {
            var position = target.transform.position;
            _mover = target.transform.DOMove(new Vector3(position.x, position.y, position.z - SpawnStep), AnimationDuration).SetLink(gameObject);
        }

        _mover.OnComplete(() =>
        {
            IsTargetChanged?.Invoke();
        });
    }

    public void RotateCube(TargetBase cubeBase, int edgeNumber)
    {
        _mover = cubeBase.transform.DOLocalRotate(_cubeRotatePoints[edgeNumber], AnimationDuration).SetLink(gameObject);
        
        _mover.OnComplete(() =>
        {
            IsTargetChanged?.Invoke();
        });
    }
}
