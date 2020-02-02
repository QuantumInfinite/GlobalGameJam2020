using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyPositionLink : MonoBehaviour
{
    public ReturnToHome[] fracturePieces;
    public Transform[] fractureTargets;
    public bool doIt = false;

    private void OnValidate()
    {
        if (doIt == false)
        {
            return;
        }
        doIt = false;
        if (fracturePieces.Length > 0 && fracturePieces.Length == fractureTargets.Length)
        {
            for (int i = 0; i < fracturePieces.Length; i++)
            {
                Debug.Log("Setting: " + fracturePieces[i].name + "'s goal to: " + fractureTargets[i].name);
                fracturePieces[i].goalOverride = fractureTargets[i];
            }
        }
    }
}
