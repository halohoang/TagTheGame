using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActive : MonoBehaviour
{
    // Variables
    [SerializeField] private List<GameObject> _activeObjects;
    // Functions
    public void ActiveObjects()
    {
		foreach (GameObject activeObject in _activeObjects)
        {
			activeObject.SetActive(true);
		}
	}
}
