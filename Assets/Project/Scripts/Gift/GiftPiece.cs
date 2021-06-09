using System;
using UnityEngine;
using UnityEngine.Events;

public class GiftPiece : MonoBehaviour
{
    public event UnityAction IsSliced;
    
    public void Slice()
    {
        SoundManager.PlaySound(SoundNames.GiftHit);
        IsSliced?.Invoke();
    }
}
