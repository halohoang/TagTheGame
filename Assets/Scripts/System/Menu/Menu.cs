using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
	// Variables
	[SerializeField] AudioSource _AudioSource;
	[SerializeField] AudioClip _AudioClip;
	// Functions
	private void Start()
	{
		_AudioSource = GetComponent<AudioSource>();
	}
	public void Play()
	{
		_AudioSource.PlayOneShot(_AudioClip,1);
		StartCoroutine(Delay(1, 2.9f)); //Load scene 1 after 2.9s delay
	}

	public void Quit()
	{
		Application.Quit();
	}
	IEnumerator Delay(int sceneIndex, float delay)
	{
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(sceneIndex);
	}
}
