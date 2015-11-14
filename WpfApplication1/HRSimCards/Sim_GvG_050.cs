using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_050 : SimTemplate //Bouncing Blade
    {

        //   Deal $1 damage to a random minion. Repeat until a minion dies.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(1, ownplay);

            int minHp = 100000;
            foreach (Minion m in p.playerFirst.ownMinions)
            {
                if (m.Hp < minHp) minHp = m.Hp;
            }
            foreach (Minion m in p.playerSecond.ownMinions)
            {
                if (m.Hp < minHp) minHp = m.Hp;
            }

            int dmgdone = (int)Math.Ceiling((double)minHp / (double)dmg) * dmg;

            p.allMinionsGetDamage(dmgdone);
        }


    }

}