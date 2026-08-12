using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowBase : MonoBehaviour
{
    [SerializeField] protected SceneController _sceneController;
    [SerializeField] protected GameObject _window;
    [SerializeField] private GameObject _firstSelected;

    public GameController Game => _sceneController.Game;

    public virtual void Show()
    {
        _window.SetActive(true);
    }

    public virtual void Hide()
    {
        _window.SetActive(false);
    }

    public bool IsActive => _window.activeSelf;

    protected virtual void Start()
    {
        Game.UI.OnNavigationStart += UI_OnNavigationStart;
    }

    protected virtual void OnDestroy()
    {
        Game.UI.OnNavigationStart -= UI_OnNavigationStart;
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
        if (Time.timeScale < 0.1f)
            Time.timeScale = 0.2f;
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
