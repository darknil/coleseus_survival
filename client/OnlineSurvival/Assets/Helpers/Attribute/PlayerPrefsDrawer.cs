using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(PlayerPrefsAttribute))]
public class PlayerPrefsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        PlayerPrefsAttribute playerPrefsAttribute = (PlayerPrefsAttribute)attribute;

        // Отображаем метку и текущее значение из PlayerPrefs
        string currentValue = PlayerPrefs.GetString(playerPrefsAttribute.Key, property.stringValue);
        string newValue = EditorGUI.TextField(position, label.text, currentValue);

        // Если значение изменилось, обновляем PlayerPrefs
        if (newValue != currentValue)
        {
            PlayerPrefs.SetString(playerPrefsAttribute.Key, newValue);
            PlayerPrefs.Save();

            // Обновляем значение поля, чтобы отобразить его корректно в инспекторе
            property.stringValue = newValue;
        }
    }
}
#endif