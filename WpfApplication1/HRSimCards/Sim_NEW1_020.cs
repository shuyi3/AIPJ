using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NEW1_020 : SimTemplate //* Wild Pyromancer
	{
		// After you cast a spell, deal 1 damage to ALL minions.

		public override void onCardIsGoingToBePlayed(Playfield p, CardDB.Card c, bool ownplay, Minion m)
        {
            if (m.own == ownplay && c.type == CardDB.cardtype.SPELL) p.allMinionsGetDamage(1);
        }
	}
}