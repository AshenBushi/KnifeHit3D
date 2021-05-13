using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LotterySection : MonoBehaviour
{
    [SerializeField] private RewardNames _name;
    public bool KnifeStuck { get; private set; } = false;

    public event UnityAction<RewardNames> IsKnifeStuck;

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent(out Knife knife)) return;
        SoundManager.PlaySound(SoundNames.TargetHit);
        knife.Stuck(transform);
        IsKnifeStuck?.Invoke(_name);
        KnifeStuck = true;
    }
}
