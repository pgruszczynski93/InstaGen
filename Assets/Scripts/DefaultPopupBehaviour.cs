using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InstaGen
{
	public class DefaultPopupBehaviour : MonoBehaviour 
	{
		[SerializeField] private TMP_InputField _textField;

		private void OnEnable()
		{
			EventsHelper.OnHashtagButtonGenerate += CloseDefaultPopup;
		}

		private void OnDisable()
		{
			EventsHelper.OnHashtagButtonGenerate -= CloseDefaultPopup;
		}

		public void CloseDefaultPopup()
		{
			gameObject.SetActive(false);
		}
		public void OpenDefaultPopup()
		{
			_textField.text = "";
			gameObject.SetActive(true);
		}
	}

}
