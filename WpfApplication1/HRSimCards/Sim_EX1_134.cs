using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_134 : SimTemplate //* si7agent
	{
        // Combo:: Deal 2 damage.

		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            if (p.playerFirst.cardsPlayedThisTurn >= 1 && target != null)
            {
                p.minionGetDamageOrHeal(target, 2);
            }
		}
	}
}