using UnityEngine;

namespace StateMashine
{
    public class BaseConditionCheck : MonoBehaviour
    {
        public virtual bool ConditionCheck()
        {
            return false;
        }
    }
}