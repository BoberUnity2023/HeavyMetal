using System.Collections.Generic;
using UnityEngine;

public class ColorPanel : MonoBehaviour
{    
    [SerializeField] private ButtonCarColor _buttonObject;
    private SceneController _sceneController;
    private Garage _garage;
    private List<ButtonCarColor> _buttons = new List<ButtonCarColor>();

    public void Init(SceneController sceneController, Garage garage)
    {
        _sceneController = sceneController;
        _garage = garage;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        CreateButtons();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void CreateButtons()
    {
        Clear();
        ConfigCar configCar = _sceneController.Game.ConfigGame.Cars[_sceneController.Game.SelectedCar];
        int countColors = configCar.Tuning.CarColors.Length;
        for (int i = 0; i < countColors; i++)
        {
            ButtonCarColor button = Instantiate(_buttonObject, transform.position, Quaternion.identity, transform);
            button.gameObject.SetActive(true);
            _buttons.Add(button);
            Color color = configCar.Tuning.CarColors[i].Color;
            Material material = configCar.Tuning.CarColors[i].Material;
            button.Init(i, color, material, this);
        }
    }

    private void Clear()
    {
        foreach (var button in _buttons)
        {
            Destroy(button.gameObject);
        }
        _buttons.Clear();
    }

    public void PressColor(Material material)
    {        
        _garage.CurrentCar.Paint.SetMaterial(material);
    }
}
