using System.Linq;
using UnityEngine;

public class CarInfoPanel : MonoBehaviour
{
    [SerializeField] private WindowSelectCar _windowSelectCar;
    [SerializeField] private BarInfo _barPower;
    [SerializeField] private BarInfo _barMass;
    [SerializeField] private BarInfo _barSpeed;

    private GameController _game;

    public void Init(SceneController sceneController)
    {        
        _game = sceneController.Game;        
    }

    public void Show()
    {
        gameObject.SetActive(true);
        SetBars();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void SetBars()
    {        
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];        
        
        int max = _game.ConfigGame.Cars.Max(p => p.MotorTorque);
        int current = configCar.MotorTorque;
        SetBar(_barPower, current, max);        

        max = _game.ConfigGame.Cars.Max(p => p.Mass);
        current = configCar.Mass;
        SetBar(_barMass, current, max);
        
        max = (int)(_game.ConfigGame.Cars.Max(p => p.MaxSpeed) * 3.6f);
        int min = _game.ConfigGame.Cars.Min(p => p.MaxSpeed);
        current = (int)(configCar.MaxSpeed * 3.6f);
        SetBar(_barSpeed, current, max);
    }

    private void SetBar(BarInfo bar, int current, int max)
    {
        float value = (float)current / max;
        bar.Set(value, current);
    }
}
