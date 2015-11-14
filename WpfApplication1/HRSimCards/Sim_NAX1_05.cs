using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_NAX1_05 : SimTemplate//* Locust Swarm
    {
        //Deal $3 damage to all enemy minions. Restore #3 Health to your hero.
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(3, ownplay);
            int heal = p.getSpellHeal(3, ownplay);
            if (ownplay)
            {
                p.minionGetDamageOrHeal(p.playerFirst.ownHero, -heal);
                p.minionGetDamageOrHeal(p.playerSecond.ownHero, dmg);
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    p.minionGetDamageOrHeal(m, dmg);
                }
            }
            else 
            {
                p.minionGetDamageOrHeal(p.playerSecond.ownHero, -heal);
                p.minionGetDamageOrHeal(p.playerFirst.ownHero, dmg);
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    p.minionGetDamageOrHeal(m, dmg);
                }
            }
        }

    }
}
