using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartClock : MonoBehaviour
{
    //[SerializeField] private CarCameras cameraLevel = null;
    [SerializeField] private Text clockText = null;
    [SerializeField] private Image numerImage = null;
    [SerializeField] private Sprite[] numerSprites = new Sprite[4];

    [SerializeField] private GameObject[] reds = null;
    [SerializeField] private GameObject[] greens = null;
    /*private float yawAngle = 0;
    private float height = 0;
    private float distance = 0;
    private float pitchAngle = 0; */
    private bool isCameraMoved = false;

    private void Start()
    {
        clockText.text = "";
        numerImage.sprite = null;
        ClockOn();
        if (clockText != null)
        {
            //Если была нажата пауза

            foreach (var red in reds)
            {
                red.gameObject.SetActive(false);
            }
            foreach (var green in greens)
            {
                green.gameObject.SetActive(false);
            }
        }
    }


    public void ClockOn()
    {
        isCameraMoved = true;

        clockText.text = "3";
        numerImage.sprite = numerSprites[3];
        StartCoroutine(Show2(0.5f));//должно быль 1 и RaceStart 4 s

    }

    private IEnumerator Show2(float time)
    {
        yield return new WaitForSeconds(time);

        clockText.text = "2";
        numerImage.sprite = numerSprites[2];
        StartCoroutine(Show1(0.5f));//должно быль 1 и RaceStart 4 s
        foreach (var red in reds)
        {
            red.gameObject.SetActive(true);
        }
    }

    private IEnumerator Show1(float time)
    {
        yield return new WaitForSeconds(time);
        clockText.text = "1";
        numerImage.sprite = numerSprites[1];
        StartCoroutine(Show0(0.5f));//должно быль 1 и RaceStart 4 s        
    }

    private IEnumerator Show0(float time)
    {
        yield return new WaitForSeconds(time);
        clockText.text = "0";
        numerImage.sprite = numerSprites[0];
        StartCoroutine(ClockOff(0.5f));//должно быль 1 и RaceStart 4 s
        foreach (var red in reds)
        {
            red.gameObject.SetActive(false);
        }
        foreach (var green in greens)
        {
            green.gameObject.SetActive(true);
        }
    }

    private IEnumerator ClockOff(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnCameraPosition();
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

    private void ReturnCameraPosition()//+ При нажатии button Pause
    {
        if (isCameraMoved)
        {
            //cameraLevel.yawAngle = yawAngle;
            //cameraLevel.height = height;
            //cameraLevel.distance = distance;
            //cameraLevel.pitchAngle = pitchAngle;
            isCameraMoved = false;
        }
    }
}
