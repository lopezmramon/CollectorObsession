using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 0)]
public class ItemData : ScriptableObject
{
	public List<Item> Items;
}
