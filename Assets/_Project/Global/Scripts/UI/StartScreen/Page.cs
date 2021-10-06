using TMPro;
using UnityEngine;

public class Page : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textNumber;

    public void SetTextNum(int index)
    {
        _textNumber.text = (index + 1).ToString();
    }

    public void Activation()
    {
        gameObject.SetActive(true);
    }

    public void Deactivation()
    {
        gameObject.SetActive(false);
    }
}
