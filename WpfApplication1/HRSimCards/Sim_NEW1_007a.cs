using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_NEW1_007a : SimTemplate //starfall choice left
    {

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(2, ownplay);
            p.allMinionOfASideGetDamage(!ownplay, dmg);
        }

    }
}