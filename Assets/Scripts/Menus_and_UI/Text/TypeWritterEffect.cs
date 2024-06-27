using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeWritterEffect : MonoBehaviour
{
	// variables
	public float delay = 0.1f; // Delay between each character being displayed
	public string fullText; // The complete text to be displayed
	private string currentText = ""; // The text being displayed gradually

	//Functions
	private void Start()
	{
		StartCoroutine(ShowText());
	}

	IEnumerator ShowText()
	{
		for (int i = 0; i <= fullText.Length; i++)
		{
			currentText = fullText.Substring(0, i);
			GetComponent<TextMeshProUGUI>().text = currentText;
			yield return new WaitForSeconds(delay);
		}
	}
}
