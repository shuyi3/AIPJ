using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NAX8_02H : SimTemplate //* Harvest heroic
	{

//    Hero PowerDraw a card. Gain a Mana Crystal.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
			
			p.playerFirst.mana = Math.Min(10, p.playerFirst.mana++);
			if (ownplay)
			{
				p.playerFirst.ownMaxMana = Math.Min(10, p.playerFirst.ownMaxMana++);
			}
			else
			{
				p.playerSecond.ownMaxMana = Math.Min(10, p.playerSecond.ownMaxMana++);
			}
        }
	}
}