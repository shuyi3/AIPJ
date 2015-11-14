using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_537 : SimTemplate //explosiveshot
	{

//    fügt einem diener $5 schaden und benachbarten dienern $2 schaden zu.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg1 = p.getSpellDamageDamage(5, ownplay);
            int dmg2 = p.getSpellDamageDamage(2, ownplay);
            List<Minion> temp = (target.own) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
            p.minionGetDamageOrHeal(target, dmg1);
            foreach (Minion m in temp)
            {
                if (m.zonepos + 1 == target.zonepos || m.zonepos - 1 == target.zonepos) p.minionGetDamageOrHeal(m, dmg2);
            }
		}

	}
}