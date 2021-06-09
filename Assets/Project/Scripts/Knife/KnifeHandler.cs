using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Scripts.Handlers
{
    public class KnifeHandler : MonoBehaviour
    {
        [SerializeField] private TargetHandler _targetHandler;
        [SerializeField] private HitScoreDisplayer _hitScoreDisplayer;
        [SerializeField] private InputField _inputField;
        [SerializeField] private KnifeSpawner _knifeSpawner;

        private Knife _currentKnife;
        private List<Knife> _thrownKnives = new List<Knife>();
        private int _knifeAmount;
        private bool _isSecondLifeUsed = false;
        private bool _canThrowKnife = true;

        public event UnityAction IsLevelFailed;

        public Knife CurrentKnifeTemplate => KnifeStorage.Knives[DataManager.GameData.ShopData.CurrentKnifeIndex];

        private void OnEnable()
        {
            _inputField.IsTapped += OnTapped;
            KnifeStorage.IsKnifeChanged += RespawnKnife;
        }

        private void OnDisable()
        {
            _inputField.IsTapped -= OnTapped;
            KnifeStorage.IsKnifeChanged -= RespawnKnife;

            if (_currentKnife == null) return;
            _currentKnife.IsStuck -= OnKnifeStuck;
            _currentKnife.IsBounced -= OnKnifeBounced;
        }

        private void Start()
        {
            InitKnife();
        }

        private void OnTapped()
        {
            if (_knifeAmount == 0 || !_canThrowKnife) return;
            
            if (_currentKnife == null)
            {
                InitKnife();
            }
            
            _currentKnife.Throw();
            _knifeAmount--;
            _thrownKnives.Add(_currentKnife);
            _currentKnife = null;
        }
        
        private void OnKnifeBounced()
        {
            InitKnife();
            
            if (DataManager.GameData.PlayerData.SecondLife > 0 && !_isSecondLifeUsed)
            {
                DataManager.GameData.PlayerData.SecondLife--;
                DataManager.Save();
                _isSecondLifeUsed = true;
                _knifeAmount++;
            }
            else
            {
                IsLevelFailed?.Invoke();
            }
        }
        
        private void OnKnifeStuck()
        {
            InitKnife();
            _hitScoreDisplayer.SubmitHit();
        }

        private void InitKnife()
        {
            if (_currentKnife != null) return;
            _currentKnife = _knifeSpawner.SpawnKnife(CurrentKnifeTemplate);
            _currentKnife.IsStuck += OnKnifeStuck;
            _currentKnife.IsBounced += OnKnifeBounced;
        }

        public void SetKnifeAmount(int knifeAmount)
        {
            _knifeAmount = knifeAmount;
        }

        private void RespawnKnife()
        {
            Destroy(_currentKnife.gameObject);
            _currentKnife = _knifeSpawner.SpawnKnife(CurrentKnifeTemplate);
            _currentKnife.IsStuck += OnKnifeStuck;
            _currentKnife.IsBounced += OnKnifeBounced;
        }

        public void AllowThrow()
        {
            _canThrowKnife = true;
        }
        
        public void DisallowThrow()
        {
            _canThrowKnife = false;
        }

        public void SecondLife()
        {
            AllowThrow();
            SetKnifeAmount(_targetHandler.CurrentTarget.HitToBreak);
        }
    }
}