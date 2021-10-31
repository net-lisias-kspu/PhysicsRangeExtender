/*
	This file is part of Physics Range Extender /L Unleashed
		© 2018-2021 Lisias T : http://lisias.net <support@lisias.net>

	THIS FILE is licensed to you under:

		* WTFPL - http://www.wtfpl.net
			* Everyone is permitted to copy and distribute verbatim or modified
 				copies of this license document, and changing it is allowed as long
				as the name is changed.

	THIS FILE is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/
using System.Diagnostics;
using KSPe.Util.Log;

namespace PhysicsRangeExtender
{
	internal static class Log
	{
		private static readonly Logger log = Logger.CreateForType<Startup>(1);

		internal static void force (string msg, params object [] @params)
		{
			log.force (msg, @params);
		}

		internal static void info (string msg, params object [] @params)
		{
			log.info (msg, @params);
		}

		internal static void warn (string msg, params object [] @params)
		{
			log.warn (msg, @params);
		}

		internal static void detail (string msg, params object [] @params)
		{
			log.detail (msg, @params);
		}

		internal static void trace (string msg, params object [] @params)
		{
			log.trace (msg, @params);
		}

		internal static void error (System.Exception e, object offended)
		{
			log.error(offended, e);
		}

		internal static void error (string msg, params object [] @params)
		{
			log.error (msg, @params);
		}

		[ConditionalAttribute ("DEBUG")]
		internal static void dbg (string msg, params object [] @params)
		{
			log.trace (msg, @params);
		}
	}
}
