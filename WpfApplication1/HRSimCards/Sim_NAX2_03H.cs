using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NAX2_03H : SimTemplate //rainoffire
	{

//  Hero Power (Heroic): Fire a missile for each card in your opponent's hand.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = 1;
			int cardCount = 0;
            if (ownplay)
            {
				cardCount = p.owncards.Count;
                if (p.doublepriest >= 1) dmg *= (2 * p.doublepriest);
            }
            else
            {
				cardCount = p.enemyAnzCards;
                if (p.enemydoublepriest >= 1) dmg *= (2 * p.enemydoublepriest);
            }
						
            for (int i = 0; i < cardCount; i++)
            {
				var found = (ownplay)? p.searchRandomMinion(p.ownMinions, Playfield.searchmode.searchHighAttackLowHP) : p.searchRandomMinion(p.enemyMinions, Playfield.searchmode.searchHighAttackLowHP);
				if (found != null)
				{
					p.minionGetDamageOrHeal(found, dmg);
				}
            }
        }
	}
}