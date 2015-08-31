using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_106 : SimTemplate //* Junkbot
    {
//   Whenever a friendly Mech dies, gain +2/+2.

        public override void onMinionDiedTrigger(Playfield p, Minion m, Minion diedMinion)
        {
			int diedMinions = (m.own)? p.tempTrigger.ownMechanicDied : p.tempTrigger.enemyMechanicDied;
			if(p.tempTrigger.ownMechanicDied > 0 && m.own)
			{
				for (int i = 0; i < diedMinions; i++)
				{
					p.minionGetBuffed(m, 2 * diedMinions, 2 * diedMinions);
				}
			}
			else if(p.tempTrigger.enemyMechanicDied > 0 && !m.own)
			{
				for (int i = 0; i < diedMinions; i++)
				{
					p.minionGetBuffed(m, 2 * diedMinions, 2 * diedMinions);
				}
			}
        }
    }
}