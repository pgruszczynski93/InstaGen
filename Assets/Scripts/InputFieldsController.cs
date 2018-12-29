using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InstaGen
{
	public class InputFieldsController : MonoBehaviour
	{
		private bool _isPanelScrollPossible;
		private int _inputCount;
		
		[SerializeField] private InputPanelScroller _inputPanelScroller;
		[SerializeField] private TMP_InputField[] _scrollingInputFields;
		[SerializeField] private TMP_InputField[] _normalInputFields;

		private void Start()
		{
			_inputCount = _scrollingInputFields.Length;
		}
		
		private void OnEnable()
		{
			EventsHelper.OnInputPanelScrollFinished += CopyUserInput;			

			for(int i=0; i<_inputCount; i++)
			{
				_scrollingInputFields[i].onSelect.AddListener(_inputPanelScroller.ScrollUpPanel);
			}
		}

		private void OnDisable()
		{
			EventsHelper.OnInputPanelScrollFinished -= CopyUserInput;			

			for(int i=0; i<_inputCount; i++)
			{
				_scrollingInputFields[i].onSelect.RemoveAllListeners();
			}
		}
		
		public TMP_InputField CurrentInputField
		{
			get { return _scrollingInputFields[InputContentScroller.VerticalContentScrollIndex]; }
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

		private void CopyUserInput()
		{
			for(int i=0; i<_inputCount; i++)
			{
				_normalInputFields[i].text = _scrollingInputFields[i].text;
			}
		}
	}
}
