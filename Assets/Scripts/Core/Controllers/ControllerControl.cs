using System;
using UnityEngine;

public class ControllerControl : MonoBehaviour
{
    [SerializeField] private ConfigControl _config;
    [SerializeField] private Hub _hub;    

    private Camera _camera;
    private Vector3 _worldPositionCursor = Vector3.zero;
    //private Vector3 _worldPositionLastClick = Vector3.zero;
    //private bool _cursorOnUI = false;    

    //public event Action<Vector3> OnClickInWorld;

    public ControlType Type => _config.Type;

    public Vector3 WorldPositionCursor
    {
        get { return _worldPositionCursor; }
    }

    //public Vector3 WorldPositionLastClick
    //{
    //    get { return _worldPositionLastClick; }
    //}

         

    #region Unity

    private void Start()
    {
        _camera = Camera.main;
        //_cursor3d.gameObject.SetActive(Type == ControlType.PC);        
    }

    private void Update()
    {
        //if (Type == ControlType.PC)
        //{
        //    //Update_WorldPositionCursor();
        //    //Update_Cursor();
        //}
    }

    #endregion

    private void Update_WorldPositionCursor()
    {
        if (IsCursorOnUI)
            return;

        RaycastHit hit;
        Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 100, _config.Mask);        
        //if (Vector3.Distance(hit.point, _hub.Hero.transform.position) < 5)
        //{
        //    _worldPositionCursor = hit.point;            
        //}        
    }

    private void Update_Cursor()
    {
        //float distanceToCursor = Vector3.Distance(_hub.Hero.transform.position, _hub.Control.WorldPositionCursor);
        
        //if (AngleToCursor > _hub.Hero.Move.AngleMoveBack)
        //{
        //    _cursor3d.SetPositionType(PositionType.Back);
        //    return;
        //}

        //if (AngleToCursor > _hub.Hero.Move.AngleMoveForwardMax)
        //{
        //    _cursor3d.SetPositionType(PositionType.Hard);
        //    return;
        //}

        //if (AngleToCursor > _hub.Hero.Move.AngleMoveForwardMax/2 || distanceToCursor > 2 || distanceToCursor < 0.4f)
        //{
        //    _cursor3d.SetPositionType(PositionType.Normal);
        //    return;
        //}
            
    }

    private bool IsCursorOnUI
    {
        get 
        {
            return IsOnUI(Input.mousePosition);
        }        
    }

    public bool IsOnUI(Vector2 position)
    {
        float bannerHeight = Screen.height * ButtonJumpSize;

        if (position.x < Screen.height * ButtonJumpSize && position.y < Screen.height * ButtonJumpSize)
            return true;

        if (position.x < Screen.height * ButtonJumpSize && position.y > Screen.height - Screen.height * ButtonJumpSize - bannerHeight)
            return true;

        return false;
    }

    float ButtonJumpSize
    {
        get
        {
            if (_hub.Screen.AspectRatio > 2.5f)
                return 0.33f;
            if (_hub.Screen.AspectRatio > 2.0f)
                return 0.26f;
            if (_hub.Screen.AspectRatio > 1.5f)
                return 0.24f;
            if (_hub.Screen.AspectRatio > 1.0f)
                return 0.2f;
            if (_hub.Screen.AspectRatio > 0.75f)
                return 0.17f;
            if (_hub.Screen.AspectRatio > 0.5f)
                return 0.15f;

            return 0.17f;
        }
    }    
}
