using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_tt_004 : SimTemplate //* flesheatingghoul
	{
//    Whenever a minion dies, gain +1 Attack.

        public override void onMinionDiedTrigger(Playfield p, Minion m, Minion diedMinion)
        {
			int diedMinions = p.tempTrigger.ownMinionsDied + p.tempTrigger.enemyMinionsDied;
            for (int i = 0; i < diedMinions; i++)
            {
				p.minionGetBuffed(m, 1, 0);
            }
        }
	}
}