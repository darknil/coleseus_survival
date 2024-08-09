using UnityEngine;

#if UNITY_EDITOR
public class PlayerPrefsAttribute : PropertyAttribute
{
    public string Key { get; private set; }
    public PlayerPrefsAttribute(string key)
    {
        Key = key;
    }
}
#endif
