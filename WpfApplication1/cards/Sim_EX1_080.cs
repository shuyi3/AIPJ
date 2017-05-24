using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Sim_EX1_080 : SimTemplate //* Secretkeeper
	{
		// Whenever a Secret: is played, gain +1/+1.
		
		public override void onCardIsGoingToBePlayed(Playfield p, CardDB.Card c, bool ownplay, Minion m)
        {
            if (c.Secret) p.minionGetBuffed(m, 1, 1);
        }

	}
}