using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_043 : SimTemplate //Glaivezooka
    {

        //   Battlecry: Give a random friendly minion +1 Attack.

        CardDB.Card w = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.GVG_043);
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.equipWeapon(w, ownplay);
            List<Minion> temp = (ownplay) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
            if (temp.Count <= 0) return;
            // Drew: Null check for searchRandomMinion.
            var found = p.searchRandomMinion(temp, Playfield.searchmode.searchLowestAttack);
            if (found != null)
            {
                p.minionGetBuffed(found, 1, 0);
            }

        }

    }

}