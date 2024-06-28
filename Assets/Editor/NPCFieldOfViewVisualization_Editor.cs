using Perception;
using UnityEditor;
using UnityEngine;

// see YT-Tuttorial: https://youtu.be/j1-OyLo77ss?t=1095 by 'Comp-3 Interaction'

[CustomEditor(typeof(NPCPerception))]
public class NPCFieldOfViewVisualization_Editor : Editor
{
    private void OnSceneGUI()
    {
        NPCPerception nPCVisPerc = (NPCPerception)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(nPCVisPerc.transform.position, -Vector3.forward, Vector3.right, 360.0f, nPCVisPerc.FOVRadius);

        Vector3 viewAngle01 = DirectionFromAngle(nPCVisPerc.transform.eulerAngles.z, nPCVisPerc.FOVAngle * 0.5f);
        Vector3 viewAngle02 = DirectionFromAngle(nPCVisPerc.transform.eulerAngles.z, -nPCVisPerc.FOVAngle * 0.5f);

        Handles.color = Color.yellow;
        Handles.DrawLine(nPCVisPerc.transform.position, nPCVisPerc.transform.position + viewAngle01 * nPCVisPerc.FOVRadius);
        Handles.DrawLine(nPCVisPerc.transform.position, nPCVisPerc.transform.position + viewAngle02 * nPCVisPerc.FOVRadius);

        if (nPCVisPerc.IsTargetDetected)
        {
            Handles.color = Color.green;
            Handles.DrawLine(nPCVisPerc.transform.position, nPCVisPerc.TargetObject.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerZ, float angleInDegrees)
    {
        angleInDegrees += eulerZ;

        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0.0f);
    }
}
