using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnHoverHoang : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	// Variables
	[SerializeField] List<GameObject> Roles;
	[SerializeField] List<GameObject> ObjectHide;
	//Functions
	public void OnPointerEnter(PointerEventData eventData)
	{
		foreach (GameObject obj in Roles)
		{
			obj.SetActive(true);
		}
		foreach (GameObject obj in ObjectHide)
		{
			obj.SetActive(false);
		}
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		foreach (GameObject obj in Roles)
		{
			obj.SetActive(false);

		}
		foreach (GameObject obj in ObjectHide)
		{
			obj.SetActive(true);
		}

	}
}
