using UnityEngine;

public class KnifeSpawner : MonoBehaviour
{
    public Knife SpawnKnife(Knife template)
    {
        return Instantiate(template, transform);
    }
}
