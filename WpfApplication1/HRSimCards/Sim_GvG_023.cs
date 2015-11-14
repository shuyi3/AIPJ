using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_023 : SimTemplate //Goblin Auto-Barber
    {

        //    Battlecry: Give your weapon +1 Attack.

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            if (own.own)
            {
                if (p.playerFirst.ownWeaponDurability >= 1)
                {
                    p.playerFirst.ownWeaponAttack += 1;
                    p.minionGetBuffed(p.playerFirst.ownHero, 1, 0);
                }
                
            }
            else
            {
                if (p.playerSecond.ownWeaponDurability >= 1)
                {
                    p.playerSecond.ownWeaponAttack += 1;
                    p.minionGetBuffed(p.playerSecond.ownHero, 1, 0);
                }
                
            }
        }


    }

}