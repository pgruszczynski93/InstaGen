using System;

namespace InstaGen
{
	public static class EventsHelper
	{
		public static Action OnApplicationExit = delegate { };
		public static Action OnSwipeEnabled = delegate { };
		public static Action OnInputPanelScrollFinished = delegate {  };
		
		public static Action<MoveDirection> OnScroll = delegate {  };
		public static Action<SwipeData> OnSwipe = delegate { };
	}
}
