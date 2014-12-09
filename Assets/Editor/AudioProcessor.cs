using UnityEngine;
using UnityEditor;
using System.Collections;

public class AudioProcessor : AssetPostprocessor
{

	void OnPreprocessAudio()
	{
		AudioImporter importer = (AudioImporter)assetImporter;
		importer.threeD = false;
	}

}

