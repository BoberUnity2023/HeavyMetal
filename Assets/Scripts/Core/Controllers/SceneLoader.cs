using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameController _game;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private float[] _fillTimes;
    private float _loadingTime;
    private int _loadingScene;
    private bool _gameReadyApi;

    void Start()
    {
        //if (_loadByStart)
        //    LoadScene(SceneIndex);
        //else
        //    _loadingScreen.Hide();
    }

    private void OnEnable()
    {        
        SceneManager.sceneLoaded += OnSceneLoaded;        
    }

    private void OnDisable()
    {        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDestroy()
    {        
        string key = "LoadingTime" + _loadingScene.ToString();
        PlayerPrefs.SetFloat(key, _loadingTime);
    }
    
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadGameSceneAsync2(sceneIndex));
        _loadingScreen.Show();
    }

    public IEnumerator LoadGameSceneAsync(int sceneIndex)
    {        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            _loadingScreen.SetProgress(operation.progress);            
            yield return null;
        }  
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);
    }    

    private IEnumerator LoadGameSceneAsync2(int sceneIndex)
    {
        bool logged = false;
        _loadingScene = sceneIndex;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        _loadingTime = 0f;        

        while (operation.progress < 0.9f || _loadingTime <= _fillTimes[sceneIndex] * 0.8)
        {            
            _loadingScreen.SetProgress(_loadingTime / _fillTimes[sceneIndex]);

            if (_loadingTime <= _fillTimes[sceneIndex] * 0.8)
            { 
                _loadingTime += Time.deltaTime; 
            }

            if (!logged)
            {
                logged = true;
                Debug.Log("Scene " + _loadingScene + " Loading time real: " + _loadingTime);
            }

            yield return null;
        }
        
        while (SceneManager.GetActiveScene().buildIndex != sceneIndex)
            yield return new WaitForSeconds(0.5f);
        
        while (_loadingTime <= _fillTimes[sceneIndex])
        {
            _loadingScreen.SetProgress(_loadingTime / _fillTimes[sceneIndex]);
            _loadingTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Scene " + _loadingScene + " Loading time full: " + _loadingTime);
        _loadingScreen.SetProgress(1);
        _loadingScreen.Hide();  
    }
}
