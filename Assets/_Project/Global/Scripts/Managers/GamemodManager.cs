using UnityEngine;
using Random = UnityEngine.Random;

public enum Gamemod
{
    KnifeHit,
    StackKnife,
    KnifeFest
}

public class GamemodManager : Singleton<GamemodManager>
{
    [SerializeField] private Skills _skills;
    [SerializeField] private HandlePages _handlerPages;
    [SerializeField] private int _knifeHitModsCount = 5;
    [SerializeField] private int _gameModCount = 2;

    public int KnifeHitMod { get; private set; }
    public Gamemod CurrentMod { get; private set; }
    public int KnifeHitModsCount => _knifeHitModsCount;
    public int GameModCount => _gameModCount;

    private void SelectRandomMod()
    {
        var randomGamemod = Random.Range(0, 3);
        var randomKnifeHitMod = Random.Range(0, 6);

        SelectKnifeHitMod(randomKnifeHitMod);
        SelectMod(randomGamemod);
    }

    public void OnClick()
    {
        SelectKnifeHitMod(_handlerPages.MenuPages[_handlerPages.CurrentPage].KnifeMod);
        SelectMod(_handlerPages.MenuPages[_handlerPages.CurrentPage].GameMod);
    }

    public void ControlSession(bool firstTime)
    {
        if (firstTime)
        {
            SelectRandomMod();
        }
        else
        {
            SelectMod((int)DataManager.Instance.GameData.CurrentGamemod);
        }
    }

    public void EndSession()
    {
        SceneLoader.Instance.LoadPreparedScene();
    }

    public void SelectMod(int index)
    {
        PlayerInput.Instance.AllowUsing();

        CurrentMod = (Gamemod)index;

        _skills.DisallowSkills();

        switch (CurrentMod)
        {
            case Gamemod.KnifeHit:
                if (KnifeHitMod == 6)
                {
                    _skills.DisallowSkills();
                }
                else
                {
                    _skills.AllowSkills();
                }
                SceneLoader.Instance.LoadGamemodScene(0);
                break;
            case Gamemod.StackKnife:
                SceneLoader.Instance.LoadGamemodScene(1);
                break;
            case Gamemod.KnifeFest:
                SceneLoader.Instance.LoadGamemodScene(2);
                break;
        }

        DataManager.Instance.GameData.CurrentGamemod = CurrentMod;
    }

    public void SelectKnifeHitMod(int index)
    {
        KnifeHitMod = index;
    }
}
