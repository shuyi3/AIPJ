using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_294 : SimTemplate //mirrorentity
    {
        //todo secret
        //    geheimnis:/ wenn euer gegner einen diener ausspielt, beschwört ihr eine kopie desselben herbei.

        public override void onSecretPlay(Playfield p, bool ownplay, Minion target, int number)
        {

            int posi = (ownplay) ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;
            p.callKid(target.handcard.card, posi, ownplay);
            if (ownplay)
            {
                if (p.playerFirst.ownMinions.Count >= 1 && p.playerFirst.ownMinions[p.playerFirst.ownMinions.Count - 1].name == target.handcard.card.name)
                {
                    int e = p.playerFirst.ownMinions[p.playerFirst.ownMinions.Count - 1].entitiyID;
                    p.playerFirst.ownMinions[p.playerFirst.ownMinions.Count - 1].setMinionTominion(target);
                    p.playerFirst.ownMinions[p.playerFirst.ownMinions.Count - 1].entitiyID = e;
                    p.playerFirst.ownMinions[p.playerFirst.ownMinions.Count - 1].own = true;
                }
            }
            else
            {
                if (p.playerSecond.ownMinions.Count >= 1 && p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].name == target.handcard.card.name)
                {
                    int e = p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].entitiyID;
                    p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].setMinionTominion(target);
                    p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].entitiyID = e;
                    p.playerSecond.ownMinions[p.playerSecond.ownMinions.Count - 1].own = false;
                }
            }
        }

    }

}