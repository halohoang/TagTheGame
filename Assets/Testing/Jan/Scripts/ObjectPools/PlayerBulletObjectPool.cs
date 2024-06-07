
public class PlayerBulletObjectPool : BaseObjectPool
{
    //------------------------------ Fields ------------------------------
    internal static PlayerBulletObjectPool Instance { get; private set; }

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
