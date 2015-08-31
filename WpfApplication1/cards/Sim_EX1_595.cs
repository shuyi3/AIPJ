using System;
using System.Collections.Generic;
using System.Text;

namespace HREngine.Bots
{
	class Sim_EX1_595 : SimTemplate //* cultmaster
	{
//    Whenever one of your other minions dies, draw a card.

        public override void onMinionDiedTrigger(Playfield p, Minion m, Minion diedMinion)
        {
			int diedMinions = (m.own)? p.tempTrigger.ownMinionsDied : p.tempTrigger.enemyMinionsDied;
            if(p.tempTrigger.ownMinionsDied > 0 && m.own)
			{
				for (int i = 0; i < diedMinions; i++)
				{
					p.drawACard(CardDB.cardName.unknown, m.own);
				}
			}
			else if(p.tempTrigger.enemyMinionsDied > 0 && !m.own)
			{
				for (int i = 0; i < diedMinions; i++)
				{
					p.drawACard(CardDB.cardName.unknown, m.own);
				}
			}
			
        }
	}
}