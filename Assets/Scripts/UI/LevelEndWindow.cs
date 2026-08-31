using System.Collections;
using TMPro;
using UnityEngine;

public class LevelEndWindow : WindowBase
{
    [SerializeField] private Hub _hub;    
    [SerializeField] private TMP_Text _finishText;
    [SerializeField] private TMP_Text _indicatorAddCoins;    
    [SerializeField] private GameObject[] _stars;    

    protected override void Start()
    {
        Init(_hub.Game);
        base.Start();
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
        _hub.Game.UI.Finish();
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

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _hub.Level.Race.OnFinish -= Race_OnFinish;
    }
}
