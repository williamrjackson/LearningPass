using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Wrj
{
	public class ActionSeries : MonoBehaviour 
	{
		[TextArea(3, 10)]
		[SerializeField]
		private string description = "Enter A Description...";
		[SerializeField]
		private bool begin = false;
		[SerializeField]
		private DelayedAction[] actions;


		private void Update ()
		{
			// If begin is checked already on Start, the sequence will start immediately. 
			// Otherwise click the toggle to start.
			if (begin)
			{
				// reset bool to prevent repetition
				begin = false;
				// Run sequence
				StartCoroutine(RunSequence());
			}
		}
		private IEnumerator RunSequence()
		{
			foreach(DelayedAction executeMe in actions)
			{
				// Wait...
				yield return new WaitForSecondsRealtime(executeMe.delay);
				// Execute
				executeMe.action.Invoke();
			}
		}
		[System.Serializable]
		private class DelayedAction
		{
			public float delay = 1f;
			public UnityEvent action;
		}
	}
}
