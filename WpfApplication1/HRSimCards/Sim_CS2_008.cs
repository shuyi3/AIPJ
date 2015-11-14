using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_CS2_008 : SimTemplate//moonfire
    {

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(1, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
            
        }

    }
}
