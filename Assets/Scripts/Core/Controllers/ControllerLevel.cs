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
    [SerializeField] private Hub _hub;    
    [SerializeField] private GameObject _screenFromShadowEffect;  
    [SerializeField] private ControllerRace _race;
    [SerializeField] private GameObject[] _tracks;
    private float _timeStart;

    public Hub Hub => _hub;

    public ConfigLevel Config => _config.Level(1);

    public ConfigLevel Level => _config.Level(_hub.Game.CurrentLevel);     

    public ControllerRace Race => _race;    

    public bool IsComplete { get; private set; }

    public bool IsLost { get; private set; }

    public bool IsPlaying => !IsLost && !IsComplete;    

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
        StartLevel();        
    }

    private void StartLevel()
    {
        _hub.Game.LastPlayedLevel = _hub.Game.CurrentLevel;           
        //_hub.Joistick.ResetCenter();  
        _timeStart = Time.time;
        //_hub.Analitycs.SendLevelStart((int)_timeStart);
        bool isFirstStart = !_hub.Game.Saves.GetPlayedLevels(_hub.Game.CurrentLevel - 1);
        if (isFirstStart)
        {
            _hub.Game.Saves.SetPlayedLevels(_hub.Game.CurrentLevel - 1, true);
            //_hub.Analitycs.SendLevelStartFirst((int)_timeStart);
        }

        SetTrack();
    }

    private void SetTrack()
    {
        int level = _hub.Game.CurrentLevel;
        for (int i = 0; i < _tracks.Length; i++)
        {
            _tracks[i].SetActive(level == i + 1);
        }
    }

    public void RestartFromCheckpoint()
    {
        //Debug.Log("RestartFromCheckpoint");
        //IsLost = false;        

        //ClearCakes();
        //StopAllCoroutines();        
        //CreateCakes(_hub.Level.CheckpointCakes);
        //LastCheckpoint.Restart();
        //ScreenFromShadowEffectStart();
        //OnLevelRestartFromCheckpoint?.Invoke();
        //StartCoroutine(CheckCakesFalled(1));        
        //_hub.Analitycs.SendLevelRestartFromCheckpoint(PlayTime);        
        
    }

    

    //public void Finish()
    //{
    //    if (IsLost)
    //        return;

    //    Debug.Log("Finish");
    //    IsComplete = true;
    //    _hub.Game.Sound.Play(SoundClip.LevelComplete);

    //    int cakesOnPodnos = CakesOnPodnos;
    //    if (cakesOnPodnos > 0)
    //    {
    //        BlockCakes();            
    //        int count = 0;
    //        for (int i = 3; i > 0; i--)
    //        {
    //            if (cakesOnPodnos >= Level.StarsPrices[i - 1])
    //            {
    //                count = i; 
    //                break;
    //            }
    //        }
    //        float time = IsFood() ? 1 : 3;
    //        StartCoroutine(ActivateStars(count, 3));            
    //        //_hub.Game.Coins += count * 10;
    //    }
    //    OnLevelComplete?.Invoke(cakesOnPodnos);
    //    _hub.Analitycs.SendLevelComplete(PlayTime);
    //}

        

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

    private void ScreenFromShadowEffectStart()
    {
        GameObject effect = Instantiate(_screenFromShadowEffect, Vector3.zero, Quaternion.identity);
        effect.transform.SetParent(_hub.CanvasLevel.transform, false);
    }    

    private void SetAmbient()
    {
        ConfigLevel config = _config.Level(_hub.Game.CurrentLevel);
        //RenderSettings.ambientLight = config.AmbientColor;
        //RenderSettings.fogColor = config.FogColor;
        //RenderSettings.fogDensity = config.FogDensity;
    }    
}
        