using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NEW1_030 : SimTemplate //deathwing
	{

//    kampfschrei:/ vernichtet alle anderen diener und werft eure hand ab.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            p.allMinionsGetDestroyed();
            if (own.own)
            {
                p.playerFirst.owncards.Clear();
                p.triggerCardsChanged(true);
            }
            else
            {
                p.playerSecond.owncarddraw = 0;
                p.playerSecond.owncards.Clear();
                p.triggerCardsChanged(false);
            }
		}

	

	}
}