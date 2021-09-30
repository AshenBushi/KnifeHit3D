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
            GetComponent<MeshRenderer>().material.DOColor(Random.ColorHSV(0, 1, 0, 1, 0, 1, 0, 0.5f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        }

        public void ChangeMultiplier(int value)
        {
            _multiplierWeight = value;
        }

        public void ChangeScale(float valueX)
        {
            transform.localScale = new Vector3(transform.localScale.x + valueX, transform.localScale.y, transform.localScale.z);
        }
    }
}
