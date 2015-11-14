using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_126 : SimTemplate //betrayal
	{

//    zwingt einen feindlichen diener, seinen schaden benachbarten dienern zuzufügen.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            //attack right neightbor
            if (target.Angr>0)
            {
                int dmg = target.Angr;
                List<Minion> temp = (ownplay) ? p.playerSecond.ownMinions : p.playerFirst.ownMinions;
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if (m.zonepos + 1 == target.zonepos || m.zonepos-1 == target.zonepos)
                    {
                        /*int oldhp = m.Hp;
                        p.minionGetDamageOrHeal(m, dmg);
                        if (!target.silenced && (target.handcard.card.name == CardDB.cardName.waterelemental ||target.handcard.card.name == CardDB.cardName.snowchugger) && m.Hp < oldhp) m.frozen=true;
                        if (!target.silenced && m.Hp < oldhp && target.poisonous) p.minionGetDestroyed(m);*/
                        p.minionAttacksMinion(target, m, true);
                    }
                }

            }

		}

	}
}