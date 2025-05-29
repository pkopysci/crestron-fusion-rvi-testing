using Crestron.SimplSharp;
using Crestron.SimplSharpPro.Fusion;
using Crestron.SimplSharpPro;

namespace CrestronTestNet6
{
	public class ControlSystem : CrestronControlSystem
	{
		public ControlSystem()
		{
			CrestronEnvironment.ProgramStatusEventHandler += ControllerProgramEventHandler;
			CrestronConsole.AddNewConsoleCommand(Test, "test", "", ConsoleAccessLevelEnum.AccessOperator);
		}

		public override void InitializeSystem()
		{
			try
			{
			}
			catch (Exception ex)
			{
				ErrorLog.Error($"Error initializing system: {ex}");
			}
		}

		private void Test(object obj)
		{
			CrestronConsole.PrintLine("Test()");

			try
			{
				var fusion = new FusionRoom(0x30, this, "Fusion Test", "01c57d66-21d4-4aeb-ae55-5ae6a4664ac9");

				fusion.AddSig(eSigType.Bool, 50, "Video Mute", eSigIoMask.InputOutputSig);
				fusion.AddSig(eSigType.Bool, 51, "Video Freeze", eSigIoMask.InputOutputSig);
				fusion.AddSig(eSigType.Bool, 52, "Select HDMI", eSigIoMask.InputOutputSig);
				fusion.AddSig(eSigType.Bool, 53, "Podium Mute", eSigIoMask.InputOutputSig);
				fusion.AddSig(eSigType.Bool, 54, "Volume Mute", eSigIoMask.InputOutputSig);
				fusion.AddSig(eSigType.Bool, 55, "Mic Mute", eSigIoMask.InputOutputSig);
				fusion.AddSig(eSigType.Bool, 56, "Select PC", eSigIoMask.InputOutputSig);
				fusion.AddSig(eSigType.Bool, 57, "Select VGA", eSigIoMask.InputOutputSig);
				fusion.AddSig(eSigType.UShort, 50, "Audio Levels", eSigIoMask.InputOutputSig);
				fusion.AddSig(eSigType.UShort, 51, "Lamp Hours", eSigIoMask.InputSigOnly);

				FusionRVI.GenerateFileForAllFusionDevices();
				if (fusion.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
				{
					ErrorLog.Error("FusionInterface.Initialize() - Failed to register connection: {0}",
						fusion.RegistrationFailureReason);
				}
			}
			catch (Exception exception)
			{
				CrestronConsole.PrintLine($"{exception.Message} -  {exception.StackTrace?.Replace("\n", "\r\n")}");
			}
		}

		private void ControllerProgramEventHandler(eProgramStatusEventType programStatusEventType)
		{
			switch (programStatusEventType)
			{
				case (eProgramStatusEventType.Paused):
					//The program has been paused.  Pause all user threads/timers as needed.
					break;
				case (eProgramStatusEventType.Resumed):
					//The program has been resumed. Resume all the user threads/timers as needed.
					break;
				case (eProgramStatusEventType.Stopping):
					//_sshClient?.Dispose();
					break;
			}
		}
	}
}
