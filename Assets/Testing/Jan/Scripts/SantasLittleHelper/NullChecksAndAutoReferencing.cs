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
    }
}