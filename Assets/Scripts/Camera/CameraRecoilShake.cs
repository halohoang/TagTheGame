using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoilShake : MonoBehaviour
{
	private Transform cameraTransform;
	private Vector3 originalPosition; // Original position of the camera

	[SerializeField] private float maxShakeDuration = 0.5f; // Maximum shake duration
	[SerializeField] private float maxShakeAmount = 0.2f; // Maximum shake amount

	private float currentShakeDuration = 0f; // Current shake duration
	private float currentShakeAmount = 0f; // Current shake amount

	private void Awake()
	{
		cameraTransform = GetComponent<Transform>();
	}

	public void StartShake(float duration, float amount)
	{
		currentShakeDuration = Mathf.Clamp(duration, 0f, maxShakeDuration);
		currentShakeAmount = Mathf.Clamp(amount, 0f, maxShakeAmount);

		originalPosition = cameraTransform.localPosition;
		StartCoroutine(PerformShake());
	}

	private IEnumerator PerformShake()
	{
		float elapsedTime = 0f;

		while (elapsedTime < currentShakeDuration)
		{
			float normalizedTime = elapsedTime / currentShakeDuration;
			Vector3 randomOffset = Random.insideUnitSphere * Mathf.Lerp(0f, currentShakeAmount, normalizedTime);
			cameraTransform.localPosition = originalPosition + randomOffset;

			elapsedTime += Time.deltaTime;

			yield return null;
		}

		cameraTransform.localPosition = originalPosition;
	}
}