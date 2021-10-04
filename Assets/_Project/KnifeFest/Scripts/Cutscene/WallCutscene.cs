using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

namespace KnifeFest
{
    public class WallCutscene : MonoBehaviour
    {
        [SerializeField] private int _multiplierWeight = 15;

        private Material _material;

        public Material Material { get => _material; set => _material = value; }
        public int MultiplierWeight => _multiplierWeight;
        public bool IsEndWall { get; private set; } = false;

        private void Awake()
        {
            _material = GetComponent<MeshRenderer>().material;
        }

        public void ChangeColor(bool isEndWall)
        {
            if (isEndWall) return;

            GetComponent<MeshRenderer>().material.DOColor(Random.ColorHSV(0, 1, 0, 1, 0, 1, 0.3f, 0.7f), 0.5f);
        }

        public void ChangeScale(float valueX)
        {
            transform.localScale = new Vector3(transform.localScale.x + valueX, transform.localScale.y, transform.localScale.z);
        }

        public void SetEndWall()
        {
            IsEndWall = true;
        }

        public void Detonate()
        {
            var childCollider = transform.parent.GetComponentsInChildren<Collider>();
            var childRigidbody = transform.parent.GetComponentsInChildren<Rigidbody>();

            foreach (var item in childCollider)
            {
                item.isTrigger = false;
            }

            foreach (var item in childRigidbody)
            {
                item.isKinematic = false;
                item.AddExplosionForce(5000f, new Vector3(0, 150f, 150f), 150f, 0f, ForceMode.Acceleration);
            }

            StartCoroutine(SelfDestruction());
        }

        public IEnumerator SelfDestruction()
        {
            gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.2f);
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
    }
}
