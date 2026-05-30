using UnityEngine;
using UnityEngine.SceneManagement;


public class InputController : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private bool _btn0;
    [SerializeField] private bool _btn1;
    [SerializeField] private bool _btn2;
    [SerializeField] private bool _btn3;
    [SerializeField] private bool _btn4;
    [SerializeField] private bool _btn5;

    [SerializeField] private bool _btn6;
    [SerializeField] private bool _btn7;
    [SerializeField] private bool _btn8;
    [SerializeField] private bool _btn9;
    [SerializeField] private bool _btn10;

    [SerializeField] private bool _btn11;
    [SerializeField] private bool _btn12;
    [SerializeField] private bool _btn13;
    [SerializeField] private bool _btn14;
    [SerializeField] private bool _btn15;
    [SerializeField] private float _horizontal;
    [SerializeField] private float _vertical;
    [SerializeField] private float _fire1;
    [SerializeField] private float _fire2;


    public PlayerInput PlayerInput => _playerInput;

    private void Start()
    {
        
    }

    private void Update()
    {
        _btn0 = Input.GetKey(KeyCode.Joystick1Button0);
        _btn1 = Input.GetKey(KeyCode.Joystick1Button1);
        _btn2 = Input.GetKey(KeyCode.Joystick1Button2);
        _btn3 = Input.GetKey(KeyCode.Joystick1Button3);
        _btn4 = Input.GetKey(KeyCode.Joystick1Button4);
        _btn5 = Input.GetKey(KeyCode.Joystick1Button5);

        _btn6 = Input.GetKey(KeyCode.Joystick1Button6);
        _btn7 = Input.GetKey(KeyCode.Joystick1Button7);
        _btn8 = Input.GetKey(KeyCode.Joystick1Button8);
        _btn9 = Input.GetKey(KeyCode.Joystick1Button9);
        _btn10 = Input.GetKey(KeyCode.Joystick1Button10);
        
        _btn11 = Input.GetKey(KeyCode.Joystick1Button11);
        _btn12 = Input.GetKey(KeyCode.Joystick1Button12);
        _btn13 = Input.GetKey(KeyCode.Joystick1Button13);
        _btn14 = Input.GetKey(KeyCode.Joystick1Button14);
        _btn15 = Input.GetKey(KeyCode.Joystick1Button15);

        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        _fire1 = Input.GetAxis("Fire1");
        _fire2 = Input.GetAxis("Fire2");
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

    
}
