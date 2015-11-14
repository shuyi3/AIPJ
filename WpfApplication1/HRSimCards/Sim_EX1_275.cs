using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_275 : SimTemplate //coneofcold
	{

//    friert/ einen diener sowie seine benachbarten diener ein und fügt ihnen $1 schaden zu.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(1, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
            target.frozen = true;
            List<Minion> temp = (target.own) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
            foreach (Minion m in temp)
            {
                if (target.zonepos == m.zonepos + 1 || target.zonepos + 1 == m.zonepos)
                {
                    p.minionGetDamageOrHeal(m, dmg);
                    m.frozen = true;
                }

            }
		}


	}
}