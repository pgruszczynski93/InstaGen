﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InstaGen
{
	public class AppManager : MonoBehaviour {
		private void OnEnable()
		{
			EventsHelper.OnApplicationExit += ExitApp;
		}

		private void OnDisable()
		{
			EventsHelper.OnApplicationExit -= ExitApp;
		}

		void Update ()
		{
			if (EventsHelper.OnApplicationExit != null)
			{
				EventsHelper.OnApplicationExit();
			}
		}

		void ExitApp()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				Application.Quit();
			}
		}
	}	
}
