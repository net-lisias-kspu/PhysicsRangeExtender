using System;
using UnityEngine;

namespace PhysicsRangeExtender
{
	public static class Debug
	{
		private static readonly KSPe.Util.Log.Logger log = KSPe.Util.Log.Logger.CreateForType<PhysicsRangeExtender>();
		
		public static void Log(string msg)
		{
			log.info(msg);
		}
		
		public static void LogError(string msg)
		{
			log.error(msg);
		}
	}
}
