using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Papa
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private Hub _hub;
        private bool wait;

        private void Start()
        {
            if (_hub == null)
                _hub = FindObjectOfType<Hub>();
        }

        private void OnTriggerEnter(Collider other)
        {

        }
    }

}
