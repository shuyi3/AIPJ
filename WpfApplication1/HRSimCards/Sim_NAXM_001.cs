using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NAXM_001 : SimTemplate //* Necroknight
	{
	//    Deathrattle: Destroy the minions next to this one as well.

		public override void onDeathrattle(Playfield p, Minion m)
		{
            List<Minion> temp = (m.own) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
            foreach (Minion mnn in temp)
            {
                if (mnn.zonepos == m.zonepos + 1 || mnn.zonepos + 1 == m.zonepos)
                {
					p.minionGetDestroyed(mnn);
                }

            }
		}
	}
}