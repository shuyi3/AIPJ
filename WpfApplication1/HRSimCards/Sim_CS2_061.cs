using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_CS2_061 : SimTemplate //drainlife
    {

        //    verursacht $2 schaden. stellt bei eurem helden #2 leben wieder her.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(2, ownplay);
            int heal = p.getSpellHeal(2, ownplay);
            p.minionGetDamageOrHeal(target, dmg);

            p.minionGetDamageOrHeal(ownplay ? p.playerFirst.ownHero : p.playerSecond.ownHero, -heal);
        }

    }
}