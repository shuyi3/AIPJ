using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_BRM_004 : SimTemplate //* Twilight Whelp
    {
        // Battlecry: If you're holding a Dragon, gain +2 Health.

        public override void getBattlecryEffect(Playfield p, Minion m, Minion target, int choice)
        {
			if(m.own)
			{
				bool dragonInHand = false;
				foreach (Handmanager.Handcard hc in p.playerFirst.owncards)
				{
					if ((TAG_RACE)hc.card.race == TAG_RACE.DRAGON)
					{
						dragonInHand = true;
						break;
					}
				}
				if(dragonInHand) p.minionGetBuffed(m, 0, 2);
			}
			else
			{
				p.minionGetBuffed(m, 0, 2);
			}
        }
    }
}