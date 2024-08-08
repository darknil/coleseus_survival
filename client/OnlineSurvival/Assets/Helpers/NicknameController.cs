using TMPro;
using UnityEngine;

namespace Assets.Helpers
{
    [RequireComponent(typeof(TMP_InputField))]
    public class NicknameController : MonoBehaviour
    {
        public const string NICKNAME_KEY = "nickname";

        public TMP_InputField Input;

        public void Awake()
        {
            Input = Input == null ? GetComponent<TMP_InputField>() : Input;
        }

        private void Start()
        {
            Input.onEndEdit.AddListener(NickChange);

            string nickname = PlayerPrefs.GetString(NICKNAME_KEY);
            Input.text = nickname;
        }

        private void NickChange(string name)
        {
            PlayerPrefs.SetString(NICKNAME_KEY, name);
        }
    }
}
