using Interactables;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class UserFeedbackUIHandler : MonoBehaviour
{
    //------------------------------ Fields ------------------------------
    [Header("Needed Referneces")]
    [Tooltip("Reference to the GameObject 'PlayerInteractionFeedback' (Found in Hierarchy -> Canvas-Ingame/IngameUI-Panel/)")]
    [SerializeField, ReadOnly] private List<GameObject> _interactionFeedbackUI;
    [Space(5)]

    [Header("Monitoring Values")]
    [SerializeField, ReadOnly] private bool _isPlayerInInteractableTriggerzone;


    //------------------------------ Methods ------------------------------
    private void Awake()
    {
        // Instatiating Lists
        _interactionFeedbackUI = new List<GameObject>();

        #region AutoReferencing        

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.CompareTag("UserUIFeedback"))
                _interactionFeedbackUI.Add(transform.GetChild(i).gameObject);
        }

        #endregion
    }

    private void OnEnable()
    {
        Interactable.OnEnterInteractableTriggerZone += SetFeedBackUIActive;
        Interactable.OnInteractionLogicHasBeenExecuted += SetFeedBackUIActive;
    }
    private void OnDisable()
    {
        Interactable.OnEnterInteractableTriggerZone -= SetFeedBackUIActive;
        Interactable.OnInteractionLogicHasBeenExecuted -= SetFeedBackUIActive;

    }

    private void SetFeedBackUIActive(bool isPlayerInInteractableTriggerZone)
    {
        _isPlayerInInteractableTriggerzone = isPlayerInInteractableTriggerZone; // caching value for beeing able to monitor value in inspector; JM (15.11.2023)

        for (int i = 0; i < _interactionFeedbackUI.Count; i++)
            _interactionFeedbackUI[i].SetActive(_isPlayerInInteractableTriggerzone);
    }

    private void Start()
    {
        // disableing the Interaction-UI-Marker
        for (int i = 0; i < _interactionFeedbackUI.Count; i++)
            _interactionFeedbackUI[i].SetActive(false);
        Debug.Log($"interactionFeedbackUI was disabled in {this}");
    }
}
