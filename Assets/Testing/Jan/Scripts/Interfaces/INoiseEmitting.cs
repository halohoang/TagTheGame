using UnityEngine;

public interface INoiseEmitting
{
    void InformObjectsInNoiseRange(bool isSomethinHappening, Vector3 positionOfEvent, float noiseRange);
}
