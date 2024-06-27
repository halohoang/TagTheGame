using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
	// Variables
	internal Light2D _light2D;
	[SerializeField] float _minIntensity;
	[SerializeField] float _maxIntensity;
	[SerializeField] float _changeSpeed;
	float _originalIntensity;
	float _targetIntensity;

	// Functions
	void Start()
	{
		_light2D = GetComponent<Light2D>();
		_originalIntensity = _light2D.intensity; // Set the current intensity to the light's intensity
		_targetIntensity = _originalIntensity; // Set the target intensity to the current intensity
		_targetIntensity = Mathf.Clamp(_targetIntensity, _minIntensity, _maxIntensity); // Clamp the intensity
		StartCoroutine(Flicker()); // Start the flicker coroutine

	}

	void Update()
	{
		_light2D.intensity = Mathf.Lerp(_light2D.intensity, _targetIntensity, _changeSpeed * Time.deltaTime); // Lerp the intensity to the target intensity
	}
	IEnumerator Flicker()
	{
		while (true)
		{
			_targetIntensity = Random.Range(_minIntensity, _maxIntensity);
			yield return new WaitForSeconds(1 / _changeSpeed);
		}
	}
}
