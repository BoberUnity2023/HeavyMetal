using UnityEngine;
using System.Collections.Generic;

public class GadgetManager : MonoBehaviour
{
    public enum GadgetSlots {
        Left,
        Right,
        Back
    }

    public List<Transform> slots = new();

    Gadget gadgetLeft = null;
    Gadget gadgetRight = null;

    public void AddGadget(GadgetSlots slot, GameObject prefab, int level)
    {
        if (prefab != null && (int)slot < slots.Count)
        {
            Transform slotTransform = slots[(int)slot];
            GameObject gameObject = Instantiate(prefab, slotTransform);
            if (gameObject != null)
            {
                Gadget gadget = gameObject.GetComponent<Gadget>();
                if(gadget != null) 
                {
                    gadget.setLevel(level);
                    switch (slot)
                    {
                        case GadgetSlots.Left: gadgetLeft = gadget; break;
                        case GadgetSlots.Right: gadgetRight = gadget; break;
                        default: break;
                    }
                }
            }
        }
    }

    public void activateLeft()
    {
        if(gadgetLeft.isReady())
        {
            gadgetLeft.Fire();
        }
    }

    public void activateRight()
    {
        if (gadgetRight.isReady())
        {
            gadgetRight.Fire();
        }
    }
}
