using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

namespace justbake.sceneloader
{
    public class SceneManager : MonoBehaviour
    {
		#region Singleton
		public static SceneManager Instance { get; private set; }
		private void Awake()
		{
			// If there is an instance, and it's not me, delete myself.

			if (Instance != null && Instance != this)
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}

			DontDestroyOnLoad(this);

			Application.runInBackground = true;
		}
		#endregion

		#region public properties
		public GameObject loadinScreen;
		public Slider progressBar;
		public TMP_Text textField;
		#endregion

		#region private properties
		private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
		private float totalSceneProgress;
		private float totalBaseSceneProgress;
		#endregion

		public void LoadScene(int buildIndex, int[] scenesToUnload = null)
		{
			loadinScreen.gameObject.SetActive(true);
			if(scenesToUnload != null)
			{
				foreach(int index in scenesToUnload)
				{
					scenesLoading.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(index));
				}
			}
			scenesLoading.Add(UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive));
			StartCoroutine(GetSceneLoadProgress());
			StartCoroutine(GetTotalProgress());
		}

		public void LoadScene(int buildIndex)
		{
			loadinScreen.gameObject.SetActive(true);
			scenesLoading.Add(UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive));
			StartCoroutine(GetSceneLoadProgress());
			StartCoroutine(GetTotalProgress());
		}

		#region progress
		public IEnumerator GetSceneLoadProgress()
		{
			for (int i = 0; i < scenesLoading.Count; i++)
			{
				while (scenesLoading[i] != null && !scenesLoading[i].isDone)
				{
					totalSceneProgress = 0;

					foreach (AsyncOperation operation in scenesLoading)
					{
						if (operation != null)
							totalSceneProgress += operation.progress;
					}

					totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;

					if(textField!=null)
						textField.text = string.Format("Loading {0}%", totalSceneProgress);

					yield return null;
				}
			}

		}

		public IEnumerator GetTotalProgress()
		{
			float totalProgress = 0;

			while (BaseSceneLoader.Instance == null || !BaseSceneLoader.Instance.isDone)
			{
				if (BaseSceneLoader.Instance == null)
				{
					totalProgress = 0;
				}
				else
				{
					totalBaseSceneProgress = Mathf.Round(BaseSceneLoader.Instance.progress * 100f);
					if (textField != null)
						textField.text = string.Format("Loading {1}: {0}%", totalBaseSceneProgress, BaseSceneLoader.Instance.currentTask);
				}

				totalProgress = Mathf.Round((totalBaseSceneProgress + totalSceneProgress) / 2f);
				if(progressBar!=null)
					progressBar.value = Mathf.RoundToInt(totalProgress);

				yield return null;
			}

			BaseSceneLoader.Instance = null;
			loadinScreen.gameObject.SetActive(false);
		}
		#endregion
	}
}
