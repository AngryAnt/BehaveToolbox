using LibraryType = BLMyLibraryType;
using UnityEngine;
using System.Collections.Generic;


/*

	Use:

	1) Change "BLMyLibraryType" (line 1) to the proper type name of your library class
	2) Add the following manadatory actions to that same library and rebuild it:
		 - Success
		 - LogError
		 - Unplug
		 - Pause
		 - WaitForDebugger
		 - SendMessage
		 - Wait
	3) Call myTree.WireDefaults (); on new tree instances.

*/


namespace Behave.Runtime
{
	public static class Extensions
	{
		public static LibraryType.ContextType GetContext (this Tree tree)
		{
			return (LibraryType.ContextType)tree.ActiveContext;
		}


		public static void WireDefaults (this Tree tree)
		{
			tree.SetTickForward ((int)LibraryType.ActionType.Success, TickSuccessAction);
			tree.SetTickForward ((int)LibraryType.ActionType.LogError, TickLogErrorAction);
			tree.SetTickForward ((int)LibraryType.ActionType.Unplug, TickUnplugAction);
			tree.SetTickForward ((int)LibraryType.ActionType.Pause, TickPauseAction);
			tree.SetTickForward ((int)LibraryType.ActionType.WaitForDebugger, TickWaitForDebuggerAction);
			tree.SetTickForward ((int)LibraryType.ActionType.SendMessage, TickSendMessageAction);
			tree.SetInitForward ((int)LibraryType.ActionType.Wait, InitWaitAction);
			tree.SetTickForward ((int)LibraryType.ActionType.Wait, TickWaitAction);
			tree.SetResetForward ((int)LibraryType.ActionType.Wait, ResetWaitAction);
		}


		static BehaveResult TickSuccessAction (Tree sender)
		{
			return BehaveResult.Success;
		}


		static BehaveResult TickLogErrorAction (Tree sender)
		{
			Debug.LogError (sender.ActiveStringParameter);

			return BehaveResult.Success;
		}


		static BehaveResult TickUnplugAction (Tree sender)
		{
			sender.Plugged = false;

			return BehaveResult.Success;
		}


		static BehaveResult TickPauseAction (Tree sender)
		{
			Debug.Break ();

			return BehaveResult.Success;
		}


		static BehaveResult TickWaitForDebuggerAction (Tree sender)
		{
			return Behave.Debugger.Debugging.Local.ConnectionCount > 0 ? BehaveResult.Success : BehaveResult.Running;
		}


		static BehaveResult TickSendMessageAction (Tree sender)
		{
			MonoBehaviour behaviour = sender.ActiveAgent as MonoBehaviour;

			if (behaviour == null)
			{
				return BehaveResult.Failure;
			}

			behaviour.SendMessage (sender.ActiveStringParameter, sender.ActiveFloatParameter);

			return BehaveResult.Success;
		}


		static Dictionary<LibraryType.ContextType, float> s_Timers = new Dictionary<LibraryType.ContextType, float> ();
		static BehaveResult InitWaitAction (Tree sender)
		{
			if (sender.ActiveFloatParameter < 0.0f)
			{
				return BehaveResult.Failure;
			}

			s_Timers[sender.GetContext ()] = Time.time + sender.ActiveFloatParameter;

			return BehaveResult.Success;
		}


		static BehaveResult TickWaitAction (Tree sender)
		{
			return Time.time < s_Timers[sender.GetContext ()] ? BehaveResult.Running : BehaveResult.Success;
		}


		static void ResetWaitAction (Tree sender)
		{
			s_Timers.Remove (sender.GetContext ());
		}
	}
}
