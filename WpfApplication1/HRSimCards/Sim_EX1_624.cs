using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_624 : SimTemplate //holyfire
    {

        //    verursacht $5 schaden. stellt bei eurem helden #5 leben wieder her.
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(5, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
            int heal = p.getSpellHeal(5, ownplay);

            p.minionGetDamageOrHeal(ownplay ? p.playerFirst.ownHero : p.playerSecond.ownHero, -heal);
        }

    }
}