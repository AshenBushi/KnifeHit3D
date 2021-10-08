using TMPro;
using UnityEngine;

public class Page : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textNumber;
    private int _knifeMod;
    private int _gameMod;

    public int KnifeMod => _knifeMod;
    public int GameMod => _gameMod;

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

    public void SetKnifeMod(int knifeMod)
    {
        _knifeMod = knifeMod;
    }

    public void SetGameMod(int gameMod)
    {
        _gameMod = gameMod;
    }
}
