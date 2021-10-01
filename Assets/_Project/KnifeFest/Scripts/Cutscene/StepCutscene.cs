using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace KnifeFest
{
    public class StepCutscene : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Material _materialBlink;
        [SerializeField] private WallCutscene _wall;

        private float _multiplier;

        public float Multiplier => _multiplier;

        public WallCutscene Wall { get => _wall; set => _wall = value; }


        private void Start()
        {
            _wall = GetComponentInChildren<WallCutscene>();
        }

        public void UpdatingTextsMultiplier()
        {
            _text.text = "x" + _multiplier;
        }

        public void ChangeIndexMultiprier(float indexPrevStep)
        {
            float index = indexPrevStep + 0.2f;
            string str = index.ToString("0.0");
            _multiplier = float.Parse(str);
        }

        public void SetEndStep()
        {
            if (_wall == null)
                _wall = GetComponentInChildren<WallCutscene>();
            _wall.SetEndWall();
        }

        public void FadeTextObject()
        {
            _text.DOFade(0, 0.3f);
        }
    }
}