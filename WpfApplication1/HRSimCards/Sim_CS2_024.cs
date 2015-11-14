using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_CS2_024 : SimTemplate//Frostbolt
    {

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(3, ownplay);
            target.frozen = true;
            p.minionGetDamageOrHeal(target,dmg);

        }

    }
}
