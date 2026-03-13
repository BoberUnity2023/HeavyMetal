using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSoundLevel : MonoBehaviour
{
    [SerializeField] private Hub _hub;

    public void PressSound()
    {
        _hub.Game.Sound.Play(SoundClip.Click);
    }
}
