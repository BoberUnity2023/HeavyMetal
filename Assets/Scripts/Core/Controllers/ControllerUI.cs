using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerUI : MonoBehaviour
{
    [SerializeField] private GameController _game;
    [SerializeField] private bool _hasSelected;
    private int _sceneBuildIndex;
    private bool _waitingRleaseButtosAftrFinish;
    

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
        Debug.Log("NavigationStart");
        _hasSelected = true;
        OnNavigationStart?.Invoke();
    }

    public void NavigationEnd()
    {
        Debug.Log("NavigationEnd");
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

        bool anyKey = AnyKey;
        if (anyKey && !_hasSelected && !_waitingRleaseButtosAftrFinish)
        {
            NavigationStart();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.JoystickButton0)
            )
            NavigationEnd();

        if (_waitingRleaseButtosAftrFinish && !anyKey)
        {
            _waitingRleaseButtosAftrFinish = false;
            NavigationEnd(); 
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        NavigationEnd();
        _sceneBuildIndex = scene.buildIndex;
    }

    private bool AnyKey
    {
        get
        {
            return 
            Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Mathf.Abs(Input.GetAxis("Vertical")) > 0.01f;
        }
    }

    public void Finish()
    {
        NavigationEnd();
        if (AnyKey)
        {
            _waitingRleaseButtosAftrFinish = true;
        }
    }
}
