using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_578 : SimTemplate //savagery
	{

//    fügt einem diener schaden zu, der dem angriff eures helden entspricht.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(p.playerFirst.ownHero.Angr, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
		}

	}
}