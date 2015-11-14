using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_045 : SimTemplate //Imp-losion
    {

        //   Deal $2-$4 damage to a minion. Summon a 1/1 Imp for each damage dealt.

        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.GVG_045t);

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(2, ownplay);
            p.minionGetDamageOrHeal(target, dmg);

            int posi = (ownplay) ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;
            p.callKid(kid, posi, ownplay);
            posi++;
            p.callKid(kid, posi, ownplay);
        }


    }

}