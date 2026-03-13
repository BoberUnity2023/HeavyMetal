using System;
using UnityEngine;

public enum ScreenOrientation
{
    Horizontal,
    Vertical
}

public class ScreenController : MonoBehaviour
{
    private ScreenOrientation orientation;
    public event Action<ScreenOrientation> OnOrientationChanged;
    //[SerializeField] private float _aspectRatio;//Only for visible

    public float AspectRatio
    {
        get
        {            
            return (float)Screen.width / Screen.height; ;
        }
    }    

    private void Update()
    {
        //_aspectRatio = AspectRatio;
        if (orientation == ScreenOrientation.Horizontal && AspectRatio < 1)
            SetVertical();

        if (orientation == ScreenOrientation.Vertical && AspectRatio > 1)
            SetHorizontal();
    }

    private void SetVertical()
    {
        //Debug.Log("Screen.SetVertical()");
        orientation = ScreenOrientation.Vertical;
        OnOrientationChanged?.Invoke(orientation);
    }

    private void SetHorizontal()
    {
        //Debug.Log("Screen.SetHorizontal()");
        orientation = ScreenOrientation.Horizontal;
        OnOrientationChanged?.Invoke(orientation);
    }
}
