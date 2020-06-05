using IdleMaster.Enums;
using System.Diagnostics;

namespace IdleMaster.Entities
{
    public class Game
    {
        //Fields
        private Process idleProcess;

        //Properties
        public string AppId { get; }

        public string Name { get; }

        public int OriginalRemainingCards { get; private set; }

        public int RemainingCards { get; private set; }

        public double HoursPlayed { get; private set; }

        public int FastIdleTries { get; set; }

        public bool HasDrops
        {
            get
            {
                return RemainingCards > 0;
            }
        }

        public GameStatus Status { get; set; }

        //Constructors
        public Game(string appId, string name, int remaining, double hours)
        {
            AppId = appId;
            Name = name;
            Status = GameStatus.Stopped;

            FastIdleTries = 10;

            OriginalRemainingCards = remaining;
            UpdateStats(remaining, hours);
        }

        //Methods
        public void StartIdling()
        {
            idleProcess = Process.Start(new ProcessStartInfo("steam-idle.exe", AppId)
            {
                WindowStyle = ProcessWindowStyle.Hidden
            });
        }

        public void StopIdling()
        {
            idleProcess.Kill();
            idleProcess.Dispose();
            idleProcess = null;
        }

        public void UpdateStats(int remainingCards, double hoursPlayed)
        {
            RemainingCards = remainingCards;
            HoursPlayed = hoursPlayed;
        }
    }
}