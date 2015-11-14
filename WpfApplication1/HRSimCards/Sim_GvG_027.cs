using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_027 : SimTemplate //Iron Sensei
    {

        //   At the end of your turn, give another friendly Mech +2/+2.

        public override void onTurnEndsTrigger(Playfield p, Minion triggerEffectMinion, bool turnEndOfOwner)
        {
            if (turnEndOfOwner == triggerEffectMinion.own)
            {
                List<Minion> temp = (turnEndOfOwner) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
                if (temp.Count >= 1)
                {
                    // Drew: Null check for searchRandomMinion.
                    var found = p.searchRandomMinion(temp, Playfield.searchmode.searchHighestAttack);
                    if (found != null)
                    {
                        p.minionGetBuffed(found, 2, 2);
                    }
                }
            }
        }


    }

}