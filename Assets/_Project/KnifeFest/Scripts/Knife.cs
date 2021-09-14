using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace KnifeFest
{
    public class Knife : MonoBehaviour
    {
        [SerializeField] private CursorTracker _cursorTracker;
        [SerializeField] private Vector3 _defaultScale;
        [SerializeField] private float _weightChangeSpeed;
        
        private GameObject _knife;
        private float _xDelta;
        
        public int KnifeWeight { get; private set; } = 10;

        public event UnityAction OnWeightChanged;

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

        private void Update()
        {
            if(_cursorTracker.XDelta != 0)
                SetKnifePosition();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Wall wall)) return;
            
            KnifeWeight = wall.ChangeValue(KnifeWeight);
            OnWeightChanged?.Invoke();

            StartCoroutine(ChangeKnifeScale());
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
            _knife.transform.localScale = _defaultScale;
            OnWeightChanged?.Invoke();
        }

        private IEnumerator ChangeKnifeScale()
        {
            Vector3 targetScale;
            
            if (1 + KnifeWeight / 100 > 2)
                targetScale = _defaultScale * 2;
            else
                targetScale = _defaultScale * (1 + (float)KnifeWeight / 100);

            while (_knife.transform.localScale != targetScale)
            {
                _knife.transform.localScale = Vector3.MoveTowards(_knife.transform.localScale, targetScale, Time.deltaTime * _weightChangeSpeed);
                
                yield return null;
            }
        }
        
        private void SetKnifePosition()
        {
            if(_xDelta - _cursorTracker.XDelta == 0) return;
            
            _xDelta = _cursorTracker.XDelta;
            
            var knifePosition = transform.position;
            
            if(Math.Abs(knifePosition.x + _cursorTracker.XDelta / 100f) >= 1.5f) return;

            transform.position = new Vector3(knifePosition.x + _cursorTracker.XDelta / 100f, knifePosition.y,
                knifePosition.z);
        }
    }
}
