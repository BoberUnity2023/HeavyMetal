using UnityEngine;
using UnityEngine.UI;

public class MainMenu : WindowBase
{
    [SerializeField] private Garage _garage;
    [SerializeField] private Button _buttonNewGame;
    [SerializeField] private Button _buttonContinue;

    protected override void Start()
    {
        base.Start();
        bool gameStarted = IsGameStarted;        
        _buttonContinue.interactable = gameStarted;
    }

    protected override void SelectFirst(GameObject firstSelected)
    {
        Button button = IsGameStarted ? _buttonContinue : _buttonNewGame;
        base.SelectFirst(button.gameObject);
    }

    public void OnPressNewGame()
    {
        Debug.Log("Press New Game");
        Game.Sound.Play(SoundClip.Click);
        Game.UI.NavigationEnd();
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("GameStarted", 1);        
        OnPressContinue();
    }

    public void OnPressContinue()
    {
        Debug.Log("Press Continue");
        Game.Sound.Play(SoundClip.Click);
        Game.UI.NavigationEnd();
        Hide();
        _garage.PressGame();
    }

    public void OnPressSettings()
    {
        Debug.Log("Press Settings");
        Game.Sound.Play(SoundClip.Click);
        Game.UI.NavigationEnd();
        Hide();
        _garage.WindowSettings.Show();
    }

    public void OnPressAboutGame()
    {
        Debug.Log("Press About Game");
        Game.Sound.Play(SoundClip.Click);
        Game.UI.NavigationEnd();
        Hide();
        _garage.WindowAboutGame.Show();
    }

    public void OnPressQuitGame()
    {
        Debug.Log("Press Quit");
        Game.Sound.Play(SoundClip.Click);
        Application.Quit();
    }

    private bool IsGameStarted => PlayerPrefs.GetInt("GameStarted", 0) == 1;
}
