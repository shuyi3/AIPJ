namespace HRSim
{
    using System;
    using ZeroMQ;

    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class LearningPlayer : PlayerAgent
    {
        int playerSide;
        Playfield board;
        ZSocket requester;

        public LearningPlayer(int side, Playfield playfield)
        {
            this.playerSide = side;
            this.board = new Playfield(playfield);
        }

        public override void updateState(Playfield playfield)
        {
            this.board = new Playfield(playfield);
        }

        public int expandDecision(Node p) //0: lethal 
        {
            if (expand(p, HeuristicType.LethalCheck) == 1) return 0;
            expand(p, HeuristicType.Boardvalue); // no moves


            //expand(p, HeuristicType.DrawCard);

            return p.children.Count;

        }

        public int expand(Node p, HeuristicType ht)
        {
            Playfield afterState = new Playfield(p.state);

            ParetoMCTSPlayer m_player = new ParetoMCTSPlayer(new ParetoTreePolicy(0.7), GameManager.getRNG(), afterState, ht);
            m_player.run(afterState, 30000, false);
            //m_player.m_root.printStats();
            //Helpfunctions.Instance.logg("turn: " + p.state.isOwnTurn);
            int memberSize = m_player.m_root.pa.m_members.size(); // will it always > 0?

            for (int i = 0; i < memberSize; i++) // this is other's turn
            {
                Node afterNode;
                Playfield pf = m_player.m_root.pa.m_members.get(i).m_state;
                if (ht == HeuristicType.DrawCard)
                {
                    if (pf.moveTrigger.newHandcardList.Count == 0)
                    {
                        continue;
                    }

                    afterNode = new ChanceNode(p, pf, null, p.depth, 1); //last param is wrong
                    pf.printActions();
                }
                else if (ht == HeuristicType.LethalCheck)
                {
                    if (pf.getLethalScore() == 1.0)
                    {
                        afterNode = new Node(p, pf, null, p.depth + 1);
                    }
                    continue;
                }
                else
                {
                    afterNode = new Node(p, pf, null, p.depth + 1);
                }
                p.children.Add(afterNode);
            }

            return p.children.Count;
        }


        public Playfield getBestPlayfield()
        {
            float bestValue = float.MinValue;
            Playfield bestBoard = new Playfield(board);
            Node currentState = new Node(null, board, null, 0);

            int count = expandDecision(currentState);

            if (count == 0) //lethal
            {
                return currentState.children[0].state;
            }
            else if (count == 1)
            {
                return currentState.children[0].state;
            }

            //choose a action based on the value function
            int selectedIndex = GameManager.getRNG().Next(currentState.children.Count);
            Helpfunctions.Instance.logg("choose child: " + selectedIndex);
            Playfield selectedMove = currentState.children[selectedIndex].state;

            return selectedMove;
        }
    }
}
