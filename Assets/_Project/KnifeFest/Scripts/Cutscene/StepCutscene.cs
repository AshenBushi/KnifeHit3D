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

        private int k = 0;

        private float _indexMultiplier;

        public float IndexMultiplier => _indexMultiplier;

        public void ChangeColor(WallCutscene wall)
        {
            StartCoroutine(ChangeColorRoutine(wall));
        }

        public void UpdatingTextsMultiplier()
        {
            _textLeft.text = "x" + _indexMultiplier;
            _textRight.text = "x" + _indexMultiplier;
        }

        public void ChangeIndexMultiprier(float indexPrevStep)
        {
            float index = indexPrevStep + 0.2f;
            string str = index.ToString("0.0");
            _indexMultiplier = float.Parse(str);
        }

        private IEnumerator ChangeColorRoutine(WallCutscene wall)
        {
            var baseMat = _material;
            var baseMatWall = wall.Material;
            GetComponent<MeshRenderer>().material.DOColor(_materialBlink.color, 0.5f).SetAutoKill(false);
            wall.GetComponent<MeshRenderer>().material.DOColor(_materialBlink.color, 0.5f).SetAutoKill(false);
            yield return new WaitForSeconds(0.2f);
            GetComponent<MeshRenderer>().material.DOColor(baseMat.color, 0.5f).Restart();
            wall.GetComponent<MeshRenderer>().material.DOColor(baseMatWall.color, 0.5f).Restart();
        }
    }
}