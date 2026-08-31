using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowBase : MonoBehaviour
{
    [SerializeField] protected SceneController _sceneController;
    [SerializeField] protected GameObject _window;
    [SerializeField] protected GameObject _firstSelected;
    protected GameController _game;
    private bool _isInited;

    public GameController Game => _sceneController.Game;

    public virtual void Init(GameController game)
    {
        _isInited = true;
        _game = game;
        _game.UI.OnNavigationStart += UI_OnNavigationStart;
    }

    public virtual void Show()
    {
        _window.SetActive(true);
    }

    public virtual void Show(GameController game)
    {
        Init(game);
        Show();
    }

    public virtual void Hide()
    {
        _window.SetActive(false);
    }

    public bool IsActive => _window.activeSelf;

    protected virtual void Start()
    {
        
    }

    protected virtual void OnDestroy()
    {
        if (_isInited)
            _game.UI.OnNavigationStart -= UI_OnNavigationStart;
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
            SelectFirst(_firstSelected);
    }

    private void UI_OnNavigationStart()
    {
        if (!IsActive)
            return;
        
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(AfterUI_OnNavigationStart(0));        
    }

    private IEnumerator AfterUI_OnNavigationStart(float time)
    {
        yield return new WaitForSecondsRealtime(0);
        SelectFirst(_firstSelected);
    }

    protected virtual void SelectFirst(GameObject firstSelected)
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
