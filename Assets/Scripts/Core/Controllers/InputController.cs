using UnityEngine;
using UnityEngine.SceneManagement;


public class InputController : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private PlayerInput _playerInput;

    public PlayerInput PlayerInput => _playerInput;

    private void Start()
    {
        
    }

    private void Update()
    {
        Update_TryJump();        
    }

    private void Update_TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {            
            Jump();
            PlayerPrefs.SetInt("Jumped", 1);
        }
    }

    public void LoadScene(int buildIndex)
    {
        _hub.SceneLoader.LoadScene(buildIndex);
    }

    public void LoadLevel(int level)
    {
        int buildIndex = level + 1;
        LoadScene(buildIndex);
    }

    public void LoadLevelNext()
    {
        //_hub.Game.CurrentLevel++;
        //int buildIndex = _hub.Game.Levels.Level(_hub.Game.CurrentLevel).SceneBuildIndex;//SceneManager.GetActiveScene().buildIndex + 1;
        //LoadScene(buildIndex);
        LoadScene(1);
    }

    public void LoadLevelRestart()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(buildIndex);
    }

    public void Jump()
    {
        //_hub.Hero.Move.TryJump();
    }

    public void RestartFromCheckpoint()
    {        
        
    }
}
