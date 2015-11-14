using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_062 : SimTemplate //* oldmurkeye
	{
//    Charge. Has +1 Attack for each other Murloc on the battlefield.

		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            foreach (Minion m in p.playerFirst.ownMinions)
            {
                if (m.handcard.card.race == 14)
                {
                    p.minionGetBuffed(own, 1, 0);
                }
            }

            foreach (Minion m in p.playerSecond.ownMinions)
            {
                if (m.handcard.card.race == 14)
                {
                    p.minionGetBuffed(own, 1, 0);
                }
            }
		}

        public override void onMinionIsSummoned(Playfield p, Minion triggerEffectMinion, Minion summonedMinion)
        {
            if (summonedMinion.handcard.card.race == 14)
            {
                p.minionGetBuffed(triggerEffectMinion, 1, 0);
            }
        }

        public override void onMinionDiedTrigger(Playfield p, Minion m, Minion diedMinion)
        {
			int diedMinions = p.tempTrigger.ownMurlocDied + p.tempTrigger.enemyMurlocDied;
            if (diedMinions >= 1)
			{
                p.minionGetBuffed(m, -1 * (p.tempTrigger.ownMurlocDied + p.tempTrigger.enemyMurlocDied), 0);
			}
        }

	}
}