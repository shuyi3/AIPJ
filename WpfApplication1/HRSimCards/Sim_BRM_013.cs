using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_BRM_013 : SimTemplate //* Quick Shot
	{
		// Deal 3 damage. If your hand is empty, draw a card.
		
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(3, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
			
			int cardsCount = (ownplay) ? p.playerFirst.owncards.Count : p.playerSecond.owncards.Count;
            if (cardsCount <= 0) p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
		}
	}
}