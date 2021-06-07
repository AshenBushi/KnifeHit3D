using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Gift : MonoBehaviour
{
    [SerializeField] private GameObject _slicedGift;
    [SerializeField] private GameObject _sliceEffect;

    private List<GiftPiece> _pieces;
    
    public event UnityAction IsSliced;

    private void Awake()
    {
        _pieces = GetComponentsInChildren<GiftPiece>().ToList();
    }

    private void OnEnable()
    {
        foreach (var piece in _pieces)
        {
            piece.IsSliced += OnPieceSliced;
        }
    }
    
    private void OnDisable()
    {
        foreach (var piece in _pieces)
        {
            piece.IsSliced -= OnPieceSliced;
        }
    }

    private void Update()
    {
        var position = transform.position;
        transform.LookAt(new Vector3(position.x, position.y, position.z + 1f));
    }
    
    private void OnPieceSliced()
    {
        StartCoroutine(SlicedAnimation());
    }
    
    private IEnumerator SlicedAnimation()
    {
        var slicedGift = Instantiate(_slicedGift, transform.position, transform.rotation);

        Instantiate(_sliceEffect, transform.position, Quaternion.identity);
        
        IsSliced?.Invoke();
        
        Destroy(gameObject);

        yield return new WaitForSeconds(2f);
        
        Destroy(slicedGift);
    }
}
