using UnityEngine;
using UnityEngine.UI;

public class ButtonCarColor : MonoBehaviour
{
    [SerializeField] private Button _button;
    private ColorPanel _colorPanel;
    private int _id;
    private Color _color;
    private Material _material;

    public void Init(int id, Color color, Material material, ColorPanel colorPanel)
    {
        _id = id;
        _color = color;
        _material = material;
        _colorPanel = colorPanel;
        _button.image.color = color;
    }

    public void OnPress()
    {
        _colorPanel.PressColor(_material);
    }
}
