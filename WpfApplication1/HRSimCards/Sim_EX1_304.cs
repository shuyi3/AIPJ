using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_304 : SimTemplate //voidterror
	{

//    kampfschrei:/ vernichtet die benachbarten diener und verleiht ihm deren angriff und leben.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            List<Minion> temp = (own.own) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;

            int angr = 0;
            int hp = 0;
            foreach (Minion m in temp)
            {
                if (m.zonepos == own.zonepos || m.zonepos == own.zonepos - 1)
                {
                    angr += m.Angr;
                    hp += m.Hp;
                    p.minionGetDestroyed(m);
                }
            }
            p.minionGetBuffed(own, angr, hp);
		}


	}
}