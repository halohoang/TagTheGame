using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    public class Interactable_Door : Interactable, INoiseEmitting 
    {
        //------------------------------ Events ------------------------------
        public static event UnityAction OnDoorStatusChange;                         // for sending message to NavMeshBuilder.cs to update NavMeshSurface
        public static event UnityAction<bool, Vector3, Collider2D[]> OnDoorKickIn;  // for sending message to EnemyQuickFixSolution_ForTesting.cs to ifnorm about DoorKickIn and DoorPosition

        //------------------------------ Fields ------------------------------
        [Header("Settings")]
        #region Tooltip
        [Tooltip("The actual noise range of this interactable object.")]
        #endregion
        [SerializeField, Range(0.0f, 20.0f), EnableIf("_showNoiseRangeGizmo")] private float _doorKickInNoiseRange = 10.0f;
        #region Tooltip
        [Tooltip("To en/-disable the Gizmo depicting the actual noise range of this interactable object.")]
        #endregion
        [SerializeField] private bool _showNoiseRangeGizmo = true;
        #region Tooltip
        [Tooltip("The game Objects of this layer mask that shall be affected, when this Interactable is interacted with. E.g. Enemies that are within the noise range of this object.")]
        #endregion
        [SerializeField] private LayerMask _affectedObjectsOnInteraction;


        //------------------------------ Methods ------------------------------

        private void OnDrawGizmos()
        {
            if (_showNoiseRangeGizmo)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, _doorKickInNoiseRange);
            }
        }

        protected override void ReadInteractionInput()
        {
            _wasInteractedWith = true;      // in this case, was door kicked in?

            //PlayAnimation("...");           // todo: if Door-KickIn-ANimation for Player is implemented fill out the Name-sting; JM (09.Oct.2023)

            //PlaySFX("...");                 // Play DoorKickIn Sound

            gameObject.SetActive(false);    // todo: exchange this later to switching the GameObjects from intact door to broken door; JM (09.Oct.2023)

            OnDoorStatusChange?.Invoke();

            InformObjectsInNoiseRange(_wasInteractedWith, transform.position, _doorKickInNoiseRange);

            base.ReadInteractionInput();    // for fireing the Event 'OnInteractionLogicHasBeenExecuted'
        }

        /// <summary>
        /// Gets all enemy object wihtin the noise range of this interactable and fires an event to inform every enemy object in the scene that this interactable was interacted with and
        /// transmits the array of enemy objects that are within the noise range of this interaction object
        /// </summary>
        /// <param name="isSomethinHappening"></param>
        /// <param name="positionOfEvent"></param>
        /// <param name="noiseRange"></param>
        public void InformObjectsInNoiseRange(bool isSomethinHappening, Vector3 positionOfEvent, float noiseRange)
        {
            // for every Enemy that actually is in the noise range of the AlertEvent set the appropriate values
            Collider2D[] CollidersWithinRange = Physics2D.OverlapCircleAll(positionOfEvent, noiseRange, _affectedObjectsOnInteraction);
            for (int i = 0; i < CollidersWithinRange.Length; i++)
            {                
                OnDoorKickIn?.Invoke(isSomethinHappening, positionOfEvent, CollidersWithinRange);
            }
        }
    }
}