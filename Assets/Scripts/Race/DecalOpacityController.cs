using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DecalOpacityController : MonoBehaviour
{
    [SerializeField] private DecalProjector[] decalProjectors;
    private int _decalCount;

    public void Init(Car _car)
    {
        //_car = car;
        _decalCount = decalProjectors.Length;
        SetOpacity(0);        
    }
    /// <summary>
    /// »змен€ет свойство Global Opacity (fadeFactor) у HDRP Decal Projector.
    /// </summary>
    /// <param name="opacity">«начение прозрачности от 0.0 (полностью прозрачный) до 1.0 (непрозрачный).</param>
    public void SetOpacity(float opacity)
    {
        float clampedOpacity = Mathf.Clamp01(opacity);
        //Debug.LogWarning("SetOpacity: " + opacity);
        for (int i = 0; i < _decalCount; i++)
        {
            DecalProjector decal = decalProjectors[i];
            if (decal == null)
            {
                Debug.LogWarning("DecalProjector не назначен в скрипте DecalOpacityController!", this);
                continue;
            }

            // ¬ HDRP Decal Projector за прозрачность отвечает свойство fadeFactor
            decal.fadeFactor = Mathf.Clamp01(opacity);
        }
    }
}
