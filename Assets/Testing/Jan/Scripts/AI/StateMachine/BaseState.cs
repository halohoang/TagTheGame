using System.Collections.Generic;
using UnityEngine;

namespace StateMashine
{
    public class BaseState : MonoBehaviour
    {
        protected List<Transition> _transitions;

        protected void Awake()
        {
            _transitions = new List<Transition>();
            // todo: implement Transitions; JM
        }

        /// <summary>
        /// For Initialization
        /// </summary>
        protected virtual void OnEnable()
        {
            // todo: develop state's initialization
        }

        /// <summary>
        /// For Finalization
        /// </summary>
        protected virtual void OnDisable()
        {
            // todo: develop state's finiliazation
        }

        /// <summary>
        /// Mehtod for handling physics related Behaviour (in case of none physics related behaviour put it in Update())
        /// </summary>
        protected virtual void FixedUpdate()
        {
            // todo: develop physics related behaviour
        }

        /// <summary>
        /// Method for developing the proper behaviour for the State
        /// </summary>
        protected virtual void Update()
        {
            // todo: develop behaviour here
        }

        /// <summary>
        /// Method for declining if and which state to enable next
        /// </summary>
        protected void LateUpdate()
        {
            foreach (Transition transit in _transitions)
            {
                if (transit.Condition.Condition())
                {
                    transit.TargetState.enabled = true;
                    this.enabled = false;
                    return;
                }
            }
        }
    }
}