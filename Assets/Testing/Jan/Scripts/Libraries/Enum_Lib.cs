using UnityEngine;

namespace EnumLibrary
{
    public class Enum_Lib : MonoBehaviour
    {
        // todo: add more InteractableTypes if necessary; JM (09.Oct.2023)
        // Interactablt Types
        public enum EInteractableType
        {
            Door
        }

        // todo: maybe change the naming of the Enemy-Type-Enums yet (if more types of Enemy will be implementet), maybe to EnemyType_one or so; JM (17.10.2023)
        public enum EEnemyType
        {
            Melee_Enemy,
            Range_Enemy
        }
    }
}