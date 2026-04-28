//using Beebyte.Obfuscator;
//using GamePush;
using System;
using UnityEngine;
//using YG;

[Serializable]
public class Save
{    
    public bool HasNoAds;    
    public int PurchasedStars;
    public int EveryDayVisits;
    public int Coins;
    public string LastVisitTime;    
    public int[] LevelStars;
    public bool[] PlayedLevels;
    public bool[] TakenDayBonuses;    
}

public class SavesContoller : MonoBehaviour
{
    [SerializeField] private GameController _game;
    //private string _json;
    private bool _canSave;

    public Save Save = new Save();

    public string KeyLevelStars => "LevelStars";

    public string KeyPlayedLevels => "PlayedLevels";    

    public string KeyPurchasedStars => "PurchasedStars";  

    public string KeyCoins => "Coins";

    public string KeyBoughtCar => "BoughtCar";

    public string KeyTuning => "Tuning";

    public string KeyColor => "Color";

    public string KeyEveryDayVisits => "EveryDayVisits";

    public string KeyLastVisitTime => "LastVisitTime";

    public string KeyTakenDayBonuses => "TakenDayBonuses";

    public string KeyNoAds => "NoAds";
    
    public string KeyJson => "json";

    public bool IsStorageReceived { get; set; }

    public event Action<int> OnCoinsChanged;

    public int PurchasedStars
    {
        get
        {
            //if (_game.Platform == Platform.Yandex)
            //{
            //    return YandexGame.savesData.PurchasedStars;
            //}

            //if (_game.Platform == Platform.Vk || 
            //    _game.Platform == Platform.GamePush)
            //{
            //    return Mathf.Max(
            //        Save.PurchasedStars,
            //        PlayerPrefs.GetInt(KeyPurchasedStars, 0)
            //        );
            //}

            //if (_game.Platform == Platform.Ok)
            //{
            //    return PlayerPrefs.GetInt(KeyPurchasedStars, 0);
            //}
            return 0;
        }

        set
        {
            PlayerPrefs.SetInt(KeyPurchasedStars, value);
            PlayerPrefs.Save();

            //if (_game.Platform == Platform.Yandex)
            //{
            //    YandexGame.savesData.PurchasedStars = value;
            //    YandexGame.SaveProgress();
            //}            

            //if (_game.Platform == Platform.Vk || 
            //    _game.Platform == Platform.GamePush)
            //{
            //    Save.PurchasedStars = value;
            //    StorageSave();
            //}
        }
    }   
    

    public bool HasNoAds
    {
        get
        {
            if (_game.Platform == Platform.Yandex)
            {
                //return YandexGame.savesData.HasNoAds;
            }

            if (_game.Platform == Platform.Vk ||
                _game.Platform == Platform.GamePush)
            {
                return Save.HasNoAds || PlayerPrefs.GetInt(KeyNoAds, 0) == 1;
            }

            if (_game.Platform == Platform.Steam)
            {
                return PlayerPrefs.GetInt(KeyNoAds, 0) == 1;
            }

            return false;
        }

        set
        {
            PlayerPrefs.SetInt(KeyNoAds, value ? 1 : 0);
            PlayerPrefs.Save();

            if (_game.Platform == Platform.Yandex)
            {
                //YandexGame.savesData.HasNoAds = value;
                //YandexGame.SaveProgress();
            }

            if (_game.Platform == Platform.Vk ||
                _game.Platform == Platform.GamePush)
            {
                Save.HasNoAds = value;
                StorageSave();
            }
        }
    }

    public int GetLevelStars(int level)//Level1 - levelId = 0
    {
        if (_game.Platform == Platform.Yandex)
        {
            //return YandexGame.savesData.LevelStars[level];
        }

        if (_game.Platform == Platform.Vk ||
            _game.Platform == Platform.GamePush)
        {
            try
            {
                return Mathf.Max(Save.LevelStars[level], PlayerPrefs.GetInt(KeyLevelStars + level.ToString(), 0));
            }
            catch (Exception e)
            {
                Debug.LogError("Error: GetLevelStars(" + level + ")" + e.Message);
                return 0;
            }            
        }

        if (_game.Platform == Platform.Steam)
        {
            return PlayerPrefs.GetInt(KeyLevelStars + level.ToString(), 0);
        }
        return 0;
    }

    public void SetLevelStars(int level, int stars)
    {
        PlayerPrefs.SetInt(KeyLevelStars + level.ToString(), stars);
        PlayerPrefs.Save();

        if (_game.Platform == Platform.Yandex)
        {
            //YandexGame.savesData.LevelStars[level] = stars;
            //YandexGame.SaveProgress();
        }

        if (_game.Platform == Platform.Vk ||
            _game.Platform == Platform.GamePush)
        {
            Save.LevelStars[level] = stars;
            StorageSave();
        }
    }    

    public bool GetPlayedLevels(int levelId)
    {
        if (_game.Platform == Platform.Yandex)
        {
            //return YandexGame.savesData.PlayedLevels[levelId];
        }

        string key = KeyByLevels(levelId);
        if (_game.Platform == Platform.Vk || 
            _game.Platform == Platform.GamePush)
        {
            return 
                Save.PlayedLevels[levelId] ||
                PlayerPrefs.GetInt(key, 0) == 1;
        }

        if (_game.Platform == Platform.Steam)
        {
            return PlayerPrefs.GetInt(key, 0) == 1;
        }

        return false;
    }

    public void SetPlayedLevels(int level, bool value)
    {
        string key = KeyByLevels(level);
        PlayerPrefs.SetInt(key, value ? 1 : 0);
        PlayerPrefs.Save();

        if (_game.Platform == Platform.Yandex)
        {
            //YandexGame.savesData.PlayedLevels[level] = value;
            //YandexGame.SaveProgress();
        }

        if (_game.Platform == Platform.Vk ||
            _game.Platform == Platform.GamePush)
        {
            Save.PlayedLevels[level] = value;
            StorageSave();
        }      
    }

    private string KeyByLevels(int level)
    {
        return KeyPlayedLevels + level.ToString();
    }

    public int EveryDayVisits
    {
        get
        {
            if (_game.Platform == Platform.Yandex)
            {
                //return YandexGame.savesData.EveryDayVisits;
            }

            if (_game.Platform == Platform.Vk || 
                _game.Platform == Platform.GamePush)
            {
                return Mathf.Max(
                    Save.EveryDayVisits, 
                    PlayerPrefs.GetInt(KeyEveryDayVisits, 0)
                    );
            }

            if (_game.Platform == Platform.Steam)
            {
                return PlayerPrefs.GetInt(KeyEveryDayVisits, 0);
            }
            return 0;
        }

        set
        {
            PlayerPrefs.SetInt(KeyEveryDayVisits, value);
            PlayerPrefs.Save();

            if (_game.Platform == Platform.Yandex)
            {
                //YandexGame.savesData.EveryDayVisits = value;
                //YandexGame.SaveProgress();
            }

            if (_game.Platform == Platform.Vk ||
            _game.Platform == Platform.GamePush)
            {
                Save.EveryDayVisits = value;
                StorageSave();
            }
        }
    }

    public string LastVisitTime
    {
        get
        {
            if (_game.Platform == Platform.Yandex)
            {
                //return YandexGame.savesData.LastVisitTime;
            }

            if (_game.Platform == Platform.Vk ||
                _game.Platform == Platform.GamePush)
            {
                return Save.LastVisitTime;//TODO: PlayerPrefs
            }

            if (_game.Platform == Platform.Steam || 
                _game.Platform == Platform.Vk)
            {
                return PlayerPrefs.GetString(KeyLastVisitTime, "0");
            }
            return "0";
        }

        set
        {

            PlayerPrefs.SetString(KeyLastVisitTime, value);
            PlayerPrefs.Save();

            if (_game.Platform == Platform.Yandex)
            {
                //YandexGame.savesData.LastVisitTime = value;
                //YandexGame.SaveProgress();
            }            

            if (_game.Platform == Platform.Vk || 
                _game.Platform == Platform.GamePush)
            {
                Save.LastVisitTime = value;
                StorageSave();
            }
        }
    }

    public bool GetTakenDayBonuses(int day)
    {
        if (_game.Platform == Platform.Yandex)
        {
            //return YandexGame.savesData.TakenDayBonuses[day];
        }

        if (_game.Platform == Platform.Vk || 
            _game.Platform == Platform.GamePush)
        {
            return 
                Save.TakenDayBonuses[day] || 
                PlayerPrefs.GetInt(KeyTakenDayBonuses + day.ToString(), 0) == 1;
        }

        if (_game.Platform == Platform.Steam)
        {
            return PlayerPrefs.GetInt(KeyTakenDayBonuses + day.ToString(), 0) == 1;
        }
        return false;
    }

    public void SetTakenDayBonuses(int day, bool value)
    {
        PlayerPrefs.SetInt(KeyTakenDayBonuses + day.ToString(), value ? 1 : 0);
        PlayerPrefs.Save();

        if (_game.Platform == Platform.Yandex)
        {
            //YandexGame.savesData.TakenDayBonuses[day] = value;
            //YandexGame.SaveProgress();
        }        

        if (_game.Platform == Platform.Vk ||
            _game.Platform == Platform.GamePush)
        {
            Save.TakenDayBonuses[day] = value;
            StorageSave();
        }
    }

    public void ResetTakenDayBonuses()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt(KeyTakenDayBonuses + i.ToString(), 0);
        }
        PlayerPrefs.Save();

        if (_game.Platform == Platform.Yandex)
        {
            //YandexGame.savesData.TakenDayBonuses = new bool[5] { false, false, false, false, false };
            //YandexGame.SaveProgress();
        }

        if (_game.Platform == Platform.Vk ||
            _game.Platform == Platform.GamePush)
        {            
            for (int i = 0; i < 5; i++)
            {
                Save.TakenDayBonuses[i] = false;
            }
            StorageSave();
        }
           
    }

    public int Coins
    {
        get
        {
            int startCoins = _game.ConfigGame.StartCoins;

            if (_game.Platform == Platform.Yandex)
            {
                //return YandexGame.savesData.Coins;
            }

            if (_game.Platform == Platform.Vk ||
                _game.Platform == Platform.GamePush)
            {                
                return Mathf.Max(Save.Coins, PlayerPrefs.GetInt(KeyCoins, startCoins));
            }

            if (_game.Platform == Platform.Steam)
            {
                return PlayerPrefs.GetInt(KeyCoins, startCoins);
            }

            return 0;
        }

        set
        {
            PlayerPrefs.SetInt(KeyCoins, value);
            PlayerPrefs.Save();

            if (_game.Platform == Platform.Yandex)
            {
                //YandexGame.savesData.Coins = value;
                //YandexGame.SaveProgress();
            }            

            if (_game.Platform == Platform.Vk ||
                _game.Platform == Platform.GamePush)
            {
                Save.Coins = value;
                StorageSave();                
            }
            OnCoinsChanged?.Invoke(value);
        }
    }

    public void SetBoughtCar(CarType cartype)
    {
        PlayerPrefs.SetInt(KeyBoughtCar + cartype.ToString(), 1);
        PlayerPrefs.Save();
    }

    public bool HasBoughtCar(CarType cartype)
    {
        return PlayerPrefs.GetInt(KeyBoughtCar + cartype.ToString()) == 1;        
    }

    public int GetTuning(CarType cartype, TuningType tuningType)
    {
        return PlayerPrefs.GetInt(KeyTuning + tuningType.ToString() + cartype.ToString(), 0);
    }

    public void SetTuning(CarType cartype, TuningType tuningType, int value)
    {
        PlayerPrefs.SetInt(KeyTuning + tuningType.ToString() + cartype.ToString(), value);
        PlayerPrefs.Save();
    }

    public void SetCarColor(CarType cartype, int value)
    {
        PlayerPrefs.SetInt(KeyColor + cartype.ToString(), value);
        PlayerPrefs.Save();
    }

    public int GetCarColor(CarType cartype)
    {
        return PlayerPrefs.GetInt(KeyColor + cartype.ToString(), 0);
    }

    public int ConvertStringToInt(string value)
    {
        int _output;        

        bool success = Int32.TryParse(value, out _output);
        if (!success)
        {
            Debug.LogWarning("Error ConvertStringToInt failed! Value:" + value);
            return 0;
        }

        return _output;
    }

    // Подписываемся на событие GetDataEvent в OnEnable
    private void OnEnable() 
    { 
        if (_game.Platform == Platform.Yandex)
        {
            //YandexGame.GetDataEvent += GetData;
        }

        if (_game.Platform == Platform.GamePush)
        {
            if (!PlayerPrefs.HasKey(_storageKey))
                PlayerPrefs.SetString(_storageKey, "");
        }
    }

    // Отписываемся от события GetDataEvent в OnDisable
    private void OnDisable() 
    {
        if (_game.Platform == Platform.Yandex)
        {
            //YandexGame.GetDataEvent -= GetData;
        }
    }

    private void Awake()
    {
        if (_game.Platform == Platform.Yandex)
        {
            // Проверяем запустился ли плагин
            //if (YandexGame.SDKEnabled == true)
            //{
            //    // Если запустился, то запускаем Ваш метод
            //    GetData();

            //    // Если плагин еще не прогрузился, то метод не запуститься в методе Start,
            //    // но он запустится при вызове события GetDataEvent, после прогрузки плагина
            //}
        }

        if (_game.Platform == Platform.Vk ||
            _game.Platform == Platform.GamePush ||
            _game.Platform == Platform.Steam)
        { 
            FillSaveFromPlayerPrefs(); 
        }
    }

    // Ваш метод, который будет запускаться в старте
    public void GetData()
    {
        //// Получаем данные из плагина и делаем с ними что хотим
        ////_gameManager.Gold = YandexGame.savesData.Gold;
        ////_game.
        ////Debug.Log("SavesContoller.GetData");        
        //YandexGame.SaveProgress();
        ////YandexGame.savesData.Gold;
    }

    public void SaveLevelStars(int count)
    {
        //Debug.Log(gameObject.name + ".SaveLevelStars(" + count + ")");
        int level = _game.CurrentLevel - 1;
        if (_game.Platform == Platform.Yandex)
        {
            //if (count > YandexGame.savesData.LevelStars[level])
            //{
            //    YandexGame.savesData.LevelStars[level] = count;
            //    YandexGame.NewLeaderboardScores("Stars", Stars);
            //    YandexGame.SaveProgress();
            //}
        }        

        if (_game.Platform == Platform.Steam || 
            _game.Platform == Platform.Vk || 
            _game.Platform == Platform.GamePush)
        {
            if (count > GetLevelStars(level))
            {
                SetLevelStars(level, count);
                if (_game.Platform == Platform.GamePush)
                {
                    //GP_Player.SetScore(Stars);
                }
                //YandexGame.NewLeaderboardScores("Stars", YandexGame.savesData.Stars);               
            }
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        if (_game.Platform == Platform.Yandex)
        {
            //YandexGame.ResetSaveProgress();
            //YandexGame.SaveProgress();
        }

        if (_game.Platform == Platform.Vk ||
            _game.Platform == Platform.GamePush)
        {
            FillSaveReset();
        }
    }

    public int Stars
    {
        get
        {
            int output = 0;
            for (int i = 0; i < 100; i++)
            {
                output += GetLevelStars(i);
            }
            return output;            
        }
    }    

    private void FillSaveFromPlayerPrefs()
    {
        Save.LevelStars = new int[100];
        for (int i = 0; i < 100; i++)
        {
            Save.LevelStars[i] = PlayerPrefs.GetInt(KeyLevelStars + i.ToString(), 0);
        }

        Save.PlayedLevels = new bool[100];
        for (int i = 0; i < 100; i++)
        {
            Save.PlayedLevels[i] = PlayerPrefs.GetInt(KeyPlayedLevels + i.ToString(), 0) == 1;
        }

        Save.TakenDayBonuses = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            Save.TakenDayBonuses[i] = PlayerPrefs.GetInt(KeyTakenDayBonuses + i.ToString(), 0) == 1;
        }        

        Save.LastVisitTime = PlayerPrefs.GetString(KeyLastVisitTime, "0");
        Save.Coins = PlayerPrefs.GetInt(KeyCoins, 0);        
        Save.HasNoAds = PlayerPrefs.GetInt(KeyNoAds, 0) == 1;        
        Save.PurchasedStars = PlayerPrefs.GetInt(KeyPurchasedStars, 0);
        Save.EveryDayVisits = PlayerPrefs.GetInt(KeyEveryDayVisits, 0);        
    }

    public void FillSaveFromPlayerPrefsOrStorage(Save save)
    {
        Save.LevelStars = new int[100];
        for (int i = 0; i < 100; i++)
        {
            Save.LevelStars[i] = Mathf.Max(PlayerPrefs.GetInt(KeyLevelStars + i.ToString(), 0), save.LevelStars[i]);
        }

        Save.PlayedLevels = new bool[100];
        for (int i = 0; i < 100; i++)
        {
            Save.PlayedLevels[i] = PlayerPrefs.GetInt(KeyPlayedLevels + i.ToString(), 0) == 1 || save.PlayedLevels[i];
        }

        Save.TakenDayBonuses = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            Save.TakenDayBonuses[i] = PlayerPrefs.GetInt(KeyTakenDayBonuses + i.ToString(), 0) == 1 || save.TakenDayBonuses[i];
        }

        int fromPlayerPrefs = ConvertStringToInt(PlayerPrefs.GetString(KeyLastVisitTime, "0"));
        int fromStorage = ConvertStringToInt(save.LastVisitTime);

        Save.LastVisitTime = Mathf.Max(fromPlayerPrefs, fromStorage).ToString();        

        Save.Coins = Mathf.Max(PlayerPrefs.GetInt(KeyCoins, 0), save.Coins);        
        Save.HasNoAds = PlayerPrefs.GetInt(KeyNoAds, 0) == 1 || save.HasNoAds;        
        Save.PurchasedStars = Mathf.Max(PlayerPrefs.GetInt(KeyPurchasedStars, 0), save.PurchasedStars);
        Save.EveryDayVisits = Mathf.Max(PlayerPrefs.GetInt(KeyEveryDayVisits, 0), save.EveryDayVisits); 
        //VKManager.Instance.StorageSave();
    }

    private void FillSaveReset()
    {
        Save.LevelStars = new int[100]; 
        Save.PlayedLevels = new bool[100];  
        Save.TakenDayBonuses = new bool[5];  
        Save.LastVisitTime = "0";
        Save.Coins = 0;        
        Save.HasNoAds = false;        
        Save.PurchasedStars = 0;
        Save.EveryDayVisits = 0;
        StorageSave();
    }
    
    private string _storageKey = "json";
    //GP

    public void OnPluginReady()
    {
        if (_game.Platform == Platform.GamePush)
        {
            //Debug.Log("Save.OnPluginReady");
            _canSave = true;
            //GP_Storage.Get(_storageKey, OnStorageGetAnswer);

            //Debug.Log("GetTING JSON...");
            //if (!GP_Player.Has(KeyJson))
            //{
            //    Debug.Log("JSON CREATED");
            //    _json = "{\"NoAds\":false,\"Coins\":0,\"Gold\":21944,\"Experience\":0,\"PlayedGames\":0,\"Wins\":0,\"Losts\":0,\"FastestWinTime\":0,\"FastestPartyTime\":0,\"LongestPartyTime\":0,\"LastVisitTime\":\"0\",\"TakenDayBonuses\":[false,false,false,false,false],\"AchivementProgress\":[0,0,0,0,0,0,0,0,0,0]}";
            //}
            //else
            //{
            //    Debug.Log("JSON loading");
            
            //string _json = GP_Player.GetString(KeyJson);
            //Debug.Log("Get JSON: " + _json);
            //if (_json.Length > 10)
            //{                               
            //    Save = JsonUtility.FromJson<Save>(_json);                
            //    Debug.Log("EveryDayVisits: " + Save.EveryDayVisits);
            //    Debug.Log("LevelStars: " + Save.LevelStars.Length);
            //    Debug.Log("PlayedLevels: " + Save.PlayedLevels.Length);
            //    Debug.Log("TakenDayBonuses: " + Save.TakenDayBonuses.Length);                
            //}
            //else
            //{
            //    Debug.Log("Create empty JSON");
            //    FillSaveReset();
            //}
        }
    }
    
    public void StorageSave()
    {
        #if UNITY_EDITOR && UNITY_WEBGL
        _canSave = true;
        #endif
        if (!_canSave)
        {
            Debug.LogError("StorageSetIgnored: canSave");
            return;
        }

        if (!IsSaveCorrect(Save))
        {
            Debug.LogError("StorageSetIgnored: arrayLength");
            return;
        }

        string json = JsonUtility.ToJson(Save);
        if (_game.Platform == Platform.Vk)
        {
            //VKManager.Instance.StorageSave();
        }

        if (_game.Platform == Platform.GamePush)
        {
            //GP_Storage.Set(_storageKey, json, OnSetValue);
            //GP_Player.Set(_storageKey, json);
            //GP_Player.Sync();
        }        
    }

    //private void OnSetValue(StorageField storage) 
    //{ 
    //    Debug.Log($"Set value: Key: {storage.key}, Value: {storage.value}"); 
    //}

    public void OnStorageGetAnswer(object value)//GP
    {
        string text = (string)value;
        Debug.Log("StorageGetAnswer: " + text);
        IsStorageReceived = true;
        _canSave = true;

        if (text.Length == 0)
        {
            Debug.LogWarning("StorageReceived Fail: text.Length == 0");
            return;
        }

        Save save = new Save();
        try
        {
            save = JsonUtility.FromJson<Save>(text);
        }
        catch (Exception ex)
        {
            Debug.LogError("Catch JSON Parse error: " + ex + "; text: " + text);
        }

        if (!IsSaveCorrect(save))
        {
            Debug.LogWarning("StorageReceived Fail: Save file is Uncorrect");
            return;
        }
        
        FillSaveFromPlayerPrefsOrStorage(save);
        Debug.LogWarning("StorageReceived: Success");
    }

    public bool IsSaveCorrect(Save save)
    {
        if (save.LevelStars.Length < 1 ||
        save.PlayedLevels.Length < 1 ||
        save.TakenDayBonuses.Length < 1)
        {
            return false;
        }

        return true;
    }

    /*private void SetSaveToJson()//GP
    {
        //Debug.Log("Setting json...");
        _json = JsonUtility.ToJson(Save);
        GP_Player.Set(KeyJson, _json);
        GP_Player.Sync();
        //Debug.Log("Set JSON: " + _json);
    }*/
}
