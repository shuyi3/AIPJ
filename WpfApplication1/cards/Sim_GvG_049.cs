using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
    class Sim_GVG_049 : SimTemplate //* Gahz'rilla
    {
        //   Whenever this minion takes damage, double its Attack.

        public override void onMinionGotDmgTrigger(Playfield p, Minion m, bool ownDmgdmin)
        {
            if (m.anzGotDmg>=1)
            {
				p.minionGetBuffed(m, m.Angr * (2^m.anzGotDmg)-m.Angr, 0);
                m.anzGotDmg = 0;
            }
        }
    }

}