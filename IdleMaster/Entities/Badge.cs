using System.Diagnostics;
using System.Globalization;

namespace IdleMaster.Entities
{
	public class Badge
	{
		//Fields
		private Process idleProcess;

		//Properties
		public string AppId { get; }

		public string Name { get; }

		public int RemainingCards { get; private set; }

		public double HoursPlayed { get; private set; }

		public bool HasDrops
		{
			get
			{
				return RemainingCards > 0;
			}
		}

		public bool IsIdling
		{
			get
			{
				return idleProcess != null && !idleProcess.HasExited;
			}
		}

		//Constructors
		public Badge(string appId, string name, string remaining, string hours)
		{
			AppId = appId;
			Name = name;
			UpdateStats(remaining, hours);
		}

		//Methods
		public Process StartIdle()
		{
			if (IsIdling)
			{
				return idleProcess;
			}

			idleProcess = Process.Start(new ProcessStartInfo("steam-idle.exe", AppId)
			{
				WindowStyle = ProcessWindowStyle.Hidden
			});

			return idleProcess;
		}

		public void StopIdle()
		{
			if (!IsIdling)
			{
				return;
			}

			idleProcess.Kill();
		}

		public void UpdateStats(string remaining, string hours)
		{
			int.TryParse(remaining, out int remainingCards);
			double.TryParse(hours, NumberStyles.Any, new NumberFormatInfo(), out double hoursPlayed);

			RemainingCards = remainingCards;
			HoursPlayed = hoursPlayed;
		}
	}
}