using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarInfo : MonoBehaviour
{
    [SerializeField] private TuningType _type;
    [SerializeField] private Image _progress;
    [SerializeField] private TMP_Text _indicator;    

    public void Set(float value, int current)
    {
        _progress.fillAmount = value;
        _indicator.text = current.ToString("f0");        
    }
}
