using TMPro;
using UnityEngine;

namespace Assets.Helpers
{
    [RequireComponent(typeof(TMP_InputField))]
    public class NicknameController : MonoBehaviour
    {
        public TMP_InputField Input;

        public void Awake()
        {
            Input = Input == null ? GetComponent<TMP_InputField>() : Input;
        }

        private void Start()
        {
            Input.onEndEdit.AddListener(NickChange);

            string nickname = PlayerPrefs.GetString(Association.PlayerPrefs.PLAYER_NAME);
            Input.text = nickname;
        }

        private void NickChange(string name)
        {
            PlayerPrefs.SetString(Association.PlayerPrefs.PLAYER_NAME, name);
        }
    }
}
