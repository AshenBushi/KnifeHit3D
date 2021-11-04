using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Page : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textNumber;
    [SerializeField] private Animator _animator;

    private Button _buttonContinue;
    private CanvasGroup _canvasGroup;
    private int _knifeMod;
    private int _gameMod;
    private int _currentMovie;

    public int KnifeMod => _knifeMod;
    public int GameMod => _gameMod;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _buttonContinue = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        _buttonContinue.onClick.AddListener(ButtonClick);

        _animator.SetBool(_currentMovie.ToString(), true);
    }

    private void OnDisable()
    {
        _buttonContinue.onClick.RemoveListener(ButtonClick);
    }

    public void InitMovie(int currentMovie)
    {
        _currentMovie = currentMovie;
        if (_animator.parameterCount < _currentMovie)
            _animator.SetBool(_currentMovie.ToString(), true);
        _animator.Play("knifehit" + _currentMovie);
    }

    public void Activation()
    {
        _animator.Play("knifehit" + _currentMovie);
        _canvasGroup.DOFade(1f, 0.3f).SetLink(gameObject);
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void Deactivation()
    {
        _canvasGroup.DOFade(0f, 0.3f).SetLink(gameObject);
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public void SetKnifeMod(int knifeMod)
    {
        _knifeMod = knifeMod;
        SetTextNumKnifeHit();
    }

    public void SetGameMod(int gameMod)
    {
        _gameMod = gameMod;

        switch (_gameMod)
        {
            case 1:
                SetTextNum(1);
                break;
            case 2:
                SetTextNum(2);
                break;
        }
    }

    private void ButtonClick()
    {
        if (PlayerPrefs.GetInt("first_level") == 1)
        {
            switch (_gameMod)
            {
                case 0:
                    var knifeMod = _knifeMod;
                    MetricaManager.SendEvent($"first_start_({knifeMod + 1})");
                    break;
                case 1:
                    MetricaManager.SendEvent("first_start_(7)");
                    break;
                case 2:
                    MetricaManager.SendEvent("first_start_(8)");
                    break;
            }

            PlayerPrefs.SetInt("first_level", 0);
        }

        PlayerInput.Instance.AllowUsing();
        PlayerInput.Instance.OnClick();
        GamemodManager.Instance.OnClick();
    }

    private void SetTextNum(int gameModIndex)
    {
        switch (gameModIndex)
        {
            case 1:
                _textNumber.text = (DataManager.Instance.GameData.ProgressData.CurrentStackKnifeLevel + 1).ToString();
                break;
            case 2:
                _textNumber.text = (DataManager.Instance.GameData.ProgressData.CurrentKnifeFestLevel + 1).ToString();
                break;
        }
    }

    private void SetTextNumKnifeHit()
    {
        _textNumber.text = (_knifeMod % 3) switch
        {
            0 => (DataManager.Instance.GameData.ProgressData.CurrentMarkLevel + 1).ToString(),
            1 => (DataManager.Instance.GameData.ProgressData.CurrentCubeLevel + 1).ToString(),
            2 => (DataManager.Instance.GameData.ProgressData.CurrentFlatLevel + 1).ToString(),
            _ => throw new System.NotImplementedException(),
        };
    }
}
