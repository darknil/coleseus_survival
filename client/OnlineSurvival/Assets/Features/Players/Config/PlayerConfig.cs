using System;
using UnityEngine;

namespace Game.Players
{
    [Serializable]
    public class PlayerConfig
    {
#if UNITY_EDITOR
        [PlayerPrefsReadOnly(Association.PlayerPrefs.PLAYER_NAME)]
#endif
        [SerializeField] private string ReadName;
#if UNITY_EDITOR
        [PlayerPrefs(Association.PlayerPrefs.PLAYER_NAME)]
#endif
        public string Nickname;
        public float Speed = 5f;
    }
}
