using UnityEngine;

namespace UI
{
    public class UIFollowsObject : MonoBehaviour
    {
        //------------------------------ Fields ------------------------------
        [Header("Needed References")]
        [Tooltip("The transform of the game object to follow")]
        [SerializeField] private Transform _transformToFollow;
        [Tooltip("The Reference to the Main Camera Object of the Scene")]
        [SerializeField] private Camera _mainCamera;
        [Space(5)]

        [Header("Settings")]
        [Tooltip("The Offset Vector that defines where the InteractionFeedbackMarker shall be shown to the User (Normally it should be 70 Uints on the Y-Axis above the Player-Object)")]
        [SerializeField] private Vector3 _offsetVector = new Vector3(0, 70, 0);


        //------------------------------ Methods ------------------------------

        //---------- Unity-Executed Methods ----------
        private void Start()
        {
            #region AutoReferencing

            if (_transformToFollow == null)
            {
                _transformToFollow = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
                Debug.Log($"<color=yellow>Caution! Reference to 'Transform To Follow' not set in inspector of {this}. Trying to set automatically.</color>");
            }

            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
                Debug.Log($"<color=yellow>Caution! Reference to 'Main Camera' not set in inspector of {this}. Trying to set automatically.</color>");
            }

            #endregion


            //setting the correct Offset-Position appropriate to the resolution of the used screen
            if (Screen.currentResolution.width == 1680 && Screen.currentResolution.height == 1050)
            {
                _offsetVector = new Vector3(0, 60, 0);
            }
            else if (Screen.currentResolution.width == 1600 && Screen.currentResolution.height == 900)
            {
                _offsetVector = new Vector3(0, 50, 0);
            }
        }

        private void Update()
        {
            if (_mainCamera != null)
            {
                transform.position = _mainCamera.WorldToScreenPoint(_transformToFollow.position) + _offsetVector;   // setting the Position to the GameObject to Follow
            }
            else
            {
                _mainCamera = Camera.main;
                transform.position = _mainCamera.WorldToScreenPoint(_transformToFollow.position) + _offsetVector;
            }
        }
    }
}