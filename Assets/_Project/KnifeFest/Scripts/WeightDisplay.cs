using TMPro;
using UnityEngine;

namespace KnifeFest
{
    public class WeightDisplay : MonoBehaviour
    {
        [SerializeField] private Knife _knife;
        [SerializeField] private TMP_Text _text;

        private void OnEnable()
        {
            _knife.OnWeightChanged += OnWeightChanged;
        }

        private void OnDisable()
        {
            _knife.OnWeightChanged -= OnWeightChanged;
        }

        private void OnWeightChanged()
        {
            _text.text = _knife.KnifeWeight.ToString();
        }
    }
}