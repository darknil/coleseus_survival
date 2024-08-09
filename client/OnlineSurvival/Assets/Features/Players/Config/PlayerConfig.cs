using System;
using UnityEngine;

namespace OS.Players
{
    [Serializable]
    public class PlayerConfig
    {
        [PlayerPrefsReadOnly(Association.PlayerPrefs.PLAYER_NAME)
            , SerializeField] private string ReadName;
        [PlayerPrefs(Association.PlayerPrefs.PLAYER_NAME)] public string Nickname;
        public float Speed = 5f;
    }
}
