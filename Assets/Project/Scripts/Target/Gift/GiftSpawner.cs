using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GiftSpawner : MonoBehaviour
{
    private Gift _gift;
    
    public event UnityAction IsGiftSliced;
    
    private void OnDisable()
    {
        if (_gift == null) return;
        _gift.IsSliced -= OnGiftSliced;
    }

    private void OnGiftSliced()
    {
        IsGiftSliced?.Invoke();
    }
    
    public void SpawnGift(Gift giftTemplate)
    {
        if (_gift != null)
        {
            _gift.IsSliced -= OnGiftSliced;
            Destroy(_gift.gameObject);
        }

        _gift = Instantiate(giftTemplate, transform);
        _gift.IsSliced += OnGiftSliced;
    }
}
