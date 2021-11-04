using UnityEngine;

public class ItemPreview : MonoBehaviour
{
    [SerializeField] private Transform _container;
    
    private GameObject _item;
    
    public void SpawnSelectedItem(GameObject itemTemplate)
    {
        Destroy(_item);
        _item = Instantiate(itemTemplate, _container);
        _item.transform.localPosition = Vector3.zero;
    }
}
