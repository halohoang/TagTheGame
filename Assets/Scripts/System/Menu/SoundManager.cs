using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour
{
	[SerializeField] AudioClip _hoverSound;
	[SerializeField] AudioClip _clickSound;

	AudioSource audioSource;

	[SerializeField] List<Button> listOfInteractableButtons;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();

		foreach (Button button in listOfInteractableButtons)
		{
			// Add listeners for PointerEnter and Click events
			EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
			if (trigger == null)
			{
				trigger = button.gameObject.AddComponent<EventTrigger>();
			}

			// PointerEnter event
			EventTrigger.Entry pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
			pointerEnter.callback.AddListener((data) => { PlaySound(_hoverSound); });
			trigger.triggers.Add(pointerEnter);

			// Click event
			EventTrigger.Entry click = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
			click.callback.AddListener((data) => { PlaySound(_clickSound); });
			trigger.triggers.Add(click);
		}
	}

	void PlaySound(AudioClip soundClip)
	{
		if (soundClip != null)
		{
			audioSource.PlayOneShot(soundClip);
		}
	}
}
