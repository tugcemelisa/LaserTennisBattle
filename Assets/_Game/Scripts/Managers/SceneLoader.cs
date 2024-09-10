using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LaserTennis
{
	public class SceneLoader : SingleMonoBehaviour<SceneLoader>
	{
		public string StartSceneKey;

		private bool _clearPreviosScene;
		private SceneInstance _previousLoadedScene;
		private GameObject _createdGameObject;
		private AsyncOperationHandle<GameObject> _asyncGameObject;
		private AsyncOperationHandle<SceneInstance> _asyncScene;

		protected override void Awake()
		{
			base.Awake();
			LoadAddressableScene(StartSceneKey);
			Addressables.InitializeAsync();
		}

		public void AddressableCompleted(AssetReferenceGameObject assetRef)
		{
			_asyncGameObject = assetRef.LoadAssetAsync<GameObject>();
			_asyncGameObject.Completed += (asyncHandle) =>
			{
				if (asyncHandle.Result)
				{
					_createdGameObject = asyncHandle.Result;
					InstantiateAsync(_createdGameObject).completed += (asyncOperation) =>
					{
						Addressables.Release(_asyncGameObject);
					};
				}
			};
		}

		public void LoadAddressableScene(string key)
		{
			if (_clearPreviosScene)
			{
				Addressables.UnloadSceneAsync(_previousLoadedScene).Completed += (asyncHandle) =>
				{
					_clearPreviosScene = false;
					_previousLoadedScene = new SceneInstance();
					Debug.Log("Previous Scene Unloaded Successfully");
				};
			}

			_asyncScene = Addressables.LoadSceneAsync(key, LoadSceneMode.Additive);
			if (_asyncScene.IsValid())
			{
				_asyncScene.Completed += (asyncHandle) =>
				{
					_clearPreviosScene = true;
					_previousLoadedScene = asyncHandle.Result;
					Debug.Log($"{key} Scene Loaded Successfully");
				};
			}
		}
	}
}