using UnityEngine;

public class ShopScreen : MonoBehaviour
{
    public void EnableScreen()
    {
        gameObject.SetActive(true);
    }

    public void DisableScreen()
    {
        gameObject.SetActive(false);
    }
}
