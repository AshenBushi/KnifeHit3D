using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace KnifeFest
{
    public class StepCutscene : MonoBehaviour
    {
        [SerializeField] private WallCutscene _wall;
        [SerializeField] private TMP_Text _textLeft;
        [SerializeField] private TMP_Text _textRight;
        [SerializeField] private Material _material;
        [SerializeField] private Material _materialBlink;

        private int _index;
        private int _indexTwo;

        public WallCutscene Wall { get => _wall; set => _wall = value; }
        public TMP_Text TextLeft { get => _textLeft; set => _textLeft = value; }
        public TMP_Text TextRight { get => _textRight; set => _textRight = value; }
        public int Index { get => _index; set => _index = value; }
        public int IndexTwo { get => _indexTwo; set => _indexTwo = value; }

        public void ChangeColor(WallCutscene wall)
        {
            StartCoroutine(ChangeColorRoutine(wall));
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