using UnityEngine;
using DG.Tweening;

public class DottweenAppear : MonoBehaviour
{
	[SerializeField] float _animationDuration = 1f;
	[SerializeField] float _maxOpacity;

	void Start()
	{
		RectTransform rectTransform = GetComponent<RectTransform>();

		// Set initial alpha to 0 
		CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
		canvasGroup.alpha = 0f;

		// Fade in to full opacity 
		canvasGroup.DOFade(_maxOpacity, _animationDuration);
	}
}
