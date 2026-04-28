using UnityEngine;

public class CarPaint : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] _renderers;
    private Car _car;

    public void Init(Car car)
    {
        _car = car;
        if (!car.IsAI)
        {
            int colorId = _car.Hub.Game.Saves.GetCarColor(car.CarType);
            int selectedCar = _car.Hub.Game.SelectedCar;
            Material material = _car.Hub.Game.ConfigGame.Cars[selectedCar].Tuning.CarColors[colorId].Material;
            SetMaterial(material);
        }
    }

    public void SetMaterial(Material material)
    {
        foreach (var renderer in _renderers)
        {
            renderer.material = material;
        }        
    }
}
