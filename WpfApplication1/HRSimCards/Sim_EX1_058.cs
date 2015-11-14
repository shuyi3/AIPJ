using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_058 : SimTemplate //sunfuryprotector
	{

//    kampfschrei:/ verleiht benachbarten dienern spott/.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            List<Minion> temp = (own.own) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
            foreach (Minion m in temp)
            {
                if (m.zonepos == own.zonepos - 1 || m.zonepos == own.zonepos)
                {
                    m.taunt = true;
                }
            }
		}

	}
}