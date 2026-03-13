using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameType
{
    Podnos,
    Race
}

public class ControllerLevel : MonoBehaviour
{
    [SerializeField] private ConfigLevels _config;
    [SerializeField] private GameType _gameType;
    [SerializeField] private Hub _hub;
    //[SerializeField] private Hero _hero;
    [SerializeField] private GameObject _screenFromShadowEffect;    
    
    [SerializeField] private ControllerRace _race;

    
    private float _timeStart;

    public GameType GameType => _gameType;

    public Hub Hub => _hub;

    //public Hero Hero => _hero;

    public ConfigLevel Level => _config.Level(_hub.Game.CurrentLevel);

    

    public Checkpoint LastCheckpoint { get; set; }

    public ControllerRace Race => _race;

    public bool HasCheckpoint => LastCheckpoint != null;

    public bool IsComplete { get; private set; }

    public bool IsLost { get; private set; }

    public bool IsPlaying => !IsLost && !IsComplete;

    public int CheckpointCakes { get; set; }

    public int CakesOnPodnos
    {
        get
        {
            int output = 0;

            return output;
        }        
    }

    public int StarsByType(LevelType type)
    {
        int output = 0;

        for (int i = 0; i < _hub.Game.CurrentLevel - 1; i++)
        {
            if (_config.Levels[i].Type == type)
            {
                //output += _hub.Game.Saves.GetLevelStars(i);
            }
        }
        //Debug.Log("StarsByType(" + type.ToString() + ")" + output);
        return output;
    }

    public int BricksByType(LevelType type)
    {
        int output = 0;

        for (int i = 1; i < _hub.Game.CurrentLevel; i++)
        {
            if (_config.Level(i).Type == type)
            {
                //int stars = _hub.Game.Saves.GetLevelStars(i - 1);                

                //if (stars > 0)
                //{                    
                //    output += _config.Level(i).StarsPrices[stars - 1];
                //}
            }
        }        
        return output;
    }

    public bool IsFood(LevelType type)
    {
        if (type == LevelType.Pumpkin)
            return true;

        if (type == LevelType.Apples)
            return true;

        if (type == LevelType.MushroomBrown)
            return true;

        if (type == LevelType.Waterlemon)
            return true;

        return false;
    }

    public bool IsFood()
    {
        return IsFood(Level.Type);
    }

    public bool IsRace => Race != null;

    public int PlayTime
    {
        get
        {
            return (int)(Time.time - _timeStart);
        }
    }

    public event Action<int> OnLevelComplete;
    public event Action OnLevelLost;
    public event Action OnLevelRestartFromCheckpoint;

    private void Start()
    {
        //GC.Collect();
        //SetAmbient();
        //_hub.Camera.SetTarget(_hero.CameraTarget);
        //_hub.CanvasLevel.SetButterflyCount(_hub.Game.Saves.Butterflies);
        //StartLevel();        
    }

    private void StartLevel()
    {
        _hub.Game.LastPlayedLevel = _hub.Game.CurrentLevel;           
        _hub.Joistick.ResetCenter();        
        
        //if (Level.CreateOnStart && GameType == GameType.Podnos)
        //{
            CreateCakes(Level.NumOfCakesOnStart);
            StartCoroutine(CheckCakesFalled(1));
        //}
        _timeStart = Time.time;
        _hub.Analitycs.SendLevelStart((int)_timeStart);        
    }

    public void RestartFromCheckpoint()
    {
        Debug.Log("RestartFromCheckpoint");
        IsLost = false;        

        ClearCakes();
        StopAllCoroutines();        
        CreateCakes(_hub.Level.CheckpointCakes);
        LastCheckpoint.Restart();
        ScreenFromShadowEffectStart();
        OnLevelRestartFromCheckpoint?.Invoke();
        StartCoroutine(CheckCakesFalled(1));        
        _hub.Analitycs.SendLevelRestartFromCheckpoint(PlayTime);        
        
    }

    private void ClearCakes()
    {
        

        //Удаление Crashed
        GameObject[] cakes = GameObject.FindGameObjectsWithTag("Cake");
        foreach(var c in cakes)
        {
            Destroy(c.gameObject);
        }
    }

    public void Finish()
    {
        if (IsLost)
            return;

        Debug.Log("Finish");
        IsComplete = true;
        _hub.Game.Sound.Play(SoundClip.LevelComplete);

        int cakesOnPodnos = CakesOnPodnos;
        if (cakesOnPodnos > 0)
        {
            BlockCakes();            
            int count = 0;
            for (int i = 3; i > 0; i--)
            {
                if (cakesOnPodnos >= Level.StarsPrices[i - 1])
                {
                    count = i; 
                    break;
                }
            }
            float time = IsFood() ? 1 : 3;
            StartCoroutine(ActivateStars(count, 3));            
            //_hub.Game.Score += count * 10;
        }
        OnLevelComplete?.Invoke(cakesOnPodnos);
        _hub.Analitycs.SendLevelComplete(PlayTime);
    }

    private void BlockCakes()
    {
           
    }

    private void FailAllCakes()
    {
        if (IsComplete)
            return;

        if (IsLost)
            return;

        Debug.Log("FailAllCakes");
        IsLost = true;        
        OnLevelLost?.Invoke();
        _hub.Analitycs.SendLevelLost(PlayTime);   
    }

    public void Dead()
    {
        if (IsComplete)
            return;

        if (IsLost)
            return;

        Debug.Log("Dead");
        IsLost = true;
        OnLevelLost?.Invoke();
        _hub.Analitycs.SendLevelLost(PlayTime);       
    }

    public IEnumerator ActivateStars(int count, float time)
    {
        yield return new WaitForSeconds(time);  
        //Debug.Log("ActivateStars(" + count + ")");
        
    }

    public void CreateCakes(int count)
    { 
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = Vector3.zero;
            if (_hub.Level.GameType == GameType.Podnos)
            {
                //pos = _hero.PodnosFloor.transform.position + Vector3.up * (i * _config.Cake(Level.Type).Height + 0.01f);//0.01 - Podnos collider height/2
            }

            if (_hub.Level.GameType == GameType.Race)
            {
                pos = _hub.Level.Race.Car.PodnosPosition.position;
            }
            Quaternion rot = Quaternion.Euler(0, i * Level.CakeRotation, 0);
            CreateCake(pos, rot, i);
        }
    }

    private void CreateCake(Vector3 position, Quaternion roation, int number)
    {
        
        GameObject cakePosition = new GameObject("CakePosition");
        
        if (_hub.Level.GameType == GameType.Race)
        { 
            cakePosition.transform.parent = Race.Car.PodnosPosition.transform;
            //cake.transform.parent = Car.PodnosPosition.transform;
        }

        
    }

    private IEnumerator CheckCakesFalled(float time)
    {
        yield return new WaitForSeconds(time);  
    }  

    private void ScreenFromShadowEffectStart()
    {
        GameObject effect = Instantiate(_screenFromShadowEffect, Vector3.zero, Quaternion.identity);
        effect.transform.SetParent(_hub.CanvasLevel.transform, false);
    }    

    private void SetAmbient()
    {
        ConfigLevel config = _config.Level(_hub.Game.CurrentLevel);
        RenderSettings.ambientLight = config.AmbientColor;
        RenderSettings.fogColor = config.FogColor;
        RenderSettings.fogDensity = config.FogDensity;
    }    
}
        