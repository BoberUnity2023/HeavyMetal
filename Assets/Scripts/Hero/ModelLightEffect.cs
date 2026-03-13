using System.Collections;
using UnityEngine;

public class ModelLightEffect : MonoBehaviour
{    
    [SerializeField] private Material _effectAddBrickMaterial;
    [SerializeField] private RuntimeAnimatorController _effectAddBrickRuntimeAnimatorController;
    protected const float _effectTime = 2.0f;


    /// <summary>
    /// Добавляет эфффект и через _effectTime/2 активирует/деактивирует объект
    /// </summary>
    /// <param name="model"></param>
    /// <param name="value">false - off, true - on</param>
    protected void AddModelLightEffect(GameObject model, bool value)
    {
        ModelLightEffectStart(model);
        StartCoroutine(ModelSetActiveCoroutine(model, _effectTime / 2, value));
    }

    private void ModelLightEffectStart(GameObject model)
    {
        Vector3 pos = model.transform.position;
        Quaternion rot = model.transform.rotation;
        GameObject modelLight = Instantiate(model, pos, rot, model.transform.parent);
        modelLight.SetActive(true);
        //modelLight.transform.localScale = model.transform.localScale * 1.03f;
        MeshRenderer meshRenderer = modelLight.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material = _effectAddBrickMaterial;
            Animator animator = modelLight.AddComponent<Animator>();
            animator.runtimeAnimatorController = _effectAddBrickRuntimeAnimatorController;
            //Debug.Log("ModelLightEffectStart(" + model.gameObject.name + ") created success");
        }
        else
            Debug.LogError("Error: ModelLightEffectStart(" + model.gameObject.name + ") has no MeshRenderer");        
    }

    private IEnumerator ModelSetActiveCoroutine(GameObject model, float time, bool value)
    {
        yield return new WaitForSeconds(time);
        model.SetActive(value);
    }
}
