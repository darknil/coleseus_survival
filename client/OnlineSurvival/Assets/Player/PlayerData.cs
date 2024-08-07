using Colyseus.Schema;

[System.Serializable]
public class PlayerData : Schema
{
    [Type(0, "float")]
    public float x = 0;

    [Type(1, "float")]
    public float y = 0;
}
