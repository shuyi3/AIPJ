using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_074 : SimTemplate //deadlypoison
	{

//    eure waffe erhält +2 angriff.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            if (ownplay)
            {
                if (p.playerFirst.ownWeaponDurability >= 1)
                {
                    p.playerFirst.ownWeaponAttack += 2;
                    p.playerFirst.ownHero.Angr += 2;
                }
            }
            else
            {
                if (p.playerSecond.ownWeaponDurability >= 1)
                {
                    p.playerSecond.ownWeaponAttack += 2;
                    p.playerSecond.ownHero.Angr += 2;
                }
            }
		}

	}
}