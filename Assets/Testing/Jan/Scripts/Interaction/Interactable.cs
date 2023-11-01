using UnityEngine;
using UnityEngine.Events;
using EnumLibrary;
using NaughtyAttributes;

namespace Interactables
{
    /// <summary>
    /// Chld class for interactable signs-object ingame tha shall be interacted by pressing the 'E'-Key when player is in appropriate trigger zone
    /// </summary>
    public class Interactable : InteractionController
    {
        //------------------------------ Events ------------------------------
        public static event UnityAction OnDoorStatusChange;       // for sending message to NavMeshBuilder.cs to update NavMeshSurface
        public static event UnityAction<bool, Vector3, float> OnDoorKickIn;    // for sending message to EnemyQuickFixSolution_ForTesting.cs to ifnorm about DoorKickIn and DoorPosition

        //------------------------------ Fields ------------------------------
        [Header("Needed References to GameObjects")]
        [Space(2)]
        [Tooltip("Reference to the Animator-Component of this Object")]
        [SerializeField] private Animator _animCtrl;
        [Tooltip("Reference to the Animator-Component of the Player-Object")]
        [SerializeField] private Animator _playerAnim;
        [Space(5)]

        [Header("Settings")]
        [Tooltip("Set which type of Interactable this Object is for appropriate Interaction-Logic")]
        [SerializeField] private Enum_Lib.EInteractableType _interactableType;
        [SerializeField, Range(0.0f, 10.0f)] private float _doorKickInNoiseRange = 10.0f;
        [Space(5)]

        [Header("Monitoring values")]
        [SerializeField, ReadOnly] private bool _wasDoorKickedIn;

        //------------------------------ Methods ------------------------------

        //---------- Unity-Executed Methods ----------
        private new void Awake()
        {
            #region AutoReferencing

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

            base.Awake();

            #endregion
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _doorKickInNoiseRange);
        }

        //---------- Custom Methods ----------
        /// <summary>
        /// activates Tresure-Open-Animation, disables User-Interaction-Feedback-Marker and starts coroutine for collecting-Item-logic 
        /// </summary>
        protected override void ReadInteractionInput()
        {
            // disable Interaction-Feedback-UI
            for (int i = 0; i < _interactionFeedbackUI.Length; i++)
                _interactionFeedbackUI[i].SetActive(false);

            switch (_interactableType)
            {
                case Enum_Lib.EInteractableType.Door:

                    _wasDoorKickedIn = true;

                    PlayAnimation("...");           // todo: if Door-KickIn-ANimation for Player is implemented fill out the Name-sting; JM (09.Oct.2023)
                    
                    PlaySFX("...");                 // Play DoorKickIn Sound

                    gameObject.SetActive(false);    // todo: exchange this later to switching the GameObjects from intact door to broken door; JM (09.Oct.2023)

                    OnDoorStatusChange?.Invoke();

                    OnDoorKickIn?.Invoke(_wasDoorKickedIn, transform.position, _doorKickInNoiseRange); // Event for Informing Enemies that Door was Kicked in to react to

                    // todo: send physics.sphereoverlap from specific door gameobject so that every enemy within a certain radius can react to the door-kick-in-event; JM (13.Oct.2023)
                    // todo: (!)start runtime baking of nw nav mesh so the new accured walkable space (where once the door was) is walkable for the AI; JM (09.Oct.2023)

                    break;

                default:
                    break;
            }
        }

        private void PlayAnimation(string animationName)
        {
            if (_animCtrl != null)
                _animCtrl.SetBool(animationName, true);
        }

        private void PlaySFX(string soundToBePlayed)
        {
            //todo: If AudioManager for Handling SFX is implemented following outcommented Code can be commented in again; JM (09.Oct.2023)
            //if (AudioManager.Instance != null)
            //    AudioManager.Instance.PlayEffectSound(soundToBePlayed);                     
        }
    }
}