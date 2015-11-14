using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_033 : SimTemplate //* Tree of Life
    {

        //    Restore all characters to full Health.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
			int heal = 1000;
            foreach (Minion m in p.playerFirst.ownMinions)
            {
                p.minionGetDamageOrHeal(m, -heal);
            }
            foreach (Minion m in p.playerSecond.ownMinions)
            {
                p.minionGetDamageOrHeal(m, -heal);
            }

            p.minionGetDamageOrHeal(p.playerSecond.ownHero, -heal);
            p.minionGetDamageOrHeal(p.playerFirst.ownHero, -heal);
        }


    }

}