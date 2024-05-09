using TMPro;
using UnityEngine;

namespace JansLittleHelper
{
    public static class NullChecksAndAutoReferencing
    {
        /// <summary>
        /// Checks the transmitted GameObject ('objToCheck') if it's null or if it fits an element of the 'arrayToCheckIn' and
        /// returns the correct GameObject if it either was null or it is not the Gameobject with the 'nameOfObjToCheck' if it was found in the 'arrayToCheckIn'.
        /// </summary>
        /// <param name="objToCheck">The GameObject Obj that shall be null-checked</param>
        /// <param name="nameOfObjToCheck">The name of the GameObject that shall be null-checked</param>
        /// <param name="arrayToCheckIn">The array wich elements are compared with the <see cref="objToCheck"/></param>
        /// <returns></returns>
        public static GameObject CheckAndGetGameObject(GameObject objToCheck, string nameOfObjToCheck, GameObject[] arrayToCheckIn)
        {
            for (int i = 0; i < arrayToCheckIn.Length; i++)
            {
                if (arrayToCheckIn[i].name == nameOfObjToCheck && (objToCheck == null || objToCheck != arrayToCheckIn[i]))
                    objToCheck = arrayToCheckIn[i];
            }
            return objToCheck;
        }

        /// <summary>
        /// Checks the transmitted GameObject-Array for element with the name that matches the transmitted 'nameToCheckFor' and returns it's TextMeshProUGUI-Component of the found Object.
        /// returns null if no Element in the Array matches the transmitted name.
        /// </summary>
        /// <param name="listToCheck">The GameObject-Array that shall be checked for the searched Object</param>
        /// <param name="nameToCheckFor">The name of the Object searched for</param>
        /// <returns></returns>
        public static TextMeshProUGUI GetTMPFromTagList(GameObject[] listToCheck, string nameToCheckFor)
        {
            TextMeshProUGUI tmpObj;

            for (int i = 0; i < listToCheck.Length; i++)
            {
                if (listToCheck[i].name == nameToCheckFor)
                {
                    tmpObj = listToCheck[i].GetComponent<TextMeshProUGUI>();

                    return tmpObj;
                }
            }

            Debug.LogError($"<color=red>Caution!</color> No fitting TextMeshProUGUI-Object was found during search in List ('{listToCheck}') -> check if 'nameToCheckFor'-string was written" +
                $" correctly or for other problem.");
            return null;
        }
    }
}