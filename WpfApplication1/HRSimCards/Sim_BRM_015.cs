using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_BRM_015 : SimTemplate //* Revenge
	{
		// Deal 1 damage to all minions. If you have 12 or less Health, deal 3 damage instead.
		
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
			int dmg = 1;
			int heroHealth = (ownplay) ? p.playerFirst.ownHero.Hp : p.playerSecond.ownHero.Hp;
			if(heroHealth <= 12) dmg = 3;

            dmg = p.getSpellDamageDamage(dmg, ownplay);
            p.allMinionsGetDamage(dmg);
		}
	}
}