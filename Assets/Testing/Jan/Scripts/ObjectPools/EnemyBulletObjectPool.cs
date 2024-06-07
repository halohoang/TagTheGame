
public class EnemyBulletObjectPool : BaseObjectPool
{
    //------------------------------ Fields ------------------------------
    internal static EnemyBulletObjectPool Instance { get; private set; }

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
