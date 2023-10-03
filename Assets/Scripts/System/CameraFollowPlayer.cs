using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
	public Transform player; // Reference to the player's transform
	public Vector3 offset;   // Offset between the camera and the player

	void Update()
	{
		if (player != null)
		{
			// Update the camera's position to follow the player with the specified offset
			transform.position = player.position + offset;
		}
	}
}
