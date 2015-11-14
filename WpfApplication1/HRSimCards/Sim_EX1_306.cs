using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_306 : SimTemplate //succubus //TODO: rework
	{

//    kampfschrei:/ werft eine zufällige karte ab.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            if (own.own)
            {
                p.playerFirst.owncarddraw -= Math.Min(1, p.playerFirst.owncards.Count);
                p.playerFirst.owncards.RemoveRange(0, Math.Min(1, p.playerFirst.owncards.Count));
                p.triggerCardsChanged(true);
            }
            else
            {
                p.playerSecond.owncarddraw -= Math.Min(1, p.playerSecond.owncards.Count);
                p.playerSecond.owncards.RemoveRange(0, Math.Min(1, p.playerSecond.owncards.Count));
                p.triggerCardsChanged(false);
            }
		}

	}
}