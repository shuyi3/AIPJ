using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_022 : SimTemplate //Tinker's Sharpsword Oil
    {

        //    Give your weapon +3 Attack. Combo: Give a random friendly minion +3 Attack.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            if (ownplay)
            {
                if (p.playerFirst.ownWeaponDurability >= 1)
                {
                    p.playerFirst.ownWeaponAttack += 3;
                    p.minionGetBuffed(p.playerFirst.ownHero, 3, 0);
                }
                if (p.playerFirst.cardsPlayedThisTurn >= 1 && p.playerFirst.ownMinions.Count >= 1)
                {
                    // Drew: Null check for searchRandomMinion.
                    var found = p.searchRandomMinion(p.playerFirst.ownMinions, Playfield.searchmode.searchLowestAttack);
                    if (found != null)
                    {
                        p.minionGetBuffed(found, 3, 0);
                    }
                }
            }
            else
            {
                if (p.playerSecond.ownWeaponDurability >= 1)
                {
                    p.playerSecond.ownWeaponAttack += 3;
                    p.minionGetBuffed(p.playerSecond.ownHero, 3, 0);
                }
                if (p.playerFirst.cardsPlayedThisTurn >= 1 && p.playerSecond.ownMinions.Count >= 1)
                {
                    // Drew: Null check for searchRandomMinion.
                    var found = p.searchRandomMinion(p.playerSecond.ownMinions, Playfield.searchmode.searchLowestAttack);
                    if (found != null)
                    {
                        p.minionGetBuffed(found, 3, 0);
                    }
                }
            }
        }


    }

}