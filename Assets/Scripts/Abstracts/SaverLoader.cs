using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public static class SaverLoader
{
	private static string path = Application.dataPath + "/_Resources/Saves/";

	public static string Path => path;

	public static void Save(SaveData currentSaveData, int slot)
	{
		string saveText = JsonUtility.ToJson(currentSaveData);
		File.WriteAllText(path + $"{slot}.json", saveText);
	}

	public static SaveData LoadSave(int slot)
	{
		SaveData saveData = null;
		/*if (File.Exists(path + $"{slot}.json"))
		{
			string json = File.ReadAllText(path + $"{slot}.json");
			saveData = JsonUtility.FromJson<SaveData>(json);
		}*/
		saveData = CheckSaves()[slot];
		return saveData;
	}

	public static List<SaveData> CheckSaves()
	{
		if (!Directory.Exists(Path))
		{
			Directory.CreateDirectory(Path);
		}
		string[] paths = Directory.GetFiles(Path, "*.json", SearchOption.TopDirectoryOnly);
		SaveData[] saves = new SaveData[3];
		for (int i = 0; i < paths.Length; i++)
		{
			if (System.IO.Path.GetExtension(paths[i]) == ".json")
			{
				SaveData save = JsonUtility.FromJson<SaveData>(File.ReadAllText(paths[i]));
				saves[save.slot] = save;
			}
		}
		for (int i = 0; i < saves.Length; i++)
		{
			if (saves[i] == null)
			{
				int index = i;
				saves[i] = new SaveData();
				saves[i].slot = index;
			}
		}
		List<SaveData> orderedList = saves.ToList();
		orderedList = orderedList.OrderBy(o => o.slot).ToList();
		return orderedList;
	}

	public static void DeleteSave(int slot, Action OnDeleted)
	{
		if(File.Exists(path + $"{slot}.json"))
		File.Delete(path + $"{slot}.json");
		OnDeleted?.Invoke();
	}
}
