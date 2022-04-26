using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

[CustomPropertyDrawer(typeof(Sample))]
public class SampleDrawer : EnumCollectionDrawer<ColorKey, Color> { }