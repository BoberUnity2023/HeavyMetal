using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameController Game
    {
        get
        {
            if (_game == null)
                _game = FindObjectOfType<GameController>();

            if (_game == null)//Editor            
                CreateGameController();            

            return _game;
        }

        set
        {
            _game = value;
        }
    }

    [SerializeField] private GameController _gameControllerPrefab;    

    protected GameController _game;

    private void OnEnable()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor) 
        {
            TryCreateGameController();
        }
    }
    private void TryCreateGameController()
    {
        GameController gameController = FindObjectOfType<GameController>();
        if (gameController == null)
            CreateGameController();
    }

    private void CreateGameController()
    {
        //Debug.LogWarning("Created GameController");
        _game = Instantiate(_gameControllerPrefab);
    }
}
