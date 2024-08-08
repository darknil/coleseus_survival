using Colyseus.Schema;

namespace OS.PlayerSystem
{
    [System.Serializable]
    public class PlayerData : Schema
    {
        [Type(0, "string")]
        public string name;

        [Type(1, "float")]
        public float x = 0;

        [Type(2, "float")]
        public float y = 0;
    }
}
