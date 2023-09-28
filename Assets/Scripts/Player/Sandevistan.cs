using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandevistan : MonoBehaviour
{
	[SerializeField] private int ClonesPerSecond = 10;
	private SpriteRenderer sr;
	private Animator animator;
	private Transform tf;
	private List<GameObject> clones;
	[SerializeField] private Vector3 scalePerSecond = new Vector3(1f, 1f, 1f);
	[SerializeField] private Color colorPerSecond = new Color(0f, 0f, 0f, 0.1f); // Adjust alpha value here
	[SerializeField] private float cloneDistance = 0.2f; // Adjust the distance between clones

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		tf = transform; // Cache the Transform component
		clones = new List<GameObject>();
		StartCoroutine(Trail());
	}

	private void Update()
	{
		// You can put other code here if needed
	}

	private IEnumerator Trail()
	{
		for (; ; )
		{
			if (Input.GetKey(KeyCode.Space)) // Check if spacebar is held
			{
				var clone = new GameObject("TrailClone");

				// Calculate the new position based on the character's direction and clone distance
				Vector3 offset = (sr.flipX ? -Vector3.right : Vector3.right) * cloneDistance;
				clone.transform.position = tf.position + offset;

				clone.transform.localScale = tf.localScale;

				var cloneRend = clone.AddComponent<SpriteRenderer>();
				cloneRend.sprite = GetCurrentSprite(); // Set the current sprite

				cloneRend.sortingOrder = sr.sortingOrder - 1;

				// Flip the trailing sprite if the character is facing left
				cloneRend.flipX = sr.flipX;

				// Copy the player's rotation to the clone
				clone.transform.rotation = tf.rotation;

				clones.Add(clone);

				// Start a coroutine for fading and scaling the clone
				StartCoroutine(FadeAndScale(cloneRend));
			}

			yield return new WaitForSeconds(1f / ClonesPerSecond);
		}
	}

	private Sprite GetCurrentSprite()
	{
		// Use the character's Animator to get the current sprite
		if (animator != null)
		{
			var currentAnimatorState = animator.GetCurrentAnimatorStateInfo(0);
			var currentFrame = Mathf.FloorToInt(currentAnimatorState.normalizedTime * currentAnimatorState.length * animator.GetCurrentAnimatorClipInfo(0).Length);
			return sr.sprite;
		}
		return sr.sprite;
	}

	private IEnumerator FadeAndScale(SpriteRenderer cloneRenderer)
	{
		while (cloneRenderer.color.a > 0f && cloneRenderer.transform.localScale != Vector3.zero)
		{
			cloneRenderer.color -= colorPerSecond * Time.deltaTime;
			cloneRenderer.transform.localScale -= scalePerSecond * Time.deltaTime;
			yield return null;
		}

		// Ensure the clone is destroyed after fading out
		clones.Remove(cloneRenderer.gameObject);
		Destroy(cloneRenderer.gameObject);
	}
}