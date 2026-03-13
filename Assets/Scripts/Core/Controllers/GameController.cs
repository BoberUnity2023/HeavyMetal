using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

public enum Device
{
    Editor,
    Desktop,
    Mobile
}

public enum Platform
{
    Yandex,
    Ok,
    Vk,
    GamePush
}

public enum GroundMaterial
{  
    Blocker = 0,
    Asphalt = 1,
    Sand = 2,
    Grass = 3,
    Snow = 4
}

[Serializable] public struct GroundProps
{
    public GroundMaterial GroundMaterial;
    public PhysicsMaterial PhysicMaterial;
    public Color Color;
    public ParticleSystem PrefabParticles;    
    public float Friction;
}


public class GameController : MonoBehaviour
{
    [SerializeField] private ConfigGame _config;
    [SerializeField] private ConfigLevels _levels;
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private ControllerSound _soundContoller;
    [SerializeField] private ControllerSettings _controllerSettings;
    [SerializeField] private ControllerAnalitycs _controllerAnalitycs;
    [SerializeField] private Canvas _console;

    [SerializeField] private Skidmarks _prefabSkidmarks; 
    [SerializeField] private GroundProps[] _groundPropses;
    
    private bool _isMobile;
    
    private int _currentLevel;
    private int _previousScene;

    public bool HasFocus { get; set; }

    public GroundProps[] GroundPropses => _groundPropses;

    public event Action<int> OnScoreChanged;

#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern bool IsMobile();
#endif

    public Device Device
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
                return Device.Desktop;// Editor;
            
            Debug.LogError("Device error");
            return Device.Desktop;//TODO: DLL
        }
    }

    public Platform Platform => _config.Platform;

    public Hub Hub { get; private set; }

    public ConfigLevels Levels => _levels;

    public SceneLoader SceneLoader => _sceneLoader;

    public ControllerSound Sound => _soundContoller;

    public ControllerSettings Settings => _controllerSettings;    

    public Canvas Console => _console;

    private bool isWaitingStorage;
    
    public int CurrentLevel 
    { 
        get 
        {
            if (_currentLevel == 0)//from Editor
            { 
                int levelNumber = 0;
                string sceneName = SceneManager.GetActiveScene().name.Substring(3);//��� "04_"                

                for (int i = 0; i < _levels.Levels.Length; i++)
                {
                    if (_levels.Levels[i].name.Contains(sceneName))
                    {                        
                        levelNumber = i + 1;
                        break;
                    }
                } 
                //Debug.Log("Scene " + sceneName + " started as Level: " + levelNumber);
                return levelNumber;
                //return SceneManager.GetActiveScene().levelNumber - 1; 
            }

            return _currentLevel;
        } 
        set 
        { 
            _currentLevel = value; 
        }
    }       

    public ControllerAnalitycs Analitycs => _controllerAnalitycs;

    public Skidmarks PrefabSkidmarks => _prefabSkidmarks;

    public int Stars
    {
        get
        { 
            return PlayerPrefs.GetInt("Stars"); 
        }
        set
        {
            PlayerPrefs.SetInt("Stars", value);
        }
    }

    public int LastPlayedLevel
    {
        get
        {
            return PlayerPrefs.GetInt("LastPlayedLevel", 1);
        }
        set
        {
            PlayerPrefs.SetInt("LastPlayedLevel", value);
            PlayerPrefs.Save();
        }
    }

    public int Score
    {
        get
        {
            return 0;
        }

        set
        {            
            OnScoreChanged?.Invoke(value);
        }
    }

    public bool IsTutorialShown
    {
        get { return true; }        
    }
    #region Unity
    private void Awake()
    {
        MarkAsSingletoon();
        DontDestroyOnLoad(gameObject);
        //Application.targetFrameRate = 10;// PlayerPrefs.GetInt("Framerate");

        //Int32.TryParse(YandexGame.EnvironmentData.payload, out int result);
        //Deeplink = result;
        //Debug.Log("Deeplink Start: " + Deeplink.ToString());
        Debug.Log(Application.productName + " v." + Application.version);        

#if !UNITY_EDITOR && UNITY_WEBGL
        _isMobile = IsMobile();
#endif
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;        
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        HasFocus = hasFocus;
    }

    private void Update()
    {
        //Update_WaitStorageRecived();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SceneLoaded: " + scene.buildIndex + "; " + scene.name);        
        HasFocus = true;

        if (scene.buildIndex == 0)
        {
            //if (Platform == Platform.Vk)
            //{
            //    isWaitingStorage = true;
            //    //StartCoroutine(WaitStorageReciveFail(3));
            //}

            //if (Platform != Platform.Vk)
            //    SceneLoader.LoadScene(1);
        }

        if (scene.buildIndex == 1)
        {
            MainMenuController mainMenuController = FindObjectOfType<MainMenuController>();
            bool fromLevel = _previousScene >= 2;
            mainMenuController.Init(this, fromLevel);
            Settings.SetGrafics();
        }
        if (scene.buildIndex >= 2)
        {
            Hub = FindObjectOfType<Hub>();
            Hub.Game = this;
            Settings.SetGrafics();
        }
        _previousScene = scene.buildIndex;
    }
    #endregion

    private void MarkAsSingletoon()
    {
        GameController[] gameControllers = FindObjectsOfType<GameController>();
        if (gameControllers.Length > 1) 
        {
            Destroy(gameObject);
        }
    }     
}
