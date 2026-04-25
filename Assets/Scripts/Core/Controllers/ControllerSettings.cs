using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
//using UnityEngine.Rendering.PostProcessing;

public class ControllerSettings : MonoBehaviour
{
    [SerializeField] private GameController _game;
    [SerializeField] private Volume _volume;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _volume = FindObjectOfType<Volume>();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetGrafics()
    {
        int level = PlayerPrefs.GetInt("QualityLevel", 1);
        bool pss = false;
        int index = 0;  

        if (level <= 2)
        {
            index = level; ;
            pss = false;
        }

        if (level == 3)
        {
            index = 1;
            pss = true;
        }

        if (level == 4)
        {
            index = 3;
            pss = true;
        }
        QualitySettings.SetQualityLevel(index, true);
        SetVolume();

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            //PostProcessLayer postProcessLayer = FindAnyObjectByType<PostProcessLayer>();
            //if (postProcessLayer != null) 
            //    postProcessLayer.enabled = pss;            
        }

        if (SceneManager.GetActiveScene().buildIndex >= 3)
        {
            //_game.Hub.Camera.GetComponent<PostProcessLayer>().enabled = pss;                    
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _volume = FindObjectOfType<Volume>();
        SetVolume();
    }

    private void SetVolume()
    {
        if (_volume == null)
            return;

        int level = PlayerPrefs.GetInt("QualityLevel", 1);
        _volume.gameObject.SetActive(level > 0);        
    }

}
