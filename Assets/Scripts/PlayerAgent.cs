using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerAgent : MonoBehaviour
{
    CarController carController = null;
    GadgetManager gadgetManager = null;

    void Start()
    {
        carController = gameObject.GetComponent<CarController>();
        gadgetManager = gameObject.GetComponent<GadgetManager>();
    }

    void Update()
    {
        if (gadgetManager != null)
        {
           //if (Input.GetKeyDown(KeyCode.E))
           //{
           //    gadgetManager.activateRight();
           //}
           //
           //if (Input.GetKeyDown(KeyCode.Q))
           //{
           //    gadgetManager.activateLeft();
           //}
        }

        if (carController != null)
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                carController.setMass(carController.getMass() + 100);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                carController.setMass(carController.getMass() - 100);
            }

            if (Input.GetKey(KeyCode.W))
            {
                carController.SetThrottle(1.0f);
            }
            if (Input.GetKey(KeyCode.S))
            {
                carController.SetThrottle(-1.0f);
            }

            if (Input.GetKey(KeyCode.A))
            {
                carController.SetSteeringAxis(-1.0f);
            }
            if (Input.GetKey(KeyCode.D))
            {
                carController.SetSteeringAxis(1.0f);
            }

            if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            {
                carController.SetThrottle(0.0f);
            }
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                carController.SetSteeringAxis(0.0f);
            }
        }
    }
}
