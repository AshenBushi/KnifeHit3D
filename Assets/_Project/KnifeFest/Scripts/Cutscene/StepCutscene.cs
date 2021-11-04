using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace KnifeFest
{
    public class StepCutscene : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Material _materialBlink;
        [SerializeField] private WallCutscene _wall;

        private GameObject _mark;
        private List<MeshRenderer> _markParts = new List<MeshRenderer>();
        private List<Collider> _childCollider = new List<Collider>();
        private List<Rigidbody> _childRigidbody = new List<Rigidbody>();
        private float _multiplier;

        public float Multiplier => _multiplier;


        public void Init()
        {
            _wall = GetComponentInChildren<WallCutscene>();
            _mark = GetComponentInChildren<TargetBase>().gameObject;
            _childCollider = _mark.GetComponentsInChildren<Collider>().ToList();
            _childRigidbody = _mark.GetComponentsInChildren<Rigidbody>().ToList();

            _markParts = _mark.GetComponentsInChildren<MeshRenderer>().ToList();
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

        public void ChangeColorMark(int index, int maxCount)
        {
            for (int i = 0; i < _markParts.Count; i++)
            {
                _markParts[i].material.color = Color.Lerp(ColorManager.Instance.CurrentColorPreset.endColor, ColorManager.Instance.CurrentColorPreset.startColor, (float)index / maxCount);
            }
        }

        public void SetEndStep()
        {
            _wall.SetEndWall();
        }

        public void FadeTextObject()
        {
            _text.DOFade(0, 0.3f).SetLink(gameObject);
        }

        public void Detonate()
        {
            foreach (var item in _childCollider)
            {
                item.isTrigger = false;
            }

            foreach (var item in _childRigidbody)
            {
                item.isKinematic = false;
                item.useGravity = false;
                item.AddExplosionForce(16f, new Vector3(0f, 0f, item.gameObject.transform.position.z), 0f, 0f, ForceMode.Impulse);
                Destroy(item.gameObject, 4f);
            }

            StartCoroutine(SelfDestruction());
        }

        private IEnumerator SelfDestruction()
        {
            _wall.transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetLink(gameObject);
            yield return new WaitForSeconds(3f);
            Destroy(_wall);
        }
    }
}