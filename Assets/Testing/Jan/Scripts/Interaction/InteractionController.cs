using UnityEngine;

namespace Interactables
{
    /// <summary>
    /// Base class for interactable objects like signs but also dungeon entrances that shall be interacted by pressing the interaction button (currently 'E' on keyboard)
    /// </summary>
    public class InteractionController : MonoBehaviour
    {
        //------------------------------ Fields ------------------------------
        [Header("Needed Referneces of Base Class")]
        [Tooltip("The Scriptable Object called 'InputReader' needs to be referenced here.")]
        [SerializeField] protected InputReaderSO _inputReaderSO;
        [Tooltip("Reference to the GameObject 'PlayerInteractionFeedback' (Found in Hierarchy -> Canvas-Ingame/IngameUI-Panel/)")]
        [SerializeField] protected GameObject[] _interactionFeedbackUI;

        protected Transform _playerTransform;


        //------------------------------ Methods ------------------------------

        //---------- Unity-Executed Methods ----------
        protected void Awake()
        {
            #region AutoReferencing            
            
            _interactionFeedbackUI = GameObject.FindGameObjectsWithTag("UserUIFeedback");            

            if (_inputReaderSO == null)
            {
                _inputReaderSO = Resources.Load("ScriptableObjects/InputReader") as InputReaderSO;
                Debug.Log($"<color=yellow>Caution! Reference for Scriptable Object 'InputReaderSO' was not set in Inspector of '{this}'. Trying to set automatically.</color>");
            }

            #endregion
        }

        protected void Start()
        {
            // disableing the Interaction-UI-Marker
            for (int i = 0; i < _interactionFeedbackUI.Length; i++)
                _interactionFeedbackUI[i].SetActive(false);
            Debug.Log($"interactionFeedbackUI was disabled in {this}");


        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            // On Collision with Player subscribe to '_inputReaderSO.OnInteractionInput' and call Logic in 'ReadInteractionInput()' and enable Interaction-FeedBack-UI for User to know an Interaction is possible
            if (collision.CompareTag("Player"))
            {
                _inputReaderSO.OnInteractionInput += ReadInteractionInput;
                Debug.Log($"<color=lime>Player entered interactable zone -> Interaction posibillity activated</color>");

                for (int i = 0; i < _interactionFeedbackUI.Length; i++)
                    _interactionFeedbackUI[i].SetActive(true);
            }
        }

        // todo: following Method might be obsolete (depends on further GameDesign-Choices tho, thats why I left it in at this point yet); JM (09.Oct.2023)
        protected void OnTriggerStay2D(Collider2D collision)
        {
            // On Collision with Player store the Transform of the Player into '_playerTransform' for possible later usage
            if (collision.CompareTag("Player"))
            {
                //Debug.Log($"<color=lime>Player stays in interactable zone -> ready for Interaction! (PRESS 'E')</color>");

                _playerTransform = collision.GetComponent<Transform>(); // needs to stay in OnTriggerStay since the PlayerPosition can also still change when the Player moves inside the Triggerzone; JM
            }
        }

        // On Collision with Player unsubscribe from '_inputReaderSO.OnInteractionInput' and disable Interaction-FeedBack-UI for User to know an Interaction is not possible anymore
        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _inputReaderSO.OnInteractionInput -= ReadInteractionInput;
                Debug.Log($"<color=lime>Player exits interactable zone -> Interaction posibillity deactivated</color>");

                for (int i = 0; i < _interactionFeedbackUI.Length; i++)
                    _interactionFeedbackUI[i].SetActive(false);
            }
        }

        //---------- Custom Methods ----------
        // ReadInteractionInput for Actual definition in ChildClasses
        protected virtual void ReadInteractionInput()
        {
            //Debug.Log($"<color=orange>Interaction-Button was pushed; Proper Logic not yet implemented tho.</color>");
        }
    }
}