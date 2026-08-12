using UnityEngine;

public class MainMenuController : SceneController
{    
    [SerializeField] private RectTransform _content; 

    public void LoadScene(int buildIndex)
    {        
        Game.SceneLoader.LoadScene(buildIndex);
    }

    public void OnLoadLevelByRewadedVideo(int level)
    {        
        LoadLevel(level);
    }

    public void PressLoadLevel(int level)
    {
        Game.Sound.Play(SoundClip.Click);

        if (IsLevelLock(level) && IsLevelAvialableByVideo(level))
        {            
            return;
        }

        if (IsLevelLock(level))
            return;

        LoadLevel(level);
    } 
    
    public void LoadLevel(int level)
    {
        Debug.Log("Load Level: " + level);        
        SaveScrollPosition();
        int buildIndex = Game.ConfigLevels.Level(level).SceneBuildIndex;//level + 1;
        Game.CurrentLevel = level;
        LoadScene(buildIndex);
    }

    public void PressLoadLastPlayedLevel() 
    {
        PressLoadLevel(Game.LastPlayedLevel);
    }

    public void PressSound()
    {
        Game.Sound.Play(SoundClip.Click);
    }

    public bool IsLevelLock(int level)
    {
        if (Game.Saves.GetPlayedLevels(level - 1))
            return false;

        int stars = Game.Saves.Stars + Game.Saves.PurchasedStars;
        return Game.ConfigLevels.Level(level).StarsForOpen > stars;
    }

    public bool IsLevelAvialableByVideo(int level)
    {

        if (!IsLevelLock(level))
            return false;

        if (!Game.IsTutorialShown)
            return false;

        //int stars = Game.Saves.Stars + Game.Saves.PurchasedStars;
        //if (Game.Levels.Level(level).StarsForOpen <= stars + 9)
        //    return true;

        return false;
    }

    private void SaveScrollPosition()
    {
        if (_content != null)//Del
            PlayerPrefs.SetInt("ScrollPosition", (int)_content.anchoredPosition.y);
    }
}
