using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnHoverHoang : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	// Variables
	[SerializeField] List<GameObject> Roles;
	//Functions
	public void OnPointerEnter(PointerEventData eventData)
	{
		foreach (GameObject obj in Roles)
		{
			obj.SetActive(true);
		}
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		foreach (GameObject obj in Roles)
		{
			obj.SetActive(false);

		}
	}
}
