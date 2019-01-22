using System;
using UnityEngine;

namespace InstaGen
{
	public static class EventsHelper
	{
		public static Action OnApplicationExit = delegate { };
			
		public static Action OnSwipeEnabled = delegate { };
		public static Action OnInputPanelScrollFinished = delegate {  };
		public static Action OnHashtagsTextsGenerate = delegate{ };
		
		public static Action<MoveDirection> OnScroll = delegate {  };
		public static Action<SwipeData> OnSwipe = delegate { };
		public static Action<Vector2> OnObjectDrag = delegate { }; 
		
	}
}
