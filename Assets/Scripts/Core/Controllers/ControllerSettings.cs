using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class ControllerSettings : MonoBehaviour
{
    [SerializeField] private GameController _game;   

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

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            PostProcessLayer postProcessLayer = FindAnyObjectByType<PostProcessLayer>();
            if (postProcessLayer != null) 
                postProcessLayer.enabled = pss;            
        }

        if (SceneManager.GetActiveScene().buildIndex >= 2)
        {
            _game.Hub.Camera.GetComponent<PostProcessLayer>().enabled = pss;                    
        }
    }
}
