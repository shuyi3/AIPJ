using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_310 : SimTemplate //doomguard
	{

//    ansturm/. kampfschrei:/ werft zwei zufällige karten ab.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            if (own.own)
            {
                int anz = Math.Min(2, p.playerFirst.owncards.Count);
                p.playerFirst.owncarddraw -= anz;
                p.playerFirst.owncards.RemoveRange(0, anz);
                if (anz >= 1)
                {
                    p.triggerCardsChanged(true);
                }

            }
            else
            {
                int anz = Math.Min(2, p.playerSecond.owncards.Count);
                p.playerSecond.owncarddraw -= anz;
                p.playerSecond.owncards.RemoveRange(0, anz);
                if (anz >= 1)
                {
                    p.triggerCardsChanged(false);
                }
            }


		}

	}
}