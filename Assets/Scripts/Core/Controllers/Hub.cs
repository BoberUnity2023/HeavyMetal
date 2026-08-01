using UnityEngine;

public class Hub : SceneController
{    
    [SerializeField] private ControllerLevel _level;
    [SerializeField] private ScreenController _screenController;
    [SerializeField] private InputController _input;    
    [SerializeField] private ControllerSoundLevel _sound;
    [SerializeField] private ControllerOptimization _optimization;
    [SerializeField] private CanvasLevel _canvasLevel;
    [SerializeField] private CameraMove _camera;    
    [SerializeField] private Joistick _joistick = null;
    [SerializeField] private WindowSettings _windowSettings;
    [SerializeField] private ResultController _resultController;
    [SerializeField] private RaceStarter _raceStarter;
    [SerializeField] private AudioListenerMovier _audioListenerMovier;    
    //public GameController Game { get; set; }

    public SceneLoader SceneLoader => Game.SceneLoader;       

    public ControllerLevel Level => _level;

    public ScreenController Screen => _screenController;

    public InputController Input => _input;

    public ControllerSoundLevel Sound => _sound;

    public ControllerOptimization Optimization => _optimization;

    public ControllerAnalitycs Analitycs => Game.Analitycs;

    public CanvasLevel CanvasLevel => _canvasLevel;

    public CameraMove Camera => _camera;

    public Joistick Joistick => _joistick;      

    public WindowSettings WindowSettings => _windowSettings;

    public ResultController Result => _resultController;

    public RaceStarter RaceStarter => _raceStarter;

    public AudioListenerMovier AudioListenerMovier => _audioListenerMovier;

    public ControllerRace Race => Level.Race;
}
