using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(EnumFilterAttribute))]
public class EnumFilterDrawer : PropertyDrawer
{
    Dictionary<string, FilterInfo> filter = new Dictionary<string, FilterInfo>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var valid = property.propertyType == SerializedPropertyType.Enum;

        if (!valid)
            EditorGUI.LabelField(position, label.text, "Use EnumFilter on Enum Types");
        else
        {
            if (!filter.ContainsKey(property.propertyPath))
                filter.Add(property.propertyPath, new FilterInfo());

            float leftOverSpace = position.width - EditorGUIUtility.labelWidth;

            var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);

            var filterRect = new Rect(labelRect.x + labelRect.width, position.y, leftOverSpace * 0.25f, position.height);

            var popupRect = new Rect(filterRect.x + filterRect.width, position.y, leftOverSpace * 0.75f, position.height);
            
            EditorGUI.LabelField(labelRect, label);

            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            filter[property.propertyPath].filter = EditorGUI.TextField(filterRect, filter[property.propertyPath].filter);

            var options = property.enumDisplayNames;

            var selected = property.enumValueIndex;

            var filtered = options.Select((s, i) => new { s, i }).
                Where(o => string.IsNullOrWhiteSpace(filter[property.propertyPath].filter) || 
                o.s.ToLower().Contains(filter[property.propertyPath].filter.ToLower()) ||
                o.i == selected);

            filter[property.propertyPath].filteredSelection = filtered.Select(o => o.i).ToArray();
            options = filtered.Select(o => o.s).ToArray();

            selected = filter[property.propertyPath].filteredSelection.
                Select((j, i) => new { j, i }).
                Where(o => o.j == selected).
                Select(o => o.i).
                FirstOrDefault();

            var temp = EditorGUI.Popup(popupRect, selected, options);

            property.enumValueIndex = filter[property.propertyPath].filteredSelection[temp];

            EditorGUI.indentLevel = indentLevel;
        }
    }

    class FilterInfo
    {
        public string filter;
        public int[] filteredSelection;
    }
}