using UnityEngine;
using UnityEngine.UI;

public class RaceStarterUI : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private Text clockText = null;
    [SerializeField] private Image numerImage = null;
    [SerializeField] private Sprite[] numerSprites = new Sprite[4];

    [SerializeField] private GameObject[] reds = null;
    [SerializeField] private GameObject[] greens = null;

    private void Start()
    {
        _hub.RaceStarter.OnTimer += RaceStarter_OnTimer;
        _hub.RaceStarter.OnStartRace += RaceStarter_OnStartRace;

        clockText.text = "";
        numerImage.sprite = null;
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

        clockText.text = "3";
        numerImage.sprite = numerSprites[3];  
    }

    private void Show(int value)
    {
        clockText.text = value.ToString();
        numerImage.sprite = numerSprites[value];
        
        foreach (var red in reds)
        {
            red.gameObject.SetActive(true);
        }
    }

    private void Show0()
    {
        clockText.text = "0";
        numerImage.sprite = numerSprites[0];
        
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
        clockText.text = "";
        clockText.gameObject.SetActive(false);
        numerImage.sprite = null;
        numerImage.gameObject.SetActive(false);

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
