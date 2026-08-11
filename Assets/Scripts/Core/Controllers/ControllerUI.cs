using System;
using UnityEngine;

public class ControllerUI : MonoBehaviour
{
    private bool _hasSelected;

    public event Action OnNavigationStart;

    public void NavigationEnd()
    {
        _hasSelected = false;
        Debug.LogWarning("UI Navigation End");
        //enabled = true;
    }

    
    private void Update()
    {
        bool anyKey = Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Mathf.Abs(Input.GetAxis("Vertical")) > 0.01f;

        if (anyKey && !_hasSelected)
        {
            _hasSelected = true;
            //enabled = false;
            OnNavigationStart?.Invoke();
            Debug.LogWarning("UI Navigation Start");
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.JoystickButton0) ||
            Input.GetKeyDown(KeyCode.JoystickButton8) ||
            Input.GetKeyDown(KeyCode.JoystickButton9))
            NavigationEnd();
    }
}
