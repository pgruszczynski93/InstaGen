using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace InstaGen
{
	public class ClipboardSender : MonoBehaviour {
		
		[SerializeField] TMP_InputField _summaryInputField;

		public void PasteToClipboard()
		{
			 AndroidNativeHelper.CopyTextToClipboard(_summaryInputField.text);
//			_summaryInputField.onSelect.AddListener(AndroidNativeHelper.CopyTextToClipboard());
		}
	}	
}

