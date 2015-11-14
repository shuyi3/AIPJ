using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_tt_010 : SimTemplate //spellbender
    {
        //todo secret
        //    geheimnis:/ wenn ein feind einen zauber auf einen diener wirkt, ruft ihr einen diener (1/3) als neues ziel herbei.

        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.tt_010a);

        public override void onSecretPlay(Playfield p, bool ownplay, Minion attacker, Minion target, out int number)
        {
            number = 0;
            if (ownplay)
            {
                int posi = p.playerFirst.ownMinions.Count;
                p.callKid(kid, posi, true);
                if (p.playerFirst.ownMinions.Count >= 1)
                {
                    if (p.playerFirst.ownMinions[p.playerFirst.ownMinions.Count - 1].name == CardDB.cardName.spellbender)
                    {
                        number = p.playerFirst.ownMinions[p.playerFirst.ownMinions.Count - 1].entitiyID;
                    }
                }
            }
            else
            {
                int posi = p.playerSecond.ownMinions.Count;
                p.callKid(kid, posi, false);

                if (p.playerSecond.ownMinions.Count >= 1)
                {
                    if (p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].name == CardDB.cardName.spellbender)
                    {
                        number = p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].entitiyID;
                    }
                }
            }

        }

    }

}