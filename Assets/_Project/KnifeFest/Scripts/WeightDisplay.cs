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
            _knife.OnWeightDisable += OnWeightDisable;
        }

        private void OnDisable()
        {
            _knife.OnWeightChanged -= OnWeightChanged;
            _knife.OnWeightDisable -= OnWeightDisable;
        }

        private void OnWeightChanged()
        {
            _text.text = _knife.KnifeWeight.ToString();
        }

        private void OnWeightDisable()
        {
            gameObject.SetActive(false);
        }
    }
}