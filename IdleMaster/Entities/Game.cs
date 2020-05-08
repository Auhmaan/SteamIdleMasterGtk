using System.Diagnostics;
using System.Globalization;

namespace IdleMaster.Entities
{
    public class Game
    {
        //Fields
        private Process idleProcess;

        private readonly int originalRemainingCards;

        private int fastIdleTries;

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

        public IdleStyle CurrentIdleStyle { get; private set; }

        //Constructors
        public Game(string appId, string name, string remaining, string hours)
        {
            AppId = appId;
            Name = name;

            int.TryParse(remaining, out originalRemainingCards);
            UpdateStats(remaining, hours);

            SetToNormalIdling();
        }

        //Methods
        private void SetToNormalIdling()
        {
            CurrentIdleStyle = IdleStyle.Normal;
            fastIdleTries = 0;
        }

        private void SetToFastIdling()
        {
            CurrentIdleStyle = IdleStyle.Fast;
            fastIdleTries = 2;
        }

        public void StartIdling()
        {
            if (IsIdling)
            {
                return;
            }

            idleProcess = Process.Start(new ProcessStartInfo("steam-idle.exe", AppId)
            {
                WindowStyle = ProcessWindowStyle.Hidden
            });
        }

        public void StopIdling()
        {
            if (!IsIdling)
            {
                return;
            }

            idleProcess.Kill();
            idleProcess.Dispose();
            idleProcess = null;
        }

        public void UpdateStats(string remaining, string hours)
        {
            int.TryParse(remaining, out int remainingCards);
            double.TryParse(hours, NumberStyles.Any, new NumberFormatInfo(), out double hoursPlayed);

            RemainingCards = remainingCards;
            HoursPlayed = hoursPlayed;

            if (CurrentIdleStyle == IdleStyle.Fast)
            {
                fastIdleTries--;

                if (fastIdleTries == 0 && RemainingCards == originalRemainingCards)
                {
                    SetToNormalIdling();
                }
            }
        }
    }
}