using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Interactables
{
    /// <summary>
    /// Base class for interactable objects ingame, that shall be interacted with by pressing a specific key or button (for now the 'E'-Key) when player is in appropriate trigger zone.
    /// 
    /// </summary>
    public abstract class Interactable : MonoBehaviour
    {
        //------------------------------ Fields ------------------------------
        public static event UnityAction<bool> OnEnterInteractableTriggerZone;
        public static event UnityAction<bool> OnInteractionLogicHasBeenExecuted;

        //------------------------------ Fields ------------------------------
        [Header("Needed Referneces of Base Class")]
        [Tooltip("The Scriptable Object called 'InputReader' needs to be referenced here.")]
        [SerializeField] protected InputReaderSO _inputReaderSO;
        [Space(5)]

        [Header("Needed References to GameObjects")]
        [Space(2)]
        [Tooltip("Reference to the Animator-Component of this Object")]
        [SerializeField] private Animator _animCtrl;
        [Tooltip("Reference to the Animator-Component of the Player-Object")]
        [SerializeField] private Animator _playerAnim;
        [Space(5)]

        [Header("Monitoring Values")]
        [SerializeField, ReadOnly] private bool _isInInteractableTriggerzone;
        [SerializeField, ReadOnly] protected bool _wasInteractedWith;

        protected Transform _playerTransform;

        //------------------------------ Methods ------------------------------

        private void Awake()
        {
            #region AutoReferencing
            if (_inputReaderSO == null)
            {
                _inputReaderSO = Resources.Load("ScriptableObjects/InputReader") as InputReaderSO;
                Debug.Log($"<color=yellow>Caution! Reference for ScriptableObjects 'InputReader' was not set in Inspector of '{this}'. Trying to set automatically.</color>");
            }

            if (_animCtrl == null)
            {
                _animCtrl = GetComponent<Animator>();
                Debug.Log($"<color=yellow>Caution! Reference for Animator 'Anim Ctrl' was not set in Inspector of '{this}'. Trying to set automatically.</color>");
            }

            if (_playerAnim == null)
            {
                _playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
                Debug.Log($"<color=yellow>Caution! Reference for Animator 'Player Anim' was not set in Inspector of '{this}'. Trying to set automatically.</color>");
            }

            #endregion
        }

        private void OnEnable()
        {
            //_inputReaderSO.OnInteractionInput += ReadInteractionInput;
            //Debug.Log($"<color=lime> OnEnable() was called in {this} </color>");            
        }
        private void OnDisable()
        {
            _inputReaderSO.OnInteractionInput -= ReadInteractionInput;
            //Debug.Log($"<color=lime> OnDisable() was called in {this} </color>");
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            // On Collision with Player subscribe to '_inputReaderSO.OnInteractionInput' and call Logic in 'ReadInteractionInput()' and enable Interaction-FeedBack-UI for User to know an Interaction is possible
            if (collision.CompareTag("Player"))
            {
                _isInInteractableTriggerzone = true;
                _inputReaderSO.OnInteractionInput += ReadInteractionInput;
                OnEnterInteractableTriggerZone?.Invoke(_isInInteractableTriggerzone);
                //Debug.Log($"<color=lime>Player entered interactable zone -> Interaction posibillity activated</color>");
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
        protected virtual void ReadInteractionInput()
        {
            OnInteractionLogicHasBeenExecuted?.Invoke(false);   // for deactivating the Interaction-Sign to User -> see 'UserFeedbackUIHandler.cs'
        }
    }
}