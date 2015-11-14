using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_221 : SimTemplate //spitefulsmith
	{

//    wutanfall:/ eure waffe hat +2 angriff.
        public override void onEnrageStart(Playfield p, Minion m)
        {
            if (m.own)
            {
                if (p.playerFirst.ownWeaponDurability >= 1)
                {
                    p.minionGetBuffed(p.playerFirst.ownHero, 2, 0);
                    p.playerFirst.ownWeaponAttack += 2;
                }
            }
            else 
            {
                if (p.playerSecond.ownWeaponDurability >= 1)
                {
                    p.playerSecond.ownWeaponAttack += 2;
                    p.minionGetBuffed(p.playerSecond.ownHero, 2, 0);
                }
            }
        }

        public override void onEnrageStop(Playfield p, Minion m)
        {
            if (m.own)
            {
                if (p.playerFirst.ownWeaponDurability >= 1)
                {
                    p.minionGetBuffed(p.playerFirst.ownHero, -2, 0);
                    p.playerFirst.ownWeaponAttack -= 2;
                }
            }
            else
            {
                if (p.playerSecond.ownWeaponDurability >= 1)
                {
                    p.playerSecond.ownWeaponAttack -= 2;
                    p.minionGetBuffed(p.playerSecond.ownHero, -2, 0);
                }
            }
        }

	}

}