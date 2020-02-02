using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyPositionLink : MonoBehaviour
{
    public ReturnToHome[] fracturePieces;
    public Transform[] fractureTargets;
    public Material lineRenderMat;
    public Material hologramMat;
    public Color lineRenderColor;
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
                var piece = fracturePieces[i];
                var target = fractureTargets[i];
                Debug.Log("Setting: " + piece.name + "'s goal to: " + target.name);
                piece.goalOverride = target;

                piece.GetComponent<MeshCollider>().convex = true;
                var lineRender = piece.GetComponent<LineRenderer>();
                lineRender.startWidth = 0.2f;
                lineRender.endWidth = 0.2f;
                lineRender.material = lineRenderMat;
                lineRender.startColor = lineRenderColor;

                target.GetComponent<MeshRenderer>().material = hologramMat;
            }
        }
    }
}
