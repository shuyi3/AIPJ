using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NEW1_024 : SimTemplate //captaingreenskin
	{
//    kampfschrei:/ verleiht eurer waffe +1/+1.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            if (own.own)
            {
                if (p.playerFirst.ownWeaponDurability >= 1)
                {
                    p.playerFirst.ownWeaponDurability++;
                    p.playerFirst.ownWeaponAttack++;
                    p.minionGetBuffed(p.playerFirst.ownHero, 1, 0);
                }
            }
            else
            {
                if (p.playerSecond.ownWeaponDurability >= 1)
                {
                    p.playerSecond.ownWeaponDurability++;
                    p.playerSecond.ownWeaponAttack++;
                    p.minionGetBuffed(p.playerSecond.ownHero, 1, 0);
                }
            }
		}

	}
}