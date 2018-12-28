using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InstaGen
{
	public class InputFieldsController : MonoBehaviour
	{
		private bool _isPanelScrollPossible;
		
		[SerializeField] private InputPanelScroller _inputPanelScroller;
		[SerializeField] private TMP_InputField[] _inputFields;

		private void OnEnable()
		{
			foreach (TMP_InputField field in _inputFields)
			{
				field.onSelect.AddListener(_inputPanelScroller.ScrollUpPanel);
				field.onValueChanged.AddListener(GetUserInput);
			}
		}

		private void OnDisable()
		{
			foreach (TMP_InputField field in _inputFields)
			{
				field.onSelect.RemoveAllListeners();
			}
		}
		
		public TMP_InputField CurrentInputField
		{
			get { return _inputFields[InputContentScroller.VerticalContentScrollIndex]; }
		}

		public void EnableInputField(string tempString)
		{
			CurrentInputField.interactable = true;
		}

		public void DisableInputField(string tempString)
		{
			CurrentInputField.interactable = false;
		}

		public bool IsPanelScrollPossible
		{
			get { return _isPanelScrollPossible; }
		}

		public void GetUserInput(string userInput)
		{
			print("Dupax xsdafkdfmd");
		}
	}
}
