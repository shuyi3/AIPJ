using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_NEW1_007b : SimTemplate //starfall choice left
    {

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(5, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
        }

    }
}