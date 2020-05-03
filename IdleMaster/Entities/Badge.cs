using System.Diagnostics;
using System.Globalization;

namespace IdleMaster.Entities
{
    public class Badge
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

        public bool IsNormalIdling { get; private set; }

        public bool IsFastIdling { get; private set; }

        //Constructors
        public Badge(string appId, string name, string remaining, string hours)
        {
            AppId = appId;
            Name = name;
            int.TryParse(remaining, out originalRemainingCards);

            SetToFastIdling();

            UpdateStats(remaining, hours);
        }

        //Methods
        public Process StartIdling()
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

        public void StopIdling()
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

            if (IsFastIdling)
            {
                fastIdleTries--;

                if (fastIdleTries == 2 && RemainingCards == originalRemainingCards)
                {
                    SetToNormalIdling();
                }
            }
        }

        private void SetToNormalIdling()
        {
            IsNormalIdling = true;
            IsFastIdling = false;
            fastIdleTries = 0;
        }

        private void SetToFastIdling()
        {
            IsNormalIdling = false;
            IsFastIdling = true;
            fastIdleTries = 2;
        }
    }
}