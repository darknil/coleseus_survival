using UnityEngine;

public class PlayerPrefsReadOnlyAttribute : PropertyAttribute
{
    public string Key { get; private set; }
    public PlayerPrefsReadOnlyAttribute(string key)
    {
        Key = key;
    }
}
