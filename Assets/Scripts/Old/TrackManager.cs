using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public List<Transform> startPoints = new();

    public List<GameObject> carPrefabs = new();
    public List<GameObject> gadetPrefabs = new();

    BoxCollider finishSensor = null;
    Transform playerCar = null;

    void Start()
    {
        finishSensor = GetComponent<BoxCollider>();

        for (int i = 0; i < startPoints.Count; i++)
        {
            if (i == 2)
            {
                GameObject gObj= Instantiate(carPrefabs[1], startPoints[i].position, startPoints[i].rotation);
                playerCar = gObj.transform;
                gObj.AddComponent<PlayerAgent>();
                //CameraFollow.Instance.setTarget(playerCar);

                GadgetManager gadgetManager = gameObject.GetComponent<GadgetManager>();
                if(gadgetManager != null)
                {
                    /*gadgetManager.AddGadget(GadgetManager.GadgetSlots.Left,
                        gadetPrefabs[GarageManager.leftSlotPrefabId()],
                        GarageManager.leftSlotLevel());

                    gadgetManager.AddGadget(GadgetManager.GadgetSlots.Right,
                        gadetPrefabs[GarageManager.rightSlotPrefabId()],
                        GarageManager.rightSlotLevel());*/
                }
            }
            else
            {
                //GameObject gameObject = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Count - 1)], startPoints[i].position, startPoints[i].rotation);
                //gameObject.AddComponent<AIController>();
            }
        }
    }

    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < startPoints.Count; i++)
        {
            Gizmos.DrawSphere(startPoints[i].position, 1.0f);
        }
    }
}
