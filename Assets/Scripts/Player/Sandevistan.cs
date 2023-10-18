using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Sandevistan : MonoBehaviour
{
	//Variables
	[Header("References")]
	[SerializeField] private InputReaderSO _inputReader;
	[Space(5)]

	[SerializeField] private int ClonesPerSecond = 10;
	//[SerializeField] private int ClonesDashPerSecond = 10;

	private SpriteRenderer sr;
	private Animator animator;
	private Transform tf;
	private List<GameObject> clones;
	[SerializeField] private Vector3 scalePerSecond = new Vector3(1f, 1f, 1f);
	[SerializeField] private Color colorPerSecond = new Color(0f, 0f, 0f, 0.1f); // Adjust alpha value here
	[SerializeField] private float cloneSpaceDistance = 0.2f; // Adjust the distance between clones

	//[SerializeField] private Movement _movement;

	//--------------- Methods ---------------
	//---------- Unity-Executed Methods ----------
	private void Awake()
	{
		if (_inputReader == null)
			_inputReader = Resources.Load("ScriptableObjects/InputReader") as InputReaderSO;
	}

	//private void OnEnable()
	//{
	//    _inputReader.OnDashInput += ;
	//}

	//private void OnDisable()
	//{
	//    _inputReader.OnDashInput -= ;
	//}

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		tf = transform; // Cache the Transform component
		clones = new List<GameObject>();
		StartCoroutine(SpaceTrail());
		//StartCoroutine(DashTrail());
	}
	internal IEnumerator SpaceTrail()
	{
		for (; ; )
		{
			if (Input.GetKey(KeyCode.Space)) // Check if spacebar is held
			{
				var clone = new GameObject("TrailClone");

				// Calculate the new position based on the character's direction and clone distance
				Vector3 offset = (sr.flipX ? -Vector3.right : Vector3.right) * cloneSpaceDistance;
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

	internal Sprite GetCurrentSprite()
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

	internal IEnumerator FadeAndScale(SpriteRenderer cloneRenderer)
	{
		while (cloneRenderer.color.a > 0f && cloneRenderer.transform.localScale != Vector3.zero)
		{
			cloneRenderer.color -= colorPerSecond * Time.deltaTime;
			cloneRenderer.transform.localScale -= scalePerSecond * Time.deltaTime;
			yield return null;
		}

		// Ensure the clone is destroyed after fading out
		clones.Remove(cloneRenderer.gameObject);
		//Destroy(cloneRenderer.gameObject);
		cloneRenderer.gameObject.SetActive(false);
	}

	//internal IEnumerator DashTrail()
	//{
	//	for (; ; )
	//	{
	//		if (_movement._currentDashCooldown <= _movement._dashCooldown)
	//		{
	//			if (Input.GetKey(KeyCode.LeftShift)) // Check if leftshift is held
	//			{
	//				// Calculate the total distance covered in the dash
	//				float totalDashDistance = Mathf.Abs(_movement._playerDashForce * _movement._dashCooldown);

	//				// Calculate the number of clones based on the desired spacing
	//				int numberOfClones = Mathf.FloorToInt(totalDashDistance / cloneDashDistance);

	//				// Calculate the actual spacing between clones
	//				float actualSpacing = totalDashDistance / numberOfClones;

	//				// Calculate the direction of the dash (left or right)
	//				Vector3 dashDirection = sr.flipX ? Vector3.left : Vector3.right;

	//				// Calculate the starting position of the dash
	//				Vector3 dashStartPosition = tf.position;

	//				for (int i = 0; i < numberOfClones; i++)
	//				{
	//					var clone = new GameObject("TrailClone");

	//					// Calculate the position of the clone based on the starting position and spacing
	//					Vector3 clonePosition = dashStartPosition + (dashDirection * (i * actualSpacing));
	//					clone.transform.position = clonePosition;

	//					clone.transform.localScale = tf.localScale;

	//					var cloneRend = clone.AddComponent<SpriteRenderer>();
	//					cloneRend.sprite = GetCurrentSprite(); // Set the current sprite

	//					cloneRend.sortingOrder = sr.sortingOrder - 1;

	//					// Flip the trailing sprite if the character is facing left
	//					cloneRend.flipX = sr.flipX;

	//					// Copy the player's rotation to the clone
	//					clone.transform.rotation = tf.rotation;

	//					clones.Add(clone);

	//					// Start a coroutine for fading and scaling the clone
	//					StartCoroutine(FadeAndScale(cloneRend));
	//				}
	//			}
	//		}
	//		yield return new WaitForSeconds(1f / ClonesDashPerSecond);
	//	}
	//}



}