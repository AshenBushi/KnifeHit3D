using UnityEngine;
using DG.Tweening;

namespace KnifeFest
{
    public class WallCutscene : MonoBehaviour
    {
        [SerializeField] private int _multiplierWeight = 15;

        private Material _material;

        public Material Material { get => _material; set => _material = value; }
        public int MultiplierWeight => _multiplierWeight;

        private void Awake()
        {
            _material = GetComponent<MeshRenderer>().material;
        }

        public void ChangeColor()
        {
            _material.DOColor(
                Color.Lerp(
                    new Color(Random.Range(0, 0.4f), Random.Range(0, 0.4f), Random.Range(0, 0.4f), 0.7f),
                    new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1), 0.7f),
                    0.375f),
                1f).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
