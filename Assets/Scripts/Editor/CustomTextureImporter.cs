using UnityEngine;
using UnityEditor;
public class CustomTextureImporter : AssetPostprocessor
{
	void OnPreprocessTexture()
	{
		Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D));
		
		if (asset)
		{
			EditorUtility.SetDirty(asset);
		}
		else
		{
			TextureImporter importer = assetImporter as TextureImporter;
			importer.anisoLevel = 0;
			importer.filterMode = FilterMode.Point;
			importer.mipmapEnabled = false;
			importer.spritePixelsPerUnit = 16f;
			importer.textureCompression = TextureImporterCompression.Uncompressed;
		}
	}
}
