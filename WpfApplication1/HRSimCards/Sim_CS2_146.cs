using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_146 : SimTemplate //southseadeckhand
	{

//    hat ansturm/, während ihr eine waffe angelegt habt.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            if (own.own)
            {
                if (p.playerFirst.ownWeaponDurability >= 1)
                {
                    p.minionGetCharge(own);
                }
            }
            else
            {
                if (p.playerSecond.ownWeaponDurability >= 1)
                {
                    p.minionGetCharge(own);
                }
            }
		}

	}
}