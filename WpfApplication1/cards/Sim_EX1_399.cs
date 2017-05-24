using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Sim_EX1_399 : SimTemplate //* gurubashiberserker
	{
//    Whenever this minion takes damage, gain +3 Attack.
        public override void onMinionGotDmgTrigger(Playfield p, Minion m, bool ownDmgdmin)
        {
            if (m.anzGotDmg>=1)
            {
                for (int i = 0; i < m.anzGotDmg; i++)
                {
					p.minionGetBuffed(m, 3, 0);
                }
                m.anzGotDmg = 0;
            }
        }

	}
}