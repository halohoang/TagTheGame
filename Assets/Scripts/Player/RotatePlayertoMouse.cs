using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayertoMouse : MonoBehaviour
{
    // Variables
    private Vector3 _mousePos;
   [SerializeField] private Camera _camera;
    private Rigidbody2D _rb2D;

    //Functions
    void Start()
    {
        _rb2D =GetComponent<Rigidbody2D>();
        _camera = Camera.main; // Gets the main camera
    }

	// Functions
	void Update()
	{
		RotateCamera();
	}
	void RotateCamera()
	{
		_mousePos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - _camera.transform.position.z));
		_rb2D.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((_mousePos.y - transform.position.y), (_mousePos.x - transform.position.x)) * Mathf.Rad2Deg);
	}
}
