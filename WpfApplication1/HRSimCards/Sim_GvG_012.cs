using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_012 : SimTemplate //Light of the Naaru
    {

        //    Restore #3 Health. If the target is still damaged, summon a Lightwarden.

        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_001);

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int heal = p.getSpellHeal(3, ownplay);
            p.minionGetDamageOrHeal(target, -heal);
            if (target.Hp < target.maxHp)
            {
                int posi = (ownplay) ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;
                p.callKid(kid, posi, ownplay);
            }
        }


    }

}