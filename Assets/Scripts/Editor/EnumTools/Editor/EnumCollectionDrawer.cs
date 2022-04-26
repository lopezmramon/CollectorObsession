using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using System.Reflection;
using UnityEditor.Callbacks;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Linq;
/// <summary>
/// Property Drawer for Enum Collection Type.
/// Displays a single entry of Type T for every value from enum TEnum.
/// Entry fields are labled with the toString of each value.
/// </summary>
/// <typeparam name="T">Type for entry fields.</typeparam>
/// <typeparam name="TEnum">enum for field lables.</typeparam>
public abstract class EnumCollectionDrawer<TEnum, T> : PropertyDrawer where TEnum : struct, IConvertible, IComparable, IFormattable
{
    //foldout memeory
    private static Dictionary<KeyValuePair<Object, string>, bool> foldout = new Dictionary<KeyValuePair<Object, string>, bool>();

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
        //insure key
        KeyValuePair<Object, string> foldoutkey = new KeyValuePair<Object, string>(property.serializedObject.targetObject, property.propertyPath);
        if (!foldout.ContainsKey(foldoutkey))
            foldout.Add(foldoutkey, false);

        //Get collection array
        SerializedProperty collectionArray = property.FindPropertyRelative("Collection");
        
        var extra = 0f;
        if(collectionArray != null)
            for (int i = 0; i < collectionArray.arraySize; i++)
            {
                var value = collectionArray.GetArrayElementAtIndex(i);
                extra += EditorGUI.GetPropertyHeight(value) - EditorGUIUtility.singleLineHeight;
            }

        //if foldout is open size extends
        return !foldout[foldoutkey] ? EditorGUIUtility.singleLineHeight : 
            collectionArray == null ? EditorGUIUtility.singleLineHeight * 2 :
            collectionArray.arraySize * EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight + extra;
	}

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //insure key
        KeyValuePair<Object, string> foldoutkey = new KeyValuePair<Object, string>(property.serializedObject.targetObject, property.propertyPath);
        if (!foldout.ContainsKey(foldoutkey))
            foldout.Add(foldoutkey, false);

        //the string values of the enumerator we are using
        string[] labels = Enum.GetNames(typeof(TEnum));

        //Get collection array
        SerializedProperty values = property.FindPropertyRelative("Collection");

        //insure size of array matches the size of the enum
        //without losing serialization.
        if (values != null)
        
        for (int i = values.arraySize; i < labels.Length; i++)
            values.InsertArrayElementAtIndex(i);

        if (values != null && values.arraySize > labels.Length)
            values.arraySize = labels.Length;

            //make sure values are updated
        if(values != null)
            values.serializedObject.ApplyModifiedProperties();

            //placement object
        Rect foldoutrect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            //is foldout open
        foldout[foldoutkey] = EditorGUI.Foldout(foldoutrect, foldout[foldoutkey], label);
        if (values != null && foldout[foldoutkey])
        {
            var offset = 0f;
            //print all of the property fields to the inspector
            for (int i = 0; i < values.arraySize; i++)
            {
                var value = values.GetArrayElementAtIndex(i);

                //how far down to print element
                float elementOffset = (EditorGUIUtility.singleLineHeight * (i + 1));

                //placement object
                Rect elementrect = new Rect(position.x + 15, position.y + elementOffset + offset,
                    position.width - 15, EditorGUIUtility.singleLineHeight);

                offset += EditorGUI.GetPropertyHeight(value) - EditorGUIUtility.singleLineHeight;
                //do print
                EditorGUI.PropertyField(elementrect, value, new GUIContent(labels[i]), true);
            }
        }
        else if(values == null && foldout[foldoutkey])
        {
            var box = new Rect(position.x + 15, position.y + EditorGUIUtility.singleLineHeight, position.width - 15, EditorGUIUtility.singleLineHeight);
            EditorGUI.HelpBox(box, "Unity is unable to searlize 2D collections. Please wrap your collection inside another class.", MessageType.Warning);
        }
    }
}

public static class DrawerHelper
{
    //file path to print the property drawers.
    private static string filepath = "Assets/Scripts/EnumTools/Editor/EnumCollectionDrawWrappers.cs";

    [DidReloadScripts]
    public static void OnScriptReload()
    {
        //current property drawers in editor assembly
        IEnumerable<Type> drawers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsSubclassOf(typeof(PropertyDrawer)) && 
                t.GetCustomAttributes<CustomPropertyDrawer>().Any());

        //Types based on GenericEnumCollection and the types uses in the generic
        //filtered to remove types that have been found in the drawers
        Dictionary<Type, Type[]> collectioninfos = Assembly.Load("Assembly-CSharp").GetTypes()
            .Where(t =>
                t != typeof(GenericEnumCollection<,>) && 
                isEnumCollection(t) &&
                !drawers.Any(d => d.GetCustomAttribute<CustomPropertyDrawer>()
                    .Match(new CustomPropertyDrawer(t))))
            .ToDictionary(t => t, t => enumCollectionArgs(t));

        //initialize list of lines to write to file
        List<string> toWrite = File.Exists(filepath) ? 
            new List<string>() { "" } : 
            new List<string>() { "using UnityEditor;\r\nusing UnityEngine;\r\n" };

        //foreach found add template
        foreach (var infos in collectioninfos)
        {
            foreach(var t in enumCollectionDrawerTemplate)
                toWrite.Add(t
                .Replace("{Wrapper}", infos.Key.Name)
                .Replace("{Enum}", infos.Value[0].Name)
                .Replace("{Object}", infos.Value[1].Name));
        }

        //remove the extra seperator line
        toWrite.RemoveAt(toWrite.Count - 1);

        //if we have at least one template in the list
        if (toWrite.Count > 2)
        {
            //write file
            File.AppendAllLines(filepath, toWrite);
            
            //make sure the file is noted and unity compiles
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
    }

    private static bool isEnumCollection(Type type)
    {
        //while type is not at root
        while (type != null && type != typeof(object))
            //check if type we want
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(GenericEnumCollection<,>))
                return true;
            //go up a level
            else
                type = type.BaseType;
        //didn't find type we wanted.
        return false;
    }

    private static Type[] enumCollectionArgs(Type t)
    {
        //while type is not at root
        while (t != typeof(object))
            //check if type we want
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(GenericEnumCollection<,>))
                return t.GetGenericArguments();
            //go up a level
            else
                t = t.BaseType;
        //didn't find type we wanted.
        return null;
    }

    //template for drawer & extra line for spacing.
    private static string[] enumCollectionDrawerTemplate =
    {
        "[CustomPropertyDrawer(typeof({Wrapper}))]",
        "public class {Wrapper}Drawer : EnumCollectionDrawer<{Enum}, {Object}> { }",
        ""
    };
}
