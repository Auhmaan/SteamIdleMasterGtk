namespace IdleMaster
{
    public class Statistics
    {
        private int SessionMinutesIdled;
        private int SessionCardIdled;
        private int RemainingCards;

        public int GetSessionMinutesIdled()
        {
            return SessionMinutesIdled;
        }

        public int GetSessionCardIdled()
        {
            return SessionCardIdled;
        }

        public int GetRemainingCards()
        {
            return RemainingCards;
        }

        public void SetRemainingCards(int remainingCards)
        {
            RemainingCards = remainingCards;
        }

        public void CheckCardRemaining(int actualCardRemaining)
        {
            if (actualCardRemaining < RemainingCards)
            {
                IncreaseCardIdled(RemainingCards - actualCardRemaining);
                RemainingCards = actualCardRemaining;
            }
            else if (actualCardRemaining > RemainingCards)
            {
                RemainingCards = actualCardRemaining;
            }
        }

        public void IncreaseCardIdled(int number)
        {
            Properties.Settings.Default.totalCardIdled += number;
            Properties.Settings.Default.Save();
            SessionCardIdled += number;
        }

        public void IncreaseMinutesIdled()
        {
            Properties.Settings.Default.totalMinutesIdled++;
            Properties.Settings.Default.Save();
            SessionMinutesIdled++;
        }
    }
}