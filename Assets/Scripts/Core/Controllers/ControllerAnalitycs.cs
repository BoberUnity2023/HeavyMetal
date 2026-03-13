using GameAnalyticsSDK;
using UnityEngine;

public class ControllerAnalitycs : MonoBehaviour
{
    [SerializeField] private GameController _game;

    private void Start()
    {
        GameAnalytics.Initialize();
        SendDeviceType(_game.Device);
    }

    public void SendDeviceType(Device device)
    {
        string eventName = "Device:" + device.ToString();
        GameAnalytics.NewDesignEvent(eventName);
    }

    public void SendLevelStart(int time)
    {
        int level = _game.CurrentLevel;
        string eventName = "Level_" + level + ":Start";
        GameAnalytics.NewDesignEvent(eventName, time);  
        //Debug.LogWarning(eventName);
    }

    public void SendLevelStartFirst(int time)
    {
        int level = _game.CurrentLevel;
        string eventName = "Level_" + level + ":StartFirst";
        GameAnalytics.NewDesignEvent(eventName, time);
        //Debug.LogWarning(eventName);
    }

    public void SendLevelLost(int time)
    {
        int level = _game.CurrentLevel;
        string eventName = "Level_" + level + ":Lost";
        GameAnalytics.NewDesignEvent(eventName, time);
        //Debug.LogWarning(eventName);
    }

    public void SendLevelComplete(int time)
    {
        int level = _game.CurrentLevel;
        string eventName = "Level_" + level + ":Complete";
        GameAnalytics.NewDesignEvent(eventName, time);
        //Debug.LogWarning(eventName);
    }

    public void SendLevelRestartFromCheckpoint(int time)
    {
        int level = _game.CurrentLevel;
        string eventName = "Level_" + level + ":RestartFromCheckpoint";
        GameAnalytics.NewDesignEvent(eventName, time);
        //Debug.LogWarning(eventName);
    }

    public void SendLevelRestartFromCheckpointButterfly(int time)
    {
        int level = _game.CurrentLevel;
        string eventName = "Level_" + level + ":RestartFromCheckpointButterfly";
        GameAnalytics.NewDesignEvent(eventName, time);
        //Debug.LogWarning(eventName);
    }

    public void SendLevelTakeButterfly(int time)
    {
        int level = _game.CurrentLevel;
        string eventName = "Level_" + level + ":TakeButterfly";
        GameAnalytics.NewDesignEvent(eventName, time);
        //Debug.LogWarning(eventName);
    }

    public void SendLevelQuitGame()//From YandexGame
    {
        int level = _game.CurrentLevel;
        string eventName = "Level_" + level + ":QuitGame";
        GameAnalytics.NewDesignEvent(eventName);
    }    

    public void SendTutorialStart()
    {
        string eventName = "Tutorial:Start";
        GameAnalytics.NewDesignEvent(eventName);
        //Debug.LogWarning(eventName);
    }

    public void SendTutorialComplete()
    {
        string eventName = "Tutorial:Complete";
        GameAnalytics.NewDesignEvent(eventName);
        //Debug.LogWarning(eventName);
    }

    public void SendPurchase(string id)
    {
        string eventName = "Purchase:" + id;
        GameAnalytics.NewDesignEvent(eventName);
    }

    public void SendBuy(string id)
    {
        string eventName = "Buy:" + id;
        GameAnalytics.NewDesignEvent(eventName);
    }

    public void SendHatTake()
    {
        string eventName = "Hat:Taken";
        GameAnalytics.NewDesignEvent(eventName);
        //Debug.LogWarning(eventName);
    }
    public void SendHatCompleteLevel()
    {
        string eventName = "Hat:CompleteLevel";
        GameAnalytics.NewDesignEvent(eventName);
        //Debug.LogWarning(eventName);
    }

    public void SendHeroForced(string enemy)
    {
        int level = _game.CurrentLevel;
        string eventName = "Level_" + level + ":HeroForced:" + enemy;
        GameAnalytics.NewDesignEvent(eventName);
    }

    public void SendVisit(string type)
    {        
        string eventName = "Visit:" + type;
        GameAnalytics.NewDesignEvent(eventName);
    }

    public void SendRewardedVideoPress(int id)
    {
        string eventName = "RewardedVideo:Press:" + id.ToString();
        GameAnalytics.NewDesignEvent(eventName);
        //Debug.LogWarning(eventName);
    }

    public void SendRewardedVideoComplete(int id)
    {
        string eventName = "RewardedVideo:Complete:" + id.ToString();
        GameAnalytics.NewDesignEvent(eventName);
        Debug.LogWarning(eventName);
    }

    public void SendDayBonus(string type)
    {
        string eventName = "DayBonus:" + type;
        GameAnalytics.NewDesignEvent(eventName);
        //Debug.LogWarning(eventName);
    }

    public void SendTimeBonus()
    {
        string eventName = "TimeBonus:";
        GameAnalytics.NewDesignEvent(eventName);
        //Debug.LogWarning(eventName);
    }
}