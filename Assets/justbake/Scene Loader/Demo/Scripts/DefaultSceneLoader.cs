using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace justbake.sceneloader
{
	public class DefaultSceneLoader : BaseSceneLoader
	{
		public float waitTime = 0.5f;

		protected override IEnumerator LoadScene()
		{
			currentTask = "Default Scene";

			progress += 1f;
			yield return new WaitForSeconds(waitTime);
			//yield return null;
		}
	}
}
