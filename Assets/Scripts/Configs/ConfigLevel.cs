using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Configs/ConfigLevel")]
public class ConfigLevel : ScriptableObject
{
    /*[SerializeField]*/ private string _title;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _sceneBuildIndex;
    [SerializeField] private LevelType _type;
    [SerializeField] private int _numOfCakesOnStart;
    [SerializeField] private int _cakeRotation;//used if _numOfCakesOnStart > 1
    /*[SerializeField] */private bool _createOnStart;
    [SerializeField] private int _starsForOpen;
    [SerializeField] private Color _ambientColor;
    [SerializeField] private Color _fogColor;
    [SerializeField] private float _fogDensity;

    public Sprite Icon => _icon;

    public int SceneBuildIndex => _sceneBuildIndex;

    public LevelType Type => _type;

    public int StarsForOpen => _starsForOpen;

    public int NumOfCakesOnStart => _numOfCakesOnStart;

    public int CakeRotation => _cakeRotation;

    public bool CreateOnStart => true;

    public Color AmbientColor => _ambientColor;

    public Color FogColor => _fogColor;

    public float FogDensity => _fogDensity;

    public int[] StarsPrices
    {
        get
        {            
            if (_numOfCakesOnStart == 1)
                return new int[3] { 1, 1, 1 };

            if (_numOfCakesOnStart == 2)
                return new int[3] { 1, 2, 2 };

            if (_numOfCakesOnStart == 3)
                return new int[3] { 1, 2, 3 };

            if (_numOfCakesOnStart == 4)
                return new int[3] { 1, 3, 4 };

            if (_numOfCakesOnStart == 5)
                return new int[3] { 1, 3, 5 };

            if (_numOfCakesOnStart == 6)
                return new int[3] { 1, 3, 6 };

            if (_numOfCakesOnStart == 7)
                return new int[3] { 1, 3, 7 };

            return new int[3] { 1, 1, 1 };
        }
    }

}
