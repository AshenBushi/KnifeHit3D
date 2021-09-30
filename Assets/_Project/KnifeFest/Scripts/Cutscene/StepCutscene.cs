using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace KnifeFest
{
    public class StepCutscene : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textLeft;
        [SerializeField] private TMP_Text _textRight;
        [SerializeField] private Material _material;
        [SerializeField] private Material _materialBlink;
        [SerializeField] private Gradient _gradientColors;
        private WallCutscene _wall;

        private float _multiplier;

        public float Multiplier => _multiplier;

        public WallCutscene Wall { get => _wall; set => _wall = value; }


        private void Start()
        {
            _wall = GetComponentInChildren<WallCutscene>();
        }

        public void ChangeColor(WallCutscene wall)
        {
            StartCoroutine(ChangeColorRoutine(wall));
        }

        public void UpdatingTextsMultiplier()
        {
            _textLeft.text = "x" + _multiplier;
            _textRight.text = "x" + _multiplier;
        }

        public void ChangeIndexMultiprier(float indexPrevStep)
        {
            float index = indexPrevStep + 0.2f;
            string str = index.ToString("0.0");
            _multiplier = float.Parse(str);
        }

        public void ChangeWallMultiplier(int multiplier)
        {
            if (_wall == null)
                _wall = GetComponentInChildren<WallCutscene>();
            _wall.ChangeMultiplier(multiplier);
        }

        private IEnumerator ChangeColorRoutine(WallCutscene wall)
        {
            var baseMat = _material;
            var baseMatWall = wall.Material;
            GetComponent<MeshRenderer>().material.DOColor(_materialBlink.color, 0.2f).SetAutoKill(false);
            wall.GetComponent<MeshRenderer>().material.DOColor(_materialBlink.color, 0.2f).SetAutoKill(false);
            yield return new WaitForSeconds(0.1f);
            GetComponent<MeshRenderer>().material.DOColor(baseMat.color, 0.2f).Restart();
            wall.GetComponent<MeshRenderer>().material.DOColor(baseMatWall.color, 0.2f).Restart();
        }
    }
}