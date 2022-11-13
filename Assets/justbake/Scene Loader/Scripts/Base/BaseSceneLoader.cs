using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace justbake.sceneloader
{
    public abstract class BaseSceneLoader : MonoBehaviour
    {
		#region singleton
		public static BaseSceneLoader Instance;
		#endregion

		#region public properties

		#region Loading
		public float progress { get; protected set; }
		public bool isDone { get; protected set; }
		public string currentTask { get; protected set; }
		#endregion

		#endregion

		#region MonoBehaviour
		protected virtual void Awake()
		{
			//set instance to this
			Instance = this;
		}

		protected virtual void Start()
		{
			StartCoroutine(Load());
		}

		#endregion


		#region Loading

		private IEnumerator Load()
		{
			yield return LoadScene();
			isDone = true;
		}

		protected abstract IEnumerator LoadScene();

		#endregion
	}
}
