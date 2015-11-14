using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_PRO_001b : SimTemplate//Rogues Do It...
    {


        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(4, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
        }

    }
}
