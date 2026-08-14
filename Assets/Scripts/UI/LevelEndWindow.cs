using System.Collections;
using TMPro;
using UnityEngine;

public class LevelEndWindow : MonoBehaviour
{
    [SerializeField] private Hub _hub;    
    [SerializeField] private GameObject _window;
    [SerializeField] private TMP_Text _finishText;
    [SerializeField] private TMP_Text _indicatorAddCoins;    
    [SerializeField] private GameObject[] _stars;

    private void Start()
    {        
        Hide();        
        StartCoroutine(AfterStart(1));
    }

    private IEnumerator AfterStart(float time)
    {
        yield return new WaitForSeconds(time);
        _hub.Level.Race.OnFinish += Race_OnFinish;
    }

    private void Race_OnFinish()
    {
        Show();
        
        _finishText.gameObject.SetActive(true);
        int place = _hub.Result.Place;
        _finishText.text = place.ToString();
        _stars[0].SetActive(place <= 3);
        _stars[1].SetActive(place <= 2);
        _stars[2].SetActive(place == 1);

        if (place <= 3)
        {
            int prize = _hub.Level.Config.FinishCoins[place - 1];
            _indicatorAddCoins.text = "+" + prize;
        }
    }

    private void OnDestroy()
    {
        _hub.Level.Race.OnFinish -= Race_OnFinish;
    }

    public void Show()
    {
        _window.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _window.gameObject.SetActive(false);
    }
}
