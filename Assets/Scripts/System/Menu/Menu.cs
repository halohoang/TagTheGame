using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
	// Variables
	[SerializeField] AudioSource _audioSource;
	[SerializeField] AudioClip _AudioClip;
	float _startVolume;
	[SerializeField] float _fadeDuration = 2.0f;


	internal bool _startPlay;
	// Functions
	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
		_startVolume = _audioSource.volume;
		
	}
	public void Play()
	{
		_audioSource.PlayOneShot(_AudioClip,1);
		_startPlay = true;
		
		StartCoroutine(Delay(1, 3.1f)); //Load scene 1 after 3.1s delay
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

	public void BackToMainMenu()
	{
		StartCoroutine(Delay(0, 0.2f)); //Load scene 2 after 1.5s delay
	}
	public void Letters() // A letter to a very single one
	{
		StartCoroutine(Delay(3, 0.2f)); //Load scene 2 after 1.5s delay
	}
}
