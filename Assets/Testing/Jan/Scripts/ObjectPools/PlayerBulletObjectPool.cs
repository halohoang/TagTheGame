using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletObjectPool : MonoBehaviour
{
    //------------------------------ Fields ------------------------------
    public static PlayerBulletObjectPool Instance;

    [Header("Needed References")]
    [Tooltip("Reference to the GameObject in the Hierarchy that shall parent the pooled Objects.")]
    [SerializeField] private GameObject _parentObject;
    [Tooltip("Reference to the kind of GameObject that shall be pooled (Idially it's a Prefab from the Project Folder, but can also be an active GameObject from the Hierarchy)")]
    [SerializeField] private GameObject _objectToPool;
    [Space(5)]

    [Header("Further Settings")]
    [Tooltip("The amount of Objects that shall be pooled at maximum")]
    [SerializeField] private int _amountToPool;
    [Space(5)]

    [Header("Currently Pooled Objects")]
    [SerializeField] private List<GameObject> _pooledObjects;

    public GameObject ObjectToPool { get => _objectToPool; private set => _objectToPool = value; }

    //------------------------------ Methods ------------------------------

    //---------- Unity-Executed Methods ----------
    private void Awake()
    {
        #region Singleton
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        #endregion

        #region Null Checks
        if (_parentObject == null)
        {
            _parentObject = GameObject.FindGameObjectWithTag("PowerupObjectPool");
            Debug.Log($"<color=yellow>Caution! Reference to 'Parent Object' was not set in the inspector of '{this}'. Trying to set automatically.</color>");
        }
        #endregion
    }

    private void Start()
    {
        // create a new Obj-List the length of _amountToPool, instantiate Objects for each Idx of this list and add the inatantiated object to this list
        _pooledObjects = new List<GameObject>();
        GameObject tempObject;
        for (int i = 0; i < _amountToPool; i++)
        {
            tempObject = Instantiate(ObjectToPool, _parentObject.transform);
            tempObject.SetActive(false);
            _pooledObjects.Add(tempObject);
        }
    }


    //---------- Custom Methods ----------
    /// <summary>
    /// Returns the disabled Objects stored to the Object-Pool-List
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _amountToPool; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }
        return null;
    }
}
