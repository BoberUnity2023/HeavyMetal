using System;
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
    GamePush,
    Steam
}

public enum CarType
{
    Jeep,
    Police,
    Gnom
}

public enum GameLanguage
{
    System,
    English,
    Russian
}

public enum GroundMaterial
{  
    Blocker = 0,
    Asphalt = 1,
    Sand = 2,
    Snow = 3,
    Grass = 4    
}

[Serializable] public struct GroundProps
{
    public GroundMaterial GroundMaterial;
    public PhysicsMaterial PhysicMaterial;
    public Color Color;
    public ParticleSystem PrefabParticles;
    public int RateOverTimeMax;
    public float Friction;
}

[Serializable]
public class CarColor
{
    [SerializeField] private Color _color;
    [SerializeField] private Material _material;

    public Color Color => _color;
    public Material Material => _material;
}

[Serializable]
public class CarTuning
{
    [SerializeField] private TuningCategory _engine;
    [SerializeField] private TuningCategory _shields;
    [SerializeField] private TuningCategory _tires;
    [SerializeField] private TuningCategory _weapon;
    [SerializeField] private TuningCategory _nitro;
    [SerializeField] private TuningCategory _mines;
    [SerializeField] private TuningCategory _shield;
    [SerializeField] private CarColor[] _carColors;

    public TuningCategory Engine => _engine;

    public TuningCategory Shields => _shields;

    public TuningCategory Tires => _tires;

    public TuningCategory Weapon => _weapon;

    public TuningCategory Nitro => _nitro;

    public TuningCategory Mines => _mines;

    public TuningCategory Shield => _shield;

    public CarColor[] CarColors => _carColors;
}

[Serializable]
public class TuningCategory
{
    [HideInInspector] public int CountBought;
    public int CountMax;
    public float Power;
    public int Price;
}

public class GameController : MonoBehaviour
{
    [SerializeField] private ConfigGame _config;
    [SerializeField] private ConfigLevels _configLevels;
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private SavesContoller _savesContoller;
    [SerializeField] private ControllerSound _soundContoller;
    [SerializeField] private ControllerUI _uiContoller;
    [SerializeField] private ControllerSettings _controllerSettings;
    [SerializeField] private LocalizeController _localizeController;
    [SerializeField] private ControllerAnalitycs _controllerAnalitycs;
    [SerializeField] private Canvas _console;
    [SerializeField] private Skidmarks _prefabSkidmarks;
       
    [SerializeField] private GroundProps[] _groundPropses;
    
    private bool _isMobile;
    
    private int _currentLevel;
    private int _previousScene;

    public int Coins
    {
        get 
        { 
            return Saves.Coins; 
        }

        set 
        { 
            Saves.Coins = value; 
        }
    }
    public bool HasFocus { get; set; }
    
    public GroundProps[] GroundPropses => _groundPropses;

    public int SelectedCar
    {
        get
        {
            return Saves.SelectedCar;
        }

        set
        {
            Saves.SelectedCar = value;
        }
    }

    public CarType SelectedCarType 
    { 
        get 
        {
            return ConfigGame.Cars[SelectedCar].CarType;
        } 
    }


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

    public ConfigGame ConfigGame => _config;

    public ConfigLevels ConfigLevels => _configLevels;

    public SceneLoader SceneLoader => _sceneLoader;

    public SavesContoller Saves => _savesContoller;

    public ControllerSound Sound => _soundContoller;

    public ControllerUI UI => _uiContoller;

    public ControllerSettings Settings => _controllerSettings;

    public LocalizeController Localize => _localizeController;

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

                for (int i = 0; i < _configLevels.Levels.Length; i++)
                {
                    if (_configLevels.Levels[i].name.Contains(sceneName))
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
                SceneLoader.LoadScene(1);
        }

        if (scene.buildIndex == 1)
        {
            Garage garage = FindFirstObjectByType<Garage>();
            bool fromLevel = _previousScene >= 2;
            garage.Init(this, fromLevel);
        }

        if (scene.buildIndex == 2)
        {
            MainMenuController mainMenuController = FindFirstObjectByType<MainMenuController>();
            bool fromLevel = _previousScene >= 2;
            //mainMenuController.Init(this, fromLevel);
            Settings.SetGrafics();
        }
        if (scene.buildIndex >= 3)
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

    public bool IsEqualPhysicsMaterials(PhysicsMaterial material1, PhysicsMaterial material2)
    {
        return material1.dynamicFriction == material2.dynamicFriction &&
            material1.frictionCombine == material2.frictionCombine;
    }

    public bool IsMaterialGround(PhysicsMaterial material)
    {
        foreach (var m in GroundPropses)
        {
            if (IsEqualPhysicsMaterials(material, m.PhysicMaterial))
                return true;
        }

        return false;
    }
}
