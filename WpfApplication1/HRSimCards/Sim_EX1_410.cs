using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_410 : SimTemplate //shieldslam
	{

//    fügt einem diener für jeden eurer rüstungspunkte 1 schaden zu.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{


            int dmg = (ownplay) ? p.getSpellDamageDamage(p.playerFirst.ownHero.armor, ownplay) : p.getSpellDamageDamage(p.playerSecond.ownHero.armor, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
		}

	}
}