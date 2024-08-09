using UnityEngine;

public class PlayerPrefsAttribute : PropertyAttribute
{
    public string Key { get; private set; }
    public PlayerPrefsAttribute(string key)
    {
        Key = key;
    }
}
