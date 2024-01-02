using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    public class Interactable_Door : Interactable
    {
        //------------------------------ Events ------------------------------
        public static event UnityAction OnDoorStatusChange;       // for sending message to NavMeshBuilder.cs to update NavMeshSurface
        public static event UnityAction<bool, Vector3, float> OnDoorKickIn;    // for sending message to EnemyQuickFixSolution_ForTesting.cs to ifnorm about DoorKickIn and DoorPosition

        //------------------------------ Fields ------------------------------
        [Header("Settings")]
        [SerializeField, Range(0.0f, 20.0f)] private float _doorKickInNoiseRange = 10.0f;


        //------------------------------ Methods ------------------------------

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _doorKickInNoiseRange);
        }

        protected override void ReadInteractionInput()
        {
            _wasInteractedWith = true;      // in this case, was door kicked in?

            //PlayAnimation("...");           // todo: if Door-KickIn-ANimation for Player is implemented fill out the Name-sting; JM (09.Oct.2023)

            //PlaySFX("...");                 // Play DoorKickIn Sound

            gameObject.SetActive(false);    // todo: exchange this later to switching the GameObjects from intact door to broken door; JM (09.Oct.2023)

            OnDoorStatusChange?.Invoke();

            OnDoorKickIn?.Invoke(_wasInteractedWith, transform.position, _doorKickInNoiseRange); // Event for Informing Enemies that Door was Kicked in to react to

            base.ReadInteractionInput();    // for fireing the Event 'OnInteractionLogicHasBeenExecuted'
        }
    }
}