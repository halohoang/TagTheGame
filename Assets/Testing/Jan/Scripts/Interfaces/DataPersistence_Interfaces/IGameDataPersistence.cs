
public interface IGameDataPersistence
{
    // todo: implement 'GameData'-Class as 'DataCollector' for Persistance System

    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
