using UnityEngine;

public class ControllerRewardedVideo : MonoBehaviour
{
    [SerializeField] private GameController _game;
    private int _id;
    private int _loadingLevelByRewadedVideo;

    private void OnEnable() 
    { 
        
    }

    // Отписываемся от события открытия рекламы в OnDisable
    private void OnDisable() 
    {
        
    }    

    // Подписанный метод получения награды
    public void Rewarded(int id)
    {
        
    }

    // Метод для вызова видео рекламы
    public void PressOpenRewardAd(int id)
    {
        _game.HasFocus = false;
        _id = id;  
        
        _game.Sound.MusicPause();
        _game.Analitycs.SendRewardedVideoPress(id);
    }

    public void Rewarded()
    {
        _game.HasFocus = true;
        Rewarded(_id);
    }

    public void OnRewardedReward(string id)
    {
        Debug.Log("GP. Rewarded: " + id);
        _game.HasFocus = true;
        Rewarded(_id);
    }

    public void RewardedError()
    {
        _game.HasFocus = true;
        _game.Sound.Play(SoundClip.CakeCrash);
    }

    public void PressLoadLevelByVideo(int level)
    {
        _loadingLevelByRewadedVideo = level;
        PressOpenRewardAd(2);
    }

    private void OnRewadedSuccess(int id)
    {
        _game.Sound.Play(SoundClip.LevelComplete);
        _game.Sound.MusicPlay();
        _game.Analitycs.SendRewardedVideoComplete(id);
    }    

    public void OnRewardedStart()//Editor YandexGame + GP
    {
        _game.HasFocus = false;
    }

    public void OnRewardedClose(bool result)//Editor YandexGame + GP
    {
        Debug.Log("GP.OnRewardedClose(" + result + ")");
        _game.HasFocus = true;
        if (!result)
            RewardedError();
    }
}
