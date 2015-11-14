using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_259 : SimTemplate//Lightning Storm
    {
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(3, ownplay);
            p.allMinionOfASideGetDamage(!ownplay, dmg);
            if (ownplay) p.ueberladung += 2;
        }

    }
}
