using UnityEngine;

namespace EnumLibrary
{
    public class Enum_Lib : MonoBehaviour
    {
        // todo: maybe change the naming of the Enemy-Type-Enums yet (if more types of Enemy will be implementet), maybe to EnemyType_one or so; JM (17.10.2023)
        public enum EEnemyType
        {
            Melee_Enemy,
            Range_Enemy
        }

        // The first simple Basic-Behaviour-Types that are not all implemented into the Prototype yet; JM (27.10.2023)
        public enum EBasicEnemyBehaviourType
        {
            Patroling,
            StandingGuard,
            StandingLookingAround
        }

        public enum EAnimationTriggerType
        {
            // todo: maybe setup some TriggerTypes for specific Animations to be triggered on specifi Events; JM (27.10.2023)
        }

        // Enum for defining if the Space Key is pressed or not
        public enum ESpaceKey
        {
            Pressed,
            NotPressed
        }

        // Enum for defining if the Left Mouse Button Key is pressed or not
        public enum ELeftMouseButton
        {
            Pressed,
            NotPressed,
            Released
        }

        /// <summary>
        /// Enum for Specification of the weapon type
        /// </summary>
        public enum EWeaponType
        {
            Handcannon,
            SMG,
            Shotgun,
            EnergyLauncher,

            Blank               // (!)ATTENTION(!) -> 'Blank' allways needs to be the last entry of this Enum!; (JM, 22.02.2024)
        }

        public enum ESelectedWeapon
        {
            FirstWeapon,
            SecondWeapon
        }

        public enum ENPCMovementBehaviourType
        {
            None,
            PatrolBehavour,
            RandomWanderBehaviour
        }

        public enum EWaypointPatrolOrder
        {
            Forward,
            Reverse
        }
    }
}