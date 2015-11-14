using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NAX6_03t : SimTemplate //* Spore
	{
//    Deathrattle: Give all enemy minions +8 Attack.

        public override void onDeathrattle(Playfield p, Minion m)
		{
			List<Minion> temp = (m.own) ? p.playerSecond.ownMinions : p.playerFirst.ownMinions;
            foreach (Minion mm in temp)
            {
                p.minionGetBuffed(mm, 8, 0);
            }
		}
	}
}