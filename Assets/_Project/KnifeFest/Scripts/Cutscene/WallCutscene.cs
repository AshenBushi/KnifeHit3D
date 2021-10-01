using UnityEngine;
using DG.Tweening;
using System.Collections;

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

        public void Detonate()
        {
            foreach (var item in transform.parent.GetComponentsInChildren<Collider>())
            {
                item.isTrigger = false;
            }

            foreach (var item in transform.parent.GetComponentsInChildren<Rigidbody>())
            {
                item.isKinematic = false;
                item.AddExplosionForce(1500f, new Vector3(0f, 3f, 10f), 20, 0);
            }

            StartCoroutine(SelfDestruction());
        }

        public void SetEndWall()
        {
            IsEndWall = true;
        }

        private IEnumerator SelfDestruction()
        {
            yield return new WaitForSeconds(0f);

            Destroy(gameObject);
        }
    }
}
