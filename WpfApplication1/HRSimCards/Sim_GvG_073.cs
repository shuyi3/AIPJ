using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_073 : SimTemplate //Cobra Shot
    {

        //   Deal $3 damage to a minion and the enemy hero.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(3, ownplay);

            p.minionGetDamageOrHeal(target, dmg);

            if (ownplay) p.minionGetDamageOrHeal(p.playerSecond.ownHero, dmg);
            else p.minionGetDamageOrHeal(p.playerFirst.ownHero, dmg);
        }


    }

}