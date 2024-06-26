using Interactables;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace EnemyPerception
{
    [DisallowMultipleComponent]
    public class AuditivePerception : MonoBehaviour
    {
        //--------------------------------
        // - - - - -  E V E N T S  - - - - 
        //--------------------------------
        public static event UnityAction<bool, Vector3> OnSomethingAlarmingIsHappening;

        private void OnEnable()
        {
            Interactable_Door.OnDoorKickIn += CheckIfAffected;
            PlayerWeaponHandling.OnPlayerShoot += CheckIfAffected;
        }
        private void OnDisable()
        {
            Interactable_Door.OnDoorKickIn -= CheckIfAffected;
            PlayerWeaponHandling.OnPlayerShoot -= CheckIfAffected;
        }

        /// <summary>
        /// Checks if this enemy object is affected by alarming event like Door-KickIn or 'hearing' shooting noises etc.
        /// </summary>
        /// <param name="isSomethinAlarmingHappening"></param>
        /// <param name="positionOfAlarmingEvent"></param>
        /// <param name="CollidersWithinRange"></param>
        private void CheckIfAffected(bool isSomethinAlarmingHappening, Vector3 positionOfAlarmingEvent, Collider2D[] CollidersWithinRange)
        {
            Collider2D thisCollider = GetComponent<Collider2D>();

            // check if this object is among the enemy objects that are actually affected by the alarming event
            for (int i = 0; i < CollidersWithinRange.Length; i++)
            {
                if (thisCollider == CollidersWithinRange[i])
                    OnSomethingAlarmingIsHappening?.Invoke(isSomethinAlarmingHappening, positionOfAlarmingEvent);
            }
        }
    }
}