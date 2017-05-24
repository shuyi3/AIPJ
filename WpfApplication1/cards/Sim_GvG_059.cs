using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
    internal class Sim_GVG_059 : SimTemplate //Coghammer
    {
        //   Battlecry: Give a random friendly minion Divine Shield and Taunt;.
        private CardDB.Card w = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.GVG_059);

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.equipWeapon(w, ownplay);
            List<Minion> temp = (ownplay) ? p.ownMinions : p.enemyMinions;
            if (temp.Count <= 0) return;
            Minion m = p.searchRandomMinion(temp, Playfield.searchmode.searchLowestHP);
            if (m != null)
            {
                m.divineshild = true;
                m.taunt = true;
            }
        }
    }
}