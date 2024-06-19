using StateMashine;
using UnityEditor;
using UnityEngine;

// see YT-Tuttorial: https://youtu.be/j1-OyLo77ss?t=1095 by 'Comp-3 Interaction'

[CustomEditor(typeof(ConditionPlayerDetectionCheck))]
public class OldEnemyFieldOfViewVisualization_Editor : Editor
{
    private void OnSceneGUI()
    {
        ConditionPlayerDetectionCheck playerDetectionCheck = (ConditionPlayerDetectionCheck)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(playerDetectionCheck.transform.position, -Vector3.forward, Vector3.right, 360.0f, playerDetectionCheck.FOVRadius);

        Vector3 viewAngle01 = DirectionFromAngle(playerDetectionCheck.transform.eulerAngles.z, playerDetectionCheck.FOVAngle * 0.5f);
        Vector3 viewAngle02 = DirectionFromAngle(playerDetectionCheck.transform.eulerAngles.z, -playerDetectionCheck.FOVAngle * 0.5f);

        Handles.color = Color.yellow;
        Handles.DrawLine(playerDetectionCheck.transform.position, playerDetectionCheck.transform.position + viewAngle01 * playerDetectionCheck.FOVRadius);
        Handles.DrawLine(playerDetectionCheck.transform.position, playerDetectionCheck.transform.position + viewAngle02 * playerDetectionCheck.FOVRadius);

        if (playerDetectionCheck.IsPlayerDetected)
        {
            Handles.color = Color.green;
            Handles.DrawLine(playerDetectionCheck.transform.position, playerDetectionCheck.PlayerObj.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerZ, float angleInDegrees)
    {
        angleInDegrees += eulerZ;

        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0.0f);
    }
}
