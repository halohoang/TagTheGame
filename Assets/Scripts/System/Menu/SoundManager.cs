using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour
{
	//Variables
	[SerializeField] AudioClip _hoverSound;
	[SerializeField] AudioClip _clickSound;

	AudioSource _audioSource;
	[SerializeField] List<Button> listOfInteractableButtons;

	private static SoundManager instance;

	[SerializeField] private AudioClip _backgroundSong;
	private bool _backgroundSongPlaying = false;

	//Functions
	void Awake()
	{
			Debug.Log("Destroy?");
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		_audioSource = GetComponent<AudioSource>();
		PlayBackgroundSong();
		SetupButtonListeners();
	}

	void PlayBackgroundSong()
	{
		if (!_backgroundSongPlaying && _backgroundSong != null)
		{
			_audioSource.clip = _backgroundSong;
			_audioSource.loop = true;
			_audioSource.Play();
			_backgroundSongPlaying = true;
		}
	}

	void SetupButtonListeners()
	{
		foreach (Button button in listOfInteractableButtons)
		{
			EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
			if (trigger == null)
			{
				trigger = button.gameObject.AddComponent<EventTrigger>();
			}

			EventTrigger.Entry pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
			pointerEnter.callback.AddListener((data) => { PlaySound(_hoverSound); });
			trigger.triggers.Add(pointerEnter);

			EventTrigger.Entry click = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
			click.callback.AddListener((data) => { PlaySound(_clickSound); });
			trigger.triggers.Add(click);
		}
	}

	void PlaySound(AudioClip soundClip)
	{
		if (soundClip != null)
		{
			_audioSource.PlayOneShot(soundClip);
		}
	}

	public void ResetBackgroundSongPlaying()
	{
		_backgroundSongPlaying = false;
	}

	public void StopPersistence()
	{
		Destroy(gameObject);
	}
	
}
