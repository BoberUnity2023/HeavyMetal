using DG.Tweening;
using UnityEngine;

public class CameraMovier : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _positionMenu;
    [SerializeField] private Transform _positionCar;

    public void SetToCups()
    {
        _camera.transform.SetLocalPositionAndRotation(_positionMenu.position, _positionMenu.rotation);
    }

    public void MoveToCar()
    {
        _camera.transform.DOMove(_positionCar.position, 1);
        _camera.transform.DORotate(_positionCar.eulerAngles, 1);
    }

    public void MoveToCups()
    {
        _camera.transform.DOMove(_positionMenu.position, 1);
        _camera.transform.DORotate(_positionMenu.eulerAngles, 1);
    }
}
