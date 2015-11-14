using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_584 : SimTemplate //ancientmage
	{
//    kampfschrei:/ verleiht benachbarten dienern zauberschaden +1/.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            List<Minion> temp = (own.own) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
            foreach (Minion m in temp)
            {
                if (m.zonepos == own.zonepos || m.zonepos + 1 == own.zonepos)
                {
                    m.spellpower++;
                    if (own.own)
                    {
                        p.playerFirst.spellpower++;
                    }
                    else
                    {
                        p.playerSecond.spellpower++;
                    }
                }
            }
		}


	}
}