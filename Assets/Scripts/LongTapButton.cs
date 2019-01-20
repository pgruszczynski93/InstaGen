using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InstaGen
{
	public class LongTapButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
	{
		[SerializeField] private bool _isTapStarted;

		[SerializeField] private float _tapTreshold = 0.4f;
		[SerializeField] private float _tapTimer;

		[SerializeField] private Button _buttonRoot;
		[SerializeField] public UnityEvent _onLongClickEvent;
		
		public void OnPointerUp(PointerEventData eventData)
		{
			ResetLongTap();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			StartLongTap();
		}

		private void Update()
		{
			if (_isTapStarted == false)
			{
				return;
			}

			InvokeLongTap();
		}

		private void InvokeLongTap()
		{
			_tapTimer += Time.deltaTime;

			if (_tapTimer < _tapTreshold || _onLongClickEvent == null)
			{
				return;
			}
			
			Debug.Log("Long click");
			_onLongClickEvent.Invoke();
			ResetLongTap();
			
		}

		public void test()
		{
			Debug.Log("test");
		}
		private void StartLongTap()
		{
			ResetLongTap();
			_isTapStarted = true;
			_buttonRoot.enabled = false;
			Debug.Log("Start");
		}

		private void ResetLongTap()
		{
			
			_tapTimer = 0f;
			_isTapStarted = false;
			_buttonRoot.enabled = true;
			Debug.Log("Stop[");

		}
	}


}
