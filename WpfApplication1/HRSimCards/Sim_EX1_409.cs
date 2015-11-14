using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_409 : SimTemplate //upgrade
	{
        CardDB.Card wcard = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_409t);//heavyaxe
//    wenn ihr eine waffe habt, erhält sie +1/+1. legt anderenfalls eine waffe (1/3) an.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            if (ownplay)
            {
                if (p.playerFirst.ownWeaponName != CardDB.cardName.unknown)
                {
                    p.playerFirst.ownWeaponAttack++;
                    p.playerFirst.ownWeaponDurability++;
                    p.minionGetBuffed(p.playerFirst.ownHero, 1, 0);
                }
                else
                {

                    p.equipWeapon(wcard, true);
                }
            }
            else
            {
                if (p.playerSecond.ownWeaponName != CardDB.cardName.unknown)
                {
                    p.playerSecond.ownWeaponAttack++;
                    p.playerSecond.ownWeaponDurability++;
                    p.minionGetBuffed(p.playerSecond.ownHero, 1, 0);
                }
                else
                {

                    p.equipWeapon(wcard, false);
                }
            }
		}

	}

}