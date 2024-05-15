using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnLaunBulletObjectPool : BaseObjectPool
{
    internal static EnLaunBulletObjectPool Instance { get; private set; }

    private new void Awake()
    {
        #region Singleton
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        #endregion

        base.Awake();
    }
}
