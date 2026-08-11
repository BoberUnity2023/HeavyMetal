using System;
using UnityEngine;

public class ControllerUI : MonoBehaviour
{
    private bool _hasSelected;

    public event Action OnNavigationStart;

    public void NavigationEnd()
    {
        _hasSelected = false;
    }
    
    private void Update()
    {
        bool anyKey = Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Mathf.Abs(Input.GetAxis("Vertical")) > 0.01f;

        if (anyKey && !_hasSelected)
        {
            _hasSelected = true;            
            OnNavigationStart?.Invoke();            
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.JoystickButton0)
            )
            NavigationEnd();
    }
}
