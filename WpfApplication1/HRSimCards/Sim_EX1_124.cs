using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_124 : SimTemplate //eviscerate
	{

//    verursacht $2 schaden. combo:/ verursacht stattdessen $4 schaden.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(4, ownplay);
            if (p.playerFirst.cardsPlayedThisTurn == 0) dmg = p.getSpellDamageDamage(2, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
		}

	}
}