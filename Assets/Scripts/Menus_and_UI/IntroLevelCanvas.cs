using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLevelCanvas : MonoBehaviour
{
    // Variables
    [SerializeField] float _fadeoutTimer;
    // Functions

    void Update()
    {
        if (_fadeoutTimer > 0)
        {
			_fadeoutTimer -= Time.deltaTime;
			if (_fadeoutTimer <= 0)
            {
				gameObject.SetActive(false);
			}
		}
    }
}
