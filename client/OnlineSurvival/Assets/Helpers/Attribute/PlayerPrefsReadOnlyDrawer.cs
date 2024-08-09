using Assets.Helpers;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PlayerPrefsReadOnlyAttribute))]
public class PlayerPrefsReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        PlayerPrefsReadOnlyAttribute ReadOnlyAttribute = (PlayerPrefsReadOnlyAttribute)attribute;

        // Получаем значение имени игрока из PlayerPrefs
        string currentValue = PlayerPrefs.GetString(ReadOnlyAttribute.Key, property.stringValue);

        // Делаем поле недоступным для редактирования
        EditorGUI.LabelField(position, label.text, currentValue);
    }
}
