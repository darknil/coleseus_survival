using Colyseus.Schema;

namespace OS.PlayerSystem
{
    public class PlayerData : Schema
    {
        [Type(0, "string")]
        public string name;

        [Type(1, "number")]
        public float x = 0;

        [Type(2, "number")]
        public float y = 0;
    }
}
