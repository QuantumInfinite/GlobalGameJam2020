using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<FractureParent> gameEndingFractures;
    public List<FractureParent> fractures;

    public UnityEvent OnGameComplete;

    public ParticleSystem ps_removePiece;
    public ParticleSystem ps_showPiece;

    private int needToComplete;

    public void OnExitPlayZone(Grabbable grab)
    {
        ps_removePiece.gameObject.transform.position = grab.transform.position;
        ps_removePiece.Play();
        grab.rb.velocity = Vector3.zero;
        grab.rb.angularVelocity = Vector3.zero;
        grab.transform.position = grab.startLocation;
        ps_showPiece.gameObject.transform.position = grab.startLocation;
        ps_showPiece.Play();
    }

    public void Start()
    {
        needToComplete = gameEndingFractures.Count;
        foreach(var objects in gameEndingFractures)
        {
            objects.OnSuccess.AddListener(() => CheckCompleted());
        }
    }

    public void CheckCompleted()
    {
        needToComplete--;
        if(needToComplete <= 0)
        {
            OnGameComplete.Invoke();
        }
    }
}
