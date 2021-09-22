using UnityEngine;

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
    }
}
