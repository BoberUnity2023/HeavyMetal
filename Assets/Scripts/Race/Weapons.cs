using System;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    [SerializeField] private Weapon[] _weapons;

    public Weapon[] Guns => _weapons;
}
