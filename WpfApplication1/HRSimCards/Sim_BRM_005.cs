using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_BRM_005 : SimTemplate //* Demonwrath
	{
		// Deal 2 damage to all non-Demon minions.
		
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(2, ownplay);
			
			foreach (Minion m in p.playerFirst.ownMinions)
            {
                if ((TAG_RACE)m.handcard.card.race != TAG_RACE.DEMON) p.minionGetDamageOrHeal(m, dmg);
            }
            foreach (Minion m in p.playerSecond.ownMinions)
            {
                if ((TAG_RACE)m.handcard.card.race != TAG_RACE.DEMON) p.minionGetDamageOrHeal(m, dmg);
            }
		}
	}
}