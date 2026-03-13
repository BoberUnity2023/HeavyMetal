using UnityEngine;

public enum PositionType
{
    Good,
    Normal,
    Hard,
    Back
}

public class Cursor3d : MonoBehaviour
{
    [SerializeField] private Light _cursor3dLight;
    [SerializeField] private Material _cursorMaterial;
    private PositionType _positionType;    

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetPositionType(PositionType type)
    {
        _positionType = type;
        switch (_positionType)
        {
            case PositionType.Good:
                {
                    _cursor3dLight.color = Color.green;
                    _cursorMaterial.color = Color.green;
                    break;
                }

            case PositionType.Normal:
                {
                    _cursor3dLight.color = Color.yellow;
                    _cursorMaterial.color = Color.yellow;
                    break;
                }

            case PositionType.Hard:
                {
                    _cursor3dLight.color = Color.red;
                    _cursorMaterial.color = Color.red;
                    break;
                }

            case PositionType.Back:
                {
                    _cursor3dLight.color = Color.blue;
                    _cursorMaterial.color = Color.blue;
                    break;
                }
        }
    }
}
