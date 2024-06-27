using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GoggleLightControl : MonoBehaviour
{
    // Variables
    private Light2D _light2D;
    [SerializeField] private float _lightOuterRadiusChangeValue;
    [SerializeField] private Light2D _backLight;

    // Functions
    void Start()
    {
        _light2D = GetComponent<Light2D>();

	}

    // Update is called once per frame
    void Update()
    {
        LightControl();
        if (_light2D.pointLightOuterRadius < 2) { _backLight.pointLightInnerAngle = 360f; _backLight.pointLightOuterAngle = 360; }
        else { _backLight.pointLightOuterAngle = 320.33f; _backLight.pointLightInnerAngle = 320.33f; } 
    }
    void LightControl()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            _light2D.pointLightOuterRadius += _lightOuterRadiusChangeValue;
            _light2D.pointLightOuterRadius = Mathf.Clamp(_light2D.pointLightOuterRadius, 1f, 6.440991f);

		}
		else if (Input.mouseScrollDelta.y < 0)
        {
            _light2D.pointLightOuterRadius -= _lightOuterRadiusChangeValue;
			_light2D.pointLightOuterRadius = Mathf.Clamp(_light2D.pointLightOuterRadius, 1f, 6.440991f);

		}
	}
}
