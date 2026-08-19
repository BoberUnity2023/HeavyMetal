using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerUI : MonoBehaviour
{
    [SerializeField] private GameController _game;
    private bool _hasSelected;
    private int _sceneBuildIndex;

    public event Action OnNavigationStart;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void NavigationStart()
    {
        _hasSelected = true;
        OnNavigationStart?.Invoke();
    }

    public void NavigationEnd()
    {
        _hasSelected = false;
    }

    private void Update()
    {
        if (_sceneBuildIndex >= 3 && 
            Time.timeScale == 1 &&
            !_game.Hub.Race.IsFinished)
        {
            return;
        }

        bool anyKey = Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Mathf.Abs(Input.GetAxis("Vertical")) > 0.01f;

        if (anyKey && !_hasSelected)
        {
            NavigationStart();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.JoystickButton0)
            )
            NavigationEnd();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        NavigationEnd();
        _sceneBuildIndex = scene.buildIndex;
    }
}
