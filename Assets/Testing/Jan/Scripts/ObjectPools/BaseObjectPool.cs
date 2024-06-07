using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectPool : MonoBehaviour
{
    //--------------------------------------
    // - - - - -  V A R I A B L E S  - - - - 
    //--------------------------------------

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
    [SerializeField, ReadOnly] private List<GameObject> _pooledObjects;

    private List<GameObject> _inactivePooledObjects;
    private List<GameObject> _activePooledObjects;

    internal GameObject ObjectToPool { get => _objectToPool; private set => _objectToPool = value; }
    internal int AmountToPool { get => _amountToPool; private set => _amountToPool = value; }

    //----------------------------------
    // - - - - -  M E T H O D S  - - - - 
    //----------------------------------

    //---------- Unity-Executed Methods ----------
    protected virtual void Awake()
    {
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
        for (int i = 0; i < AmountToPool; i++)
        {
            tempObject = Instantiate(ObjectToPool, _parentObject.transform);
            tempObject.SetActive(false);
            _pooledObjects.Add(tempObject);
        }

        // initialize other Lists
        _inactivePooledObjects = new List<GameObject>();
        _activePooledObjects = new List<GameObject>();
    }


    //---------- Custom Methods ----------
    /// <summary>
    /// Returns the disabled Objects stored to the Object-Pool-List
    /// </summary>
    /// <returns></returns>
    public GameObject GetInactivePooledObject()
    {
        for (int i = 0; i < AmountToPool; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
                return _pooledObjects[i];
        }
        return null;
    }

    /// <summary>
    /// Returns the enabled Objects stored to the Object-Pool-List
    /// </summary>
    /// <returns></returns>
    public GameObject GetActivePooledObject()
    {
        for (int i = 0; i < AmountToPool; i++)
        {
            if (_pooledObjects[i].activeInHierarchy)
                return _pooledObjects[i];
        }
        return null;
    }

    /// <summary>
    /// Returns the disabled Objects stored to the Object-Pool-List
    /// </summary>
    /// <returns></returns>
    public GameObject[] GetAllInactivePooledObjects()
    {
        // clear List
        _inactivePooledObjects.Clear();

        // add any inactive Obj. to List
        for (int i = 0; i < AmountToPool; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
                _inactivePooledObjects.Add(_pooledObjects[i]);
        }
        // return List
        return _inactivePooledObjects.ToArray();
    }

    /// <summary>
    /// Returns the enabled Objects stored to the Object-Pool-List
    /// </summary>
    /// <returns></returns>
    public GameObject[] GetAllActivePooledObjects()
    {
        // clear List
        _activePooledObjects.Clear();

        // add any active Obj. to List
        for (int i = 0; i < AmountToPool; i++)
        {
            if (_pooledObjects[i].activeInHierarchy)            
                _activePooledObjects.Add(_pooledObjects[i]);
        }
        // return List
        return _activePooledObjects.ToArray();
    }

    /// <summary>
    /// Retursn all pooled Objects no matter if en- or disabled
    /// </summary>
    /// <returns></returns>
    public GameObject[] GetAllPooledObjects()
    {        
        return _pooledObjects.ToArray();
    }
}