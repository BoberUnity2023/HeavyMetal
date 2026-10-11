using UnityEngine;

public class AnalitycsAverageFPS : MonoBehaviour
{
    [SerializeField] private SceneController _sceneController;
    private const string _key = "IsSentAverageFPS";
    private int _frames;

    private void Start()
    {
        if (!IsSent)        
            enabled = true;        
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        _frames++;
    }

    public void PressContinue()
    {
        if (!IsSent)
        {
            int averageFPS = (int)(_frames / Time.timeSinceLevelLoad);
            _sceneController.Game.Analitycs.SendAverageFPS(averageFPS);
            IsSent = true;
            Debug.Log(string.Format("SendAverageFPS: {0}", averageFPS));
        }
    }

    private bool IsSent
    {
        get 
        { 
            return PlayerPrefs.GetInt(_key, 0) == 1; 
        }
        set 
        {
            if (value)
                PlayerPrefs.SetInt(_key, 1);
            else
                PlayerPrefs.DeleteKey(_key);

        }
    }
}
