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
        #region Old Interactable-Class
        ////------------------------------ Events ------------------------------
        //public static event UnityAction OnDoorStatusChange;       // for sending message to NavMeshBuilder.cs to update NavMeshSurface
        //public static event UnityAction<bool, Vector3, float> OnDoorKickIn;    // for sending message to EnemyQuickFixSolution_ForTesting.cs to ifnorm about DoorKickIn and DoorPosition
        //public static event UnityAction<bool> OnInteractionLogicHasBeenExecuted;

        ////------------------------------ Fields ------------------------------
        //[Header("Needed References to GameObjects")]
        //[Space(2)]
        //[Tooltip("Reference to the Animator-Component of this Object")]
        //[SerializeField] private Animator _animCtrl;
        //[Tooltip("Reference to the Animator-Component of the Player-Object")]
        //[SerializeField] private Animator _playerAnim;
        //[Space(5)]

        //[Header("Settings")]
        //[Tooltip("Set which type of Interactable this Object is for appropriate Interaction-Logic")]
        //[SerializeField] private Enum_Lib.EInteractableType _interactableType;
        //[SerializeField, Range(0.0f, 20.0f), EnableIf("_interactableType", Enum_Lib.EInteractableType.DoorKickInable)] private float _doorKickInNoiseRange = 10.0f;
        //[Tooltip("The GameObject (Console) that should be connetced to this Door to controll it (oben/close)")]
        //[SerializeField, EnableIf("_interactableType", Enum_Lib.EInteractableType.Console)] private List<GameObject> _consoleControledObjects;
        //[Space(5)]

        //[Header("Monitoring values")]
        //[SerializeField, ReadOnly] protected bool _wasInteractedWith;

        ////------------------------------ Methods ------------------------------

        ////---------- Unity-Executed Methods ----------
        //private void Awake()
        //{
        //    #region AutoReferencing
        //    if (_animCtrl == null)
        //    {
        //        _animCtrl = GetComponent<Animator>();
        //        Debug.Log($"<color=yellow>Caution! Reference for Animator 'Anim Ctrl' was not set in Inspector of '{this}'. Trying to set automatically.</color>");
        //    }

        //    if (_playerAnim == null)
        //    {
        //        _playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        //        Debug.Log($"<color=yellow>Caution! Reference for Animator 'Player Anim' was not set in Inspector of '{this}'. Trying to set automatically.</color>");
        //    }
            
        //    #endregion
        //}

        //private void OnDrawGizmos()
        //{
        //    if (_interactableType == Enum_Lib.EInteractableType.DoorKickInable)
        //    {
        //        Gizmos.color = Color.green;
        //        Gizmos.DrawWireSphere(transform.position, _doorKickInNoiseRange);
        //    }
        //}

        ////---------- Custom Methods ----------
        ///// <summary>
        ///// activates Tresure-Open-Animation, disables User-Interaction-Feedback-Marker and starts coroutine for collecting-Item-logic 
        ///// </summary>
        //protected override void ReadInteractionInput()
        //{
        //    switch (_interactableType)
        //    {
        //        case Enum_Lib.EInteractableType.DoorKickInable:

        //            _wasInteractedWith = true;      // in this case, was door kicked in?

        //            //PlayAnimation("...");           // todo: if Door-KickIn-ANimation for Player is implemented fill out the Name-sting; JM (09.Oct.2023)

        //            //PlaySFX("...");                 // Play DoorKickIn Sound

        //            gameObject.SetActive(false);    // todo: exchange this later to switching the GameObjects from intact door to broken door; JM (09.Oct.2023)

        //            OnDoorStatusChange?.Invoke();

        //            OnDoorKickIn?.Invoke(_wasInteractedWith, transform.position, _doorKickInNoiseRange); // Event for Informing Enemies that Door was Kicked in to react to

        //            break;

        //        case Enum_Lib.EInteractableType.Console:

        //            _wasInteractedWith = true;      // in this case, was console used?

        //            // initial Check if Array is empty
        //            if (_consoleControledObjects.Count < 1)
        //            {
        //                Debug.LogError($"<color=orange>Error!</color>: There are no references set to the elements of the Array 'Console Controled Objects' in the inspector of '{this}'! Therefore {this.gameObject.name} will not work!");
        //                break;
        //            }

        //            //PlayAnimation("...");           // todo: if Door-KickIn-ANimation for Player is implemented fill out the Name-sting; JM (09.Oct.2023)

        //            //PlaySFX("...");                 // Play DoorKickIn Sound

        //            // todo: exchange this Logic by playing Open/Close Animation; JM (14.11.2023)
        //            foreach (GameObject controledObj in _consoleControledObjects)
        //            {
        //                if (controledObj != null)
        //                {
        //                    SetActiveStatusOfControlledObj(controledObj);
        //                }
        //                else // clear list, set new references to ListElements and try anew to set Active Status
        //                {
        //                    _consoleControledObjects.Clear();

        //                    for (int y = 0; y < gameObject.transform.childCount; y++)
        //                    {
        //                        _consoleControledObjects.Add(gameObject.transform.GetChild(y).gameObject);
        //                    }

        //                    SetActiveStatusOfControlledObj(controledObj);
        //                }
        //            }

        //            OnDoorStatusChange?.Invoke();


        //            break;

        //        default:
        //            break;
        //    }

        //    // Fire Event for disableing Interaction-Feedback-UI (the transimmted bool leads to deactivation of FeedbackUI in 'UserFeedbackUIHandler.cs')
        //    //OnInteractionLogicHasBeenExecuted?.Invoke(false);
        //}

        //private void SetActiveStatusOfControlledObj(GameObject controledObj)
        //{
        //    if (controledObj.activeSelf)
        //    {
        //        controledObj.SetActive(false);
        //        Debug.Log($"<color=lime>{gameObject.name}</color>: was used, '{controledObj.name}' should have been _opend_. New NavMesh should have been baked.");
        //    }
        //    else if (!controledObj.activeSelf)
        //    {
        //        controledObj.SetActive(true);
        //        Debug.Log($"<color=lime>{gameObject.name}</color>: was used, '{controledObj.name}' should have been _closed_. New NavMesh should have been baked.");
        //    }
        //}

        //private void PlayAnimation(string animationName)
        //{
        //    if (_animCtrl != null)
        //        _animCtrl.SetBool(animationName, true);
        //}

        //private void PlaySFX(string soundToBePlayed)
        //{
        //    //todo: If AudioManager for Handling SFX is implemented following outcommented Code can be commented in again; JM (09.Oct.2023)
        //    //if (AudioManager.Instance != null)
        //    //    AudioManager.Instance.PlayEffectSound(soundToBePlayed);                     
        //}
        #endregion

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
        protected virtual void ReadInteractionInput()
        {
            OnInteractionLogicHasBeenExecuted?.Invoke(false);   // for deactivating the Interaction-Sign to User -> see 'UserFeedbackUIHandler.cs'
        }
    }
}