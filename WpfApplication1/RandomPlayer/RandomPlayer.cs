using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverfishAi
{
    class RandomMoveGenerator
    {
        public HRSim.Playfield playfield;

        RandomMoveGenerator(HRSim.Playfield playfield) {
            this.playfield = playfield;
        }

        public List<Action> generatePossibleMove(bool isFirstPlayer)
        {
            List<Playfield> posmoves = new List<Playfield>(7000);

            Silverfish mPlayer;
            if (isFirstPlayer)
            {
                mPlayer = (Silverfish) HRSim.GameManager.Instance.playerFirst;
            }
            else {
                mPlayer = (Silverfish)HRSim.GameManager.Instance.playerSecond;
            }

            mPlayer.hrtprozis.updatePositions();

            posmoves.Clear();
            posmoves.Add(new Playfield(mPlayer));
            List<Action> actions = null;
            actions = mPlayer.Movegenerator.getMoveList(posmoves[0], true, false, true);
            return actions;
        }

        public Action getRandomMove(bool isFirstPlayer)
        {
            List<Action> actions = generatePossibleMove(isFirstPlayer);
            if (actions != null){
                return actions[HRSim.GameManager.getRNG().Next(actions.Count)];
            }
            return null;
        }
    }
}
