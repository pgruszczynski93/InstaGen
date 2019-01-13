using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InstaGen
{
	public class HashtagPanelManager : MonoBehaviour
	{
		[SerializeField] private Button _openAddHashtagButton;
		[SerializeField] private GameObject _hashtagPopup;
		[SerializeField] private TMP_InputField _hashtagText;

		private void OnEnable()
		{
			EventsHelper.OnHashtagButtonGenerate += CloseHashtagPopup;
		}

		private void OnDisable()
		{
			EventsHelper.OnHashtagButtonGenerate -= CloseHashtagPopup;
		}

		public void CloseHashtagPopup()
		{
			_hashtagPopup.SetActive(false);
		}
		public void OpenHashtagPopup()
		{
			_hashtagText.text = "";
			_hashtagPopup.SetActive(true);
		}
	}

}
