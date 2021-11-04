using UnityEngine;

public class StackKnife : MonoBehaviour
{
    private GameObject _knife;
    
    private void OnEnable()
    {
        KnifeStorage.Instance.IsKnifeChanged += OnKnifeChanged;
    }

    private void OnDisable()
    {
        KnifeStorage.Instance.IsKnifeChanged -= OnKnifeChanged;
    }

    private void Start()
    {
        SpawnKnife();
    }

    private void OnKnifeChanged()
    {
        SpawnKnife();
    }

    private void SpawnKnife()
    {
        if (_knife != null)
        {
            Destroy(_knife.gameObject);
        }

        _knife = Instantiate(
            KnifeStorage.Instance.StackKnives[DataManager.Instance.GameData.ShopData.CurrentKnifeIndex], transform);
    }
}
