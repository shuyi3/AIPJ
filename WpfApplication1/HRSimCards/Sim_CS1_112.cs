using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_CS1_112 : SimTemplate//holy nova
    {
        //todo make it better :D
        //FÃ¼gt allen Feinden $2 Schaden zu. Stellt bei allen befreundeten Charakteren #2 Leben wieder her.
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(2, ownplay);
            int heal = p.getSpellHeal(2, ownplay);
            if (ownplay)
            {
                p.minionGetDamageOrHeal(p.playerFirst.ownHero, -heal);
                p.minionGetDamageOrHeal(p.playerSecond.ownHero, dmg);
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    p.minionGetDamageOrHeal(m, -heal);
                }

                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    p.minionGetDamageOrHeal(m, dmg);
                }
            }
            else 
            {
                p.minionGetDamageOrHeal(p.playerSecond.ownHero, -heal);
                p.minionGetDamageOrHeal(p.playerFirst.ownHero, dmg);
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    p.minionGetDamageOrHeal(m, -heal);
                }

                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    p.minionGetDamageOrHeal(m, dmg);
                }
            }
        }

    }
}
