using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DecalOpacityController : MonoBehaviour
{
    [SerializeField] private DecalProjector[] decalProjectors;

    public void Init(Car _car)
    {
        //_car = car;
        SetOpacity(0);
    }
    /// <summary>
    /// »змен€ет свойство Global Opacity (fadeFactor) у HDRP Decal Projector.
    /// </summary>
    /// <param name="opacity">«начение прозрачности от 0.0 (полностью прозрачный) до 1.0 (непрозрачный).</param>
    public void SetOpacity(float opacity)
    {
        Debug.LogWarning("SetOpacity: " + opacity);
        foreach (var decal in decalProjectors)
        {
            if (decal == null)
            {
                Debug.LogWarning("DecalProjector не назначен в скрипте DecalOpacityController!", this);
                return;
            }

            // ¬ HDRP Decal Projector за прозрачность отвечает свойство fadeFactor
            decal.fadeFactor = Mathf.Clamp01(opacity);
        }
    }
}
