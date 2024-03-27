using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    /// <summary>
    /// Child class to 'Interactable.cs'. Handles 
    /// </summary>
    public class Interactable_Console : Interactable
    {
        //------------------------------ Events ------------------------------
        public static event UnityAction OnDoorStatusChange;       // for sending message to NavMeshBuilder.cs to update NavMeshSurface

        //------------------------------ Fields ------------------------------
        [Header("Settings")]
        [Tooltip("The GameObject (Console) that should be connetced to this Door to controll it (open/close; enables/disables the Object)")]
        [SerializeField] private List<GameObject> _consoleControledObjects;


        //------------------------------ Methods ------------------------------
        protected override void ReadInteractionInput()
        {
            _wasInteractedWith = true;      // in this case, was console used?

            // initial Check if Array is empty
            if (_consoleControledObjects.Count < 1)
            {
                Debug.LogError($"<color=orange>Error!</color>: There are no references set to the elements of the Array 'Console Controled Objects' in the inspector of '{this}'! Therefore {this.gameObject.name} will not work!");
                return;
            }

            //PlayAnimation("...");           // todo: if Door-KickIn-ANimation for Player is implemented fill out the Name-sting; JM (09.Oct.2023)

            //PlaySFX("...");                 // Play DoorKickIn Sound

            // todo: exchange this Logic by playing Open/Close Animation; JM (14.11.2023)
            // Activate/deactivate door-object on console interaction
            foreach (GameObject controledObj in _consoleControledObjects)
            {
                if (controledObj != null)
                {
                    SetActiveStatusOfControlledObj(controledObj);
                }
                else // clear list, set new references to ListElements and try anew to set Active Status
                {
                    _consoleControledObjects.Clear();

                    for (int y = 0; y < gameObject.transform.childCount; y++)
                    {
                        _consoleControledObjects.Add(gameObject.transform.GetChild(y).gameObject);
                    }

                    SetActiveStatusOfControlledObj(controledObj);
                }
            }

            OnDoorStatusChange?.Invoke();

            base.ReadInteractionInput();    // for fireing the Event 'OnInteractionLogicHasBeenExecuted'
        }

        /// <summary>
        /// Enables/disables the thransmitted GameObject respective to its activeSelf-property
        /// </summary>
        /// <param name="controledObj"></param>
        private void SetActiveStatusOfControlledObj(GameObject controledObj)
        {
            if (controledObj.activeSelf)
            {
                controledObj.SetActive(false);
                Debug.Log($"<color=lime>{gameObject.name}</color>: was used, '{controledObj.name}' should have been _opend_. New NavMesh should have been baked.");
            }
            else if (!controledObj.activeSelf)
            {
                controledObj.SetActive(true);
                Debug.Log($"<color=lime>{gameObject.name}</color>: was used, '{controledObj.name}' should have been _closed_. New NavMesh should have been baked.");
            }
        }
    }
}