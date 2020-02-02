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

    private int needToComplete;

    public void OnExitPlayZone(Grabbable grab)
    {
        grab.transform.position = grab.startLocation;
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
