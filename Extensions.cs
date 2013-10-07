using LibraryType = BLMyLibraryType;


/*

	Use:

	1) Change "BLMyLibraryType" to the proper type name of your library class
	2) Add the following manadatory actions to that same library and rebuild it:
		 - Success
		 - LogError
		 - Unplug
		 - Pause
		 - WaitForDebugger
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
		}


		static BehaveResult TickSuccessAction (Tree sender, string stringParameter, float floatParameter, IAgent agent, object data)
		{
			return BehaveResult.Success;
		}


		static BehaveResult TickLogErrorAction (Tree sender, string stringParameter, float floatParameter, IAgent agent, object data)
		{
			Debug.LogError (stringParameter);

			return BehaveResult.Success;
		}


		static BehaveResult TickUnplugAction (Tree sender, string stringParameter, float floatParameter, IAgent agent, object data)
		{
			sender.Plugged = false;

			return BehaveResult.Success;
		}


		static BehaveResult TickPauseAction (Tree sender, string stringParameter, float floatParameter, IAgent agent, object data)
		{
			Debug.Break ();

			return BehaveResult.Success;
		}


		static BehaveResult TickWaitForDebuggerAction (Tree sender, string stringParameter, float floatParameter, IAgent agent, object data)
		{
			return Behave.Debugger.Debugging.Local.ConnectionCount > 0 ? BehaveResult.Success : BehaveResult.Running;
		}
	}
}
