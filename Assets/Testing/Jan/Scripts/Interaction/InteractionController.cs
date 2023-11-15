using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    /// <summary>
    /// Base class for interactable objects like signs but also dungeon entrances that shall be interacted by pressing the interaction button (currently 'E' on keyboard)
    /// </summary>
    public abstract class InteractionController : MonoBehaviour
    {
        //------------------------------ Fields ------------------------------
        public static event UnityAction<bool> OnEnterInteractableTriggerZone;

        //------------------------------ Fields ------------------------------
        [Header("Needed Referneces of Base Class")]
        [Tooltip("The Scriptable Object called 'InputReader' needs to be referenced here.")]
        [SerializeField] protected InputReaderSO _inputReaderSO;
        [Tooltip("Reference to the GameObject 'PlayerInteractionFeedback' (Found in Hierarchy -> Canvas-Ingame/IngameUI-Panel/)")]
        [SerializeField] protected GameObject[] _interactionFeedbackUI;
        [Space(5)]

        [Header("Monitoring Values")]
        [SerializeField, ReadOnly] private bool _isInInteractableTriggerzone;

        protected Transform _playerTransform;



        //------------------------------ Methods ------------------------------
        private void OnEnable()
        {
            //_inputReaderSO.OnInteractionInput += ReadInteractionInput;
            Debug.Log($"<color=lime> OnEnable() was called in {this} </color>");
        }
        private void OnDisable()
        {
            //_inputReaderSO.OnInteractionInput -= ReadInteractionInput;
            Debug.Log($"<color=lime> OnDisable() was called in {this} </color>");
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            // On Collision with Player subscribe to '_inputReaderSO.OnInteractionInput' and call Logic in 'ReadInteractionInput()' and enable Interaction-FeedBack-UI for User to know an Interaction is possible
            if (collision.CompareTag("Player"))
            {
                _isInInteractableTriggerzone = true;
                _inputReaderSO.OnInteractionInput += ReadInteractionInput;
                OnEnterInteractableTriggerZone?.Invoke(_isInInteractableTriggerzone);
                Debug.Log($"<color=lime>Player entered interactable zone -> Interaction posibillity activated</color>");
            }
        }

        // todo: following Method might be obsolete (depends on further GameDesign-Choices tho, thats why I left it in at this point yet); JM (09.Oct.2023)
        protected void OnTriggerStay2D(Collider2D collision)
        {
            // On Collision with Player store the Transform of the Player into '_playerTransform' for possible later usage
            if (collision.CompareTag("Player"))
            {                
                _playerTransform = collision.GetComponent<Transform>(); // needs to stay in OnTriggerStay since the PlayerPosition can also still change when the Player moves inside the Triggerzone; JM
            }
        }

        // On Collision with Player unsubscribe from '_inputReaderSO.OnInteractionInput' and disable Interaction-FeedBack-UI for User to know an Interaction is not possible anymore
        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _isInInteractableTriggerzone = false;
                _inputReaderSO.OnInteractionInput -= ReadInteractionInput;
                OnEnterInteractableTriggerZone?.Invoke(_isInInteractableTriggerzone);
                Debug.Log($"<color=lime>Player exits interactable zone -> Interaction posibillity deactivated</color>");                
            }
        }

        //---------- Custom Methods ----------
        // ReadInteractionInput for Actual definition in ChildClasses
        protected abstract void ReadInteractionInput();        
    }
}