namespace HRSim
{
    using System.Collections.Generic;
    using System;

    public class HandHashTable : HashTable
    {

        public HandHashTable()
        {
            TTable = new Dictionary<float, List<Playfield>>();
        }

        //Return true is already exist
        // false if non-exist
        public override bool addToMap(Playfield state)
        {
            float key = getHashkey(state);
            //Helpfunctions.Instance.logg("end key = " + key);
            if (TTable.ContainsKey(key))
            {
                foreach (Playfield existState in TTable[key])
                {
                    if (existState.isHandEqual(state, true))
                    {
                        return true;
                    }
                }
                if (TTable[key][0].getBoardValue() < state.getBoardValue())
                {
                    //Helpfunctions.Instance.logg("orig board " + TTable[key][0].getBoardValue());
                    //TTable[key][0].debugMinions();
                    //Helpfunctions.Instance.logg("new board " + state.getBoardValue());
                    //state.debugMinions();
                    TTable[key][0] = state;
                } 
                return false;
            }
            else
            {
                TTable.Add(key, new List<Playfield>());
                TTable[key].Add(state);
            }
            return false;
        }

        public override float getHashkey(Playfield p)
        {
            Player mPlayer;
            if (p.isOwnTurn) //end turn evaluation, so opponent
            {
                mPlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
            }

            float handValue = 0f;

            foreach (Handmanager.Handcard hc in p.playerFirst.owncards)
            {
                if (hc.card.type == CardDB.cardtype.MOB)
                {
                    handValue += hc.card.cost * (int)(hc.card.name);
                }
                else
                {
                    handValue += (float)Math.Sqrt(hc.card.cost) * (int)(hc.card.name);
                }
            }

            return handValue;
        }
    }
}
