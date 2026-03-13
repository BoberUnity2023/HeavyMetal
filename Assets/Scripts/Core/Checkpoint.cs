using DG.Tweening;
using System.Collections;
using UnityEngine;

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
        if (_hub.Level.IsLost)
        {
            Debug.Log("Checkpoint. returns: _hub.Level.IsLost");
            return; 
        }

        int cakesOnPodnos = _hub.Level.CakesOnPodnos;
        if (cakesOnPodnos == 0)
        {
            Debug.Log("Checkpoint. returns: CakesOnPodnos == 0");
            return; 
        }

        //if (other.GetComponent<Hero>() != null)
        //{
        //    StartComplete();
        //}
    }

    public void StartComplete()
    {
        int cakesOnPodnos = _hub.Level.CakesOnPodnos;
        float time = wait ? 1 : 0;
        if (!wait)
            _hub.Game.Sound.Play(SoundClip.Checkpoint);

        StartCoroutine(Complete(time, cakesOnPodnos));
    }

    private IEnumerator Complete(float time, int cakesOnPodnos)
    {
        //TryCorrectStart();
        yield return new WaitForSeconds(time);
        transform.DOScale(0, 1);//.OnComplete(TryCorrectStop);
        _hub.Level.LastCheckpoint = this;
        _hub.Level.CheckpointCakes = cakesOnPodnos;
        //TryCorrectCakes();
        Debug.Log("Checkpoint. CakesOnPodnos: " + cakesOnPodnos);
    }

    public void Restart()
    {
        transform.localScale = Vector3.one;
        wait = true;
    }

    /*private void TryCorrectStart()
    {
        if (_hub.Level.CakesOnPodnos == _hub.Level.Cakes.Count )
        foreach (Cake cake in _hub.Level.Cakes)
        {
            //if (cake.CanCorrect)
            cake.CorrectStart();
        }
    }

    private void TryCorrectStop()
    {

        foreach (Cake cake in _hub.Level.Cakes)
        {
            //if (cake.CanCorrect)
            cake.CorrectStop();
        }
    }*/
}
