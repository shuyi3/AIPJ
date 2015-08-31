using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_531 : SimTemplate //* scavenginghyena
	{
//    Whenever a friendly Beast dies, gain +2/+1.

        public override void onMinionDiedTrigger(Playfield p, Minion m, Minion diedMinion)
        {
            int diedMinions = (m.own) ? p.tempTrigger.ownBeastDied : p.tempTrigger.enemyBeastDied;
			if(p.tempTrigger.ownBeastDied > 0 && m.own)
			{
				p.minionGetBuffed(m, 2 * diedMinions, diedMinions); //p.minionGetBuffed(m, 2, 1);
			}
			else if(p.tempTrigger.enemyBeastDied > 0 && !m.own)
			{
				p.minionGetBuffed(m, 2 * diedMinions, diedMinions); 
			}		
        }

	}
}