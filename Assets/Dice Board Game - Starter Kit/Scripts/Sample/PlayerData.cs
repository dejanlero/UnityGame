using UnityEngine;

[System.Serializable]
public class PlayerData
{
    #region Variables
    public Color color;
    public string prefabName;
    public int data1;
    public string data2;
    public float data3;
    /// and so on ...
    #endregion
    #region Load / Save
    public bool Load(int playerIndex)
    {
        string playerIndexKey = PLAYER_INDEX_KEY(playerIndex);
        if (!PlayerPrefs.HasKey(playerIndexKey)) return false;
        string jsonData = PlayerPrefs.GetString(playerIndexKey);
        if (string.IsNullOrEmpty(jsonData)) return false;
        var thisClass =(PlayerData) this;
        thisClass = CreateFromJSON(jsonData);
        return true;
    }
    public bool Save(int playerIndex)
    {
        string playerIndexKey = PLAYER_INDEX_KEY(playerIndex);
        string playerToJson = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(playerIndexKey,playerToJson);
        PlayerPrefs.Save();
        return true;
    }
    public static PlayerData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PlayerData>(jsonString);
    }
    public static string PLAYER_INDEX_KEY(int playerIndex)
    {
        return "MY_GAME_Player_" + playerIndex.ToString();
    }
    #endregion
}
