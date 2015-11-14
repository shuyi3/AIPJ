using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_136 : SimTemplate //redemption
    {
        //todo secret
        //    geheimnis:/ wenn einer eurer diener stirbt, wird er mit 1 leben wiederbelebt.

        public override void onSecretPlay(Playfield p, bool ownplay, int number)
        {
            int posi = ownplay ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;

            CardDB.Card kid = CardDB.Instance.getCardDataFromID(ownplay ? p.playerFirst.revivingOwnMinion : p.playerSecond.revivingOwnMinion);

            p.callKid(kid, posi, ownplay);
            if (ownplay)
            {
                if (p.playerFirst.ownMinions.Count >= 1)
                {
                    if (p.playerFirst.ownMinions[p.playerFirst.ownMinions.Count - 1].handcard.card.cardIDenum == kid.cardIDenum)
                    {
                        p.playerFirst.ownMinions[p.playerFirst.ownMinions.Count - 1].Hp = 1;
                        p.playerFirst.ownMinions[p.playerFirst.ownMinions.Count - 1].wounded = false;
                        if (p.playerFirst.ownMinions[p.playerFirst.ownMinions.Count - 1].Hp < p.playerFirst.ownMinions[p.playerFirst.ownMinions.Count - 1].maxHp)
                        {
                            p.playerFirst.ownMinions[p.playerFirst.ownMinions.Count - 1].wounded = true;
                        }
                    }
                }
            }
            else
            {
                if (p.playerSecond.ownMinions.Count >= 1)
                {
                    if (p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].handcard.card.cardIDenum == kid.cardIDenum)
                    {
                        p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].Hp = 1;
                        p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].wounded = false;
                        if (p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].Hp < p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].maxHp)
                        {
                            p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].wounded = true;
                        }
                    }
                }
            }

        }

    }
}