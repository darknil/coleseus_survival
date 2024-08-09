using TMPro;
using UnityEngine;

namespace OS.PlayerSystem
{
    public class PlayerNickname : MonoBehaviour
    {
        public TMP_Text Nickname;

        public void SetName(string name)
        {
            Nickname.text = name;
        }
    }
}
