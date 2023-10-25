using UnityEngine;

namespace StateMashine
{
    public class BaseCondition
    {
        protected internal virtual bool Condition()
        {
            return false;
        }
    }
}