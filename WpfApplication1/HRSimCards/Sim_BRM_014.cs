using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_BRM_014 : SimTemplate //* Core Rager
    {
        // Battlecry: If your hand is empty, gain +3/+3.

        public override void getBattlecryEffect(Playfield p, Minion m, Minion target, int choice)
        {
            int cardsCount = (m.own) ? p.playerFirst.owncards.Count : p.playerSecond.owncards.Count;
            if (cardsCount <= 0)
            {
                p.minionGetBuffed(m, 3, 3);
            }
        }
    }
}