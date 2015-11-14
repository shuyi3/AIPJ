using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_130 : SimTemplate //noblesacrifice
    {
        //todo secret
        //    geheimnis:/ wenn ein feind angreift, ruft ihr einen verteidiger (2/1) als neues ziel herbei.

        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_130a);

        public override void onSecretPlay(Playfield p, bool ownplay, Minion attacker, Minion target, out int number)
        {
            number = 0;
            if (ownplay)
            {
                int posi = p.playerFirst.ownMinions.Count;
                p.callKid(kid, posi, true);
                if (p.playerFirst.ownMinions.Count >= 1)
                {
                    if (p.playerFirst.ownMinions[p.playerFirst.ownMinions.Count - 1].name == CardDB.cardName.defender)
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
                    if (p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].name == CardDB.cardName.defender)
                    {
                        number = p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].entitiyID;
                    }
                }
            }

        }

    }

}