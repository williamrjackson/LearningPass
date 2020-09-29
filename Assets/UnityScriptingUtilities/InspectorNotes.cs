using UnityEngine;

namespace Wrj
{
	public class InspectorNotes : MonoBehaviour 
	{
		[TextArea(20, 10)]
		[SerializeField]
		private string Notes = "Enter Notes...";
		private void Start()
		{
			Utils.SupressUnusedVarWarning(Notes);
		}
	}		
}
