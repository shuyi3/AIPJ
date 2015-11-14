using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_012 : SimTemplate //swipe
	{

//    fügt einem feind $4 schaden und allen anderen feinden $1 schaden zu.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(1, ownplay);
            int dmg1 = p.getSpellDamageDamage(4, ownplay);

            List<Minion> temp = (ownplay) ? p.playerSecond.ownMinions : p.playerFirst.ownMinions;
            p.minionGetDamageOrHeal(target, dmg1);
            foreach (Minion m in temp)
            {
                if (m.entitiyID != target.entitiyID)
                {
                    p.minionGetDamageOrHeal(m, dmg);
                }
            }
            if (ownplay)
            {
                if (p.playerSecond.ownHero.entitiyID != target.entitiyID)
                {
                    p.minionGetDamageOrHeal(p.playerSecond.ownHero, dmg);
                }
            }
            else
            {
                if (p.playerFirst.ownHero.entitiyID != target.entitiyID)
                {
                    p.minionGetDamageOrHeal(p.playerFirst.ownHero, dmg);
                }
                
            }
		}

	}
}