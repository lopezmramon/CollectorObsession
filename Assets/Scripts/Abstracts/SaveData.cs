using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
	public int slot;
	public string saveName;
	public bool HasBeenSavedIn = false;
	public float playTime;
	

	public SaveData()
	{
		saveName = "New Game";	
	}


	public Dictionary<ResourceType, int> LoadedResources => new Dictionary<ResourceType, int>()
	{
		
	};
}