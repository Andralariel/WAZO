using UnityEditor;
using UnityEngine;

namespace TechArt.Editor
{
	public class RandomlyPlaceObjectsOnSurface : EditorWindow
	{
		private string objectBaseName = "";
		//public GameObject[] objectsToSpawn;
		//public float spawnRadius = 10.0f;
		private int numberOfObjects = 10;
		//public bool randomOrientation = false;
		//private bool orientToSurface = false;
		private GameObject objectToSpawn;
		private GameObject zoneToSpawn;



		[MenuItem("Tools/Item Spawner")]
		public static void ShowWindow()
		{
			GetWindow(typeof(RandomlyPlaceObjectsOnSurface));
		}

		private void OnGUI()
		{
			GUILayout.Label("Spawn New Object", EditorStyles.boldLabel);

			objectBaseName = EditorGUILayout.TextField("Base Name", objectBaseName);
			numberOfObjects = EditorGUILayout.IntField("Number of Object", numberOfObjects);
			objectToSpawn = EditorGUILayout.ObjectField("Prefab to Spawn", objectToSpawn,typeof(GameObject), false) as GameObject;

			if (GUILayout.Button("SPAAAAWN"))
			{
				SpawnObject();
			}
		}
	
		private void SpawnObject()
		{
			Debug.Log("Bonjour" + numberOfObjects );
		}
	
		// Use this for initialization
		/*
	void Start ()
	{
		for(int i = 0; i < numberOfObjects; i++)
		{
			//What we will spawn
			GameObject objectToSpawn = objectsToSpawn[Random.Range(0,objectsToSpawn.Length)];

			Vector2 spawnPositionV2 = Random.insideUnitCircle*spawnRadius;

			Vector3 spawnPosition = new Vector3(spawnPositionV2.x,0.0f,spawnPositionV2.y);

			Vector3 transformOffsetSpawnPosition = transform.position+spawnPosition;

			RaycastHit hit;
			if(Physics.Raycast(transformOffsetSpawnPosition,Vector3.down,out hit))
			{
				Vector3 finalSpawnPosition = hit.point;

				Quaternion orientation;
				
				if(randomOrientation)
				{
					orientation = Random.rotation;
				}
				else if(orientToSurface)
				{
					orientation = Quaternion.LookRotation(hit.normal);
				}
				else
				{
					orientation = objectToSpawn.transform.rotation;
				}

				Instantiate(objectToSpawn,finalSpawnPosition,orientation);
			}
		}
	}
	*/
	}
}
