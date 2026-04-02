using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceStarterUI : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private TMP_Text _clockText = null;
    [SerializeField] private Image _numerImage = null;
    [SerializeField] private Sprite[] _numerSprites = new Sprite[4];

    [SerializeField] private GameObject[] reds = null;
    [SerializeField] private GameObject[] greens = null;

    private void Start()
    {
        _hub.RaceStarter.OnTimer += RaceStarter_OnTimer;
        _hub.RaceStarter.OnStartRace += RaceStarter_OnStartRace;

        _clockText.text = "";
        //_numerImage.sprite = null;
    }

    private void OnDestroy()
    {
        _hub.RaceStarter.OnTimer -= RaceStarter_OnTimer;
        _hub.RaceStarter.OnStartRace -= RaceStarter_OnStartRace;
    }

    private void RaceStarter_OnTimer(int value)
    {
        if (value == 3)
        { 
            ClockOn();
            Show(3);
        }

        if (value == 2)
            Show(2);

        if (value == 1)
            Show(1);

        if (value == 0)
            Show0();
    }

    private void RaceStarter_OnStartRace()
    {
        ClockOff();
    }

    public void ClockOn()
    { 
        //if (clockText != null)
        //{
        //    //Если была нажата пауза

        //    foreach (var red in reds)
        //    {
        //        red.gameObject.SetActive(false);
        //    }
        //    foreach (var green in greens)
        //    {
        //        green.gameObject.SetActive(false);
        //    }
        //}

        _clockText.text = "3";
        //_numerImage.sprite = _numerSprites[3];  
    }

    private void Show(int value)
    {
        _clockText.text = value.ToString();
        //_numerImage.sprite = _numerSprites[value];
        
        foreach (var red in reds)
        {
            red.gameObject.SetActive(true);
        }
    }

    private void Show0()
    {
        _clockText.text = "0";
        //_numerImage.sprite = _numerSprites[0];
        
        foreach (var red in reds)
        {
            red.gameObject.SetActive(false);
        }
        foreach (var green in greens)
        {
            green.gameObject.SetActive(true);
        }
    }

    private void ClockOff()
    {        
        _clockText.text = "";
        _clockText.gameObject.SetActive(false);
        //_numerImage.sprite = null;
        //_numerImage.gameObject.SetActive(false);

        foreach (var red in reds)
        {
            red.gameObject.SetActive(false);
        }
        foreach (var green in greens)
        {
            green.gameObject.SetActive(false);
        }
        //clock.SetActive(false);
    }
}
