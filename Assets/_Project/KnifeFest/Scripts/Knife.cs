using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

namespace KnifeFest
{
    public class Knife : MonoBehaviour
    {
        private GameObject _knife;

        private void OnEnable()
        {
            KnifeStorage.Instance.IsKnifeChanged += SpawnKnife;
        }

        private void OnDisable()
        {
            KnifeStorage.Instance.IsKnifeChanged -= SpawnKnife;
        }

        private void Start()
        {
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            yield return new WaitForSeconds(0f);
            SpawnKnife();
        }
        
        private void SpawnKnife()
        {
            if (_knife != null)
            {
                Destroy(_knife);
            }

            _knife = Instantiate(KnifeStorage.Instance.GetSimpleKnife(), transform.position, Quaternion.Euler(90, 0, 0), transform);
            _knife.transform.localScale = Vector3.one;
        }
    }
}
