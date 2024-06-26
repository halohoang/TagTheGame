using EnemyPerception;
using UnityEditor;
using UnityEngine;

// see YT-Tuttorial: https://youtu.be/j1-OyLo77ss?t=1095 by 'Comp-3 Interaction'

[CustomEditor(typeof(VisualPerception))]
public class EnemyFieldOfViewVisualization_Editor : Editor
{
    private void OnSceneGUI()
    {       
        VisualPerception visPerc = (VisualPerception)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(visPerc.transform.position, -Vector3.forward, Vector3.right, 360.0f, visPerc.FOVRadius);

        Vector3 viewAngle01 = DirectionFromAngle(visPerc.transform.eulerAngles.z, visPerc.FOVAngle * 0.5f);
        Vector3 viewAngle02 = DirectionFromAngle(visPerc.transform.eulerAngles.z, -visPerc.FOVAngle * 0.5f);

        Handles.color = Color.yellow;
        Handles.DrawLine(visPerc.transform.position, visPerc.transform.position + viewAngle01 * visPerc.FOVRadius);
        Handles.DrawLine(visPerc.transform.position, visPerc.transform.position + viewAngle02 * visPerc.FOVRadius);

        if (visPerc.IsTargetDetected)
        {
            Handles.color = Color.green;
            Handles.DrawLine(visPerc.transform.position, visPerc.TargetObject.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerZ, float angleInDegrees)
    {
        angleInDegrees += eulerZ;

        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0.0f);
    }
}
