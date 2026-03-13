using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private GameObject _blastPrefab;
    [SerializeField] private GameObject _blastSmallPrefab;
    [SerializeField] private GameObject _blastFrogPrefab;
    [SerializeField] private GameObject _ringsPrefab;
    [SerializeField] private GameObject _ringPrefab;
    private bool _isHeroInWater;
    private bool _isInited;

    private void Start()
    {
        StartCoroutine(Init(0.5f));
    }

    private void FixedUpdate()
    {
        //if (_hub.Hero.transform.position.y < transform.position.y && !_isHeroInWater)
        //{
        //    _isHeroInWater = true;
        //    OnHeroEnter();
        //}

        //if (_hub.Hero.transform.position.y > transform.position.y)
        //    _isHeroInWater = false;
    }

    private void OnTriggerEnter(Collider other)        
    {
        if (!_isInited)
            return;

        if (other.gameObject.name == "Hero")
        {
            if (_isHeroInWater)
                return;

            //Debug.Log("Water + Hero: " + _hub.Hero.CharacterController.velocity.magnitude);
            OnHeroEnter();
        }
        else
        {
            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            if (rigidbody == null)
                return;
            
            GameObject _prefab = Prefab(rigidbody.linearVelocity.magnitude);            

            if (other.gameObject.name.Contains("Frog"))
                _prefab = _blastFrogPrefab;

            if (other.gameObject.name.Contains("Wall"))
                _prefab = _blastSmallPrefab;

            CreateEffect(_prefab, other.transform.position);
        }
    }

    private IEnumerator Init(float time)
    {
        yield return new WaitForSeconds(time);
        _isInited = true;
    }

    private void OnHeroEnter()
    {        
        //GameObject prefab = Prefab(_hub.Hero.CharacterController.velocity.magnitude);
        //CreateEffect(prefab, _hub.Hero.transform.position);
        StartCoroutine(TryCreateRings());
    }

    private void CreateEffect(GameObject prefab, Vector3 position)
    {
        position.y = transform.position.y + 0.02f;
        GameObject effect = Instantiate(prefab, position, Quaternion.identity);        
        Destroy(effect, 3);
    }

    private IEnumerator TryCreateRings()
    {
        //float time = _hub.Hero.Move.CanMove ? 0.4f : 1;
        yield return new WaitForSeconds(1);
        //if (_hub.Hero.transform.position.y < transform.position.y)
        //{
        //    GameObject prefab = Prefab(_hub.Hero.CharacterController.velocity.magnitude);
        //    CreateEffect(prefab, _hub.Hero.transform.position);
        //    StartCoroutine(TryCreateRings());
        //}
    }

    private GameObject Prefab(float force)
    {
        if (force > 3.5f)
            return _blastPrefab;

        if (force > 1.5f)
            return _ringsPrefab;

        return _ringPrefab;
    }
}
