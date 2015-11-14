using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_015
        : SimTemplate //Darkbomb
    {

        //   Deal $3 damage.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(3, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
        }


    }

}