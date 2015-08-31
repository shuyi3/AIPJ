using System;
using System.Collections.Generic;
using System.Text;

namespace HREngine.Bots
{
    class Sim_GVG_040 : SimTemplate //* Siltfin Spiritwalker
    {
//    Whenever another friendly Murloc dies, draw a card. Overload: (1)

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            if (own.own) p.ueberladung++;
        }
		
        public override void onMinionDiedTrigger(Playfield p, Minion m, Minion diedMinion)
        {
			int diedMinions = (m.own)? p.tempTrigger.ownMurlocDied : p.tempTrigger.enemyMurlocDied;
			if(p.tempTrigger.ownMurlocDied > 0 && m.own)
			{
				for (int i = 0; i < diedMinions; i++)
				{
					p.drawACard(CardDB.cardName.unknown, m.own);
				}
			}
			else if(p.tempTrigger.enemyMurlocDied > 0 && !m.own)
			{
				for (int i = 0; i < diedMinions; i++)
				{
					p.drawACard(CardDB.cardName.unknown, m.own);
				}
			}
        }
    }
}