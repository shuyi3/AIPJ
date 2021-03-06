using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_047 : SimTemplate //Sabotage
    {

        //   Destroy a random enemy minion. Combo: And your opponent's weapon.


        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            List<Minion> temp = (ownplay)? p.playerSecond.ownMinions : p.playerFirst.ownMinions;
            if (temp.Count >= 1)
            {
                // Drew: Null check for searchRandomMinion.
                var found = p.searchRandomMinion(temp, Playfield.searchmode.searchLowestHP);
                if (found != null)
                {
                    p.minionGetDestroyed(found);
                }
            }
            if (p.playerFirst.cardsPlayedThisTurn >= 1) p.lowerWeaponDurability(1000, !ownplay);
        }


    }

}