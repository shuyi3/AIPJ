using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_408 : SimTemplate //mortalstrike
	{

//    verursacht $4 schaden. verursacht stattdessen $6 schaden, wenn euer held max. 12 leben hat.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = 0;

            if (ownplay)
            {
                dmg = (p.playerFirst.ownHero.Hp <= 12) ? p.getSpellDamageDamage(6, ownplay) : p.getSpellDamageDamage(4, ownplay);
            }
            else
            {
                dmg = (p.playerSecond.ownHero.Hp <= 12) ? p.getSpellDamageDamage(6, ownplay) : p.getSpellDamageDamage(4, ownplay);
            }
            p.minionGetDamageOrHeal(target, dmg);
		}

	}
}