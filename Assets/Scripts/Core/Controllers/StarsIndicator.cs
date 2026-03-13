using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarsIndicator : MonoBehaviour
{
    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private TMP_Text _indicator;
    
    private void OnEnable()
    {
        //_indicator.text = _mainMenuController.Game.Saves.Stars.ToString();
    }
}
