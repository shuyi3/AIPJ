using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZeroMQ;
using Newtonsoft.Json;
using System.Linq;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace HRSim
{
    public class Node
    {
        public Node parent;
        public List<Node> children = new List<Node>();
        public float SFVal;
        public volatile int numVisited;
        public volatile float mean;
        public Playfield state;
        public Action move;
        public bool isExpanded;
        public int depth;

        public Node(Node n) //no children copied, should we recursively copy the children or just copy the refs?
        {
            this.parent = n.parent;
            this.numVisited = n.numVisited;
            this.mean = n.mean;
            this.state = new Playfield(n.state);
            this.depth = n.depth;
            this.move = n.move;
            this.SFVal = 0.0f;
        }

        public Node(Node parent, Playfield state, Action move, int depth)
        {
            this.parent = parent;
            numVisited = 0;
            mean = 0;
            this.state = state;
            this.move = move;
            this.depth = depth;
        }

        public void printChildren()
        {
            Helpfunctions.Instance.logg("=================childrens:");
            foreach (Node child in this.children)
            {
                if (child.move == null)
                {
                    Helpfunctions.Instance.logg("null move");
                }
                else
                {
                    child.move.print();
                }
                Helpfunctions.Instance.logg("visited:" + child.numVisited + ", Mean:" + child.mean);
            }
            Helpfunctions.Instance.logg("===========================");
        }
    }

    //public class ChanceNode : Node
    //{
    //    public Dictionary<ActionResult, int> moveNodeMap;
    //    public bool firstExpanded = false;
    //    public int number = GameManager.Instance.moveCount;

    //    public ChanceNode(Node parent, Playfield state, Action move, int depth)
    //        : base(parent, state, move, depth)
    //    {
    //        moveNodeMap = new Dictionary<ActionResult, int>();
    //    }

    //}

    public class ChanceNode : Node
    {
        public List<double> probability;
        public bool firstExpanded = false;
        public int chanceNodeNum;

        public ChanceNode(Node parent, Playfield state, Action move, int depth, int numCards)
            : base(parent, state, move, depth)
        {
            //GameManager.Instance.moveCount++;

            probability = new List<double>(5);
            Player mPlayer;
            List<CardDB.Card> deck;

            if (state.isOwnTurn)
            {
                mPlayer = state.playerFirst;
                deck = state.homeDeck;
            }
            else
            {
                mPlayer = state.playerSecond;
                deck = state.awayDeck;
            }

            List<List<CardDB.Card>> bucket = new List<List<CardDB.Card>>();

            if (move == null) //begin of turn draw
            {

                for (int i = 0; i < 5; i++)
                {
                    bucket.Add(new List<CardDB.Card>());
                }

                foreach (CardDB.Card card in deck)
                {
                    int index = 0;
                    for (index = 0; index < GameManager.bucketMana.Length; index++)
                    {
                        if (card.cost <= GameManager.bucketMana[index])
                            break;
                    }

                    bucket[index].Add(card);
                }

                double sum = 0;
                for (int i = 0; i < 5; i++)
                {
                    List<CardDB.Card> cardList = bucket[i];
                    if (bucket[i].Count == 0)
                        continue;
                    Playfield nextState = new Playfield(state);

                    int cardIndex = GameManager.getRNG().Next(cardList.Count);
                    CardDB.Card cardToDraw = cardList[cardIndex];
                    nextState.drawTurnStartCard(cardToDraw);

                    children.Add(new Node(this, nextState, null, depth));
                    double prob = (double)cardList.Count / deck.Count;
                    probability.Add(prob);
                    sum += prob;
                }

                if (sum < 0.99)
                {
                    int debug = 1;
                }
            }
            else //draw cards in turn
            {
                if (numCards == 1)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        bucket.Add(new List<CardDB.Card>());
                    }

                    foreach (CardDB.Card card in deck)
                    {
                        int index = 0;
                        for (index = 0; index < GameManager.bucketMana.Length; index++)
                        {
                            if (card.cost <= GameManager.bucketMana[index])
                                break;
                        }

                        bucket[index].Add(card);
                    }

                    double sum = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        List<CardDB.Card> cardList = bucket[i];
                        if (bucket[i].Count == 0)
                            continue;
                        Playfield nextState = new Playfield(state);

                        int cardIndex = GameManager.getRNG().Next(cardList.Count);
                        CardDB.Card cardToDraw = cardList[cardIndex];
                        nextState.addCardToDrawList(cardToDraw);
                        nextState.doAction(move);

                        children.Add(new Node(this, nextState, null, depth));
                        double prob = (double)cardList.Count / deck.Count;
                        probability.Add(prob);
                        sum += prob;
                    }

                    if (sum < 0.99)
                    {
                        int debug = 1;
                    }
                }
                if (numCards == 2) //3 buckets, 6 possible nodes
                {

                    bucket = new List<List<CardDB.Card>>(3);

                    for (int i = 0; i < 3; i++)
                    {
                        bucket.Add(new List<CardDB.Card>());
                    }

                    foreach (CardDB.Card card in deck)
                    {
                        int index = 0;
                        for (index = 0; index < GameManager.bucketManaFor3.Length; index++)
                        {
                            if (card.cost <= GameManager.bucketManaFor3[index])
                                break;
                        }

                        bucket[index].Add(card);
                    }

                    double sum = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        List<CardDB.Card> cardList = bucket[i];
                        if (bucket[i].Count == 0)
                            continue;

                        int cardIndex = GameManager.getRNG().Next(cardList.Count);
                        CardDB.Card cardToDraw = cardList[cardIndex];
                        double prob = (double)cardList.Count / deck.Count;
                        cardList.RemoveAt(cardIndex);

                        for (int j = 0; j < 3; j++)
                        {
                            List<CardDB.Card> secondCardList = bucket[j];
                            if (bucket[j].Count == 0)
                                continue;
                            Playfield nextState = new Playfield(state);

                            int secondCardIndex = GameManager.getRNG().Next(secondCardList.Count);
                            CardDB.Card secondcardToDraw = secondCardList[secondCardIndex];

                            //List<CardDB.Card> cardListToDraw = nextState.isOwnTurn ? nextState.ownCardsToDraw : nextState.enemyCardsToDraw;
                            //cardListToDraw.Add(cardToDraw);
                            //cardListToDraw.Add(secondcardToDraw);

                            //remove from deck first
                            nextState.addCardToDrawList(cardToDraw);
                            nextState.addCardToDrawList(secondcardToDraw);
                            nextState.doAction(move);

                            children.Add(new Node(this, nextState, null, depth));
                            double finalProb = prob * (double)secondCardList.Count / (deck.Count - 1);
                            probability.Add(finalProb);
                            sum += finalProb;
                        }

                        cardList.Insert(cardIndex, cardToDraw);
                    }

                    if (sum < 0.99)
                    {
                        int debug = 1;
                    }
                }
            }
        }
    }

    public class MCTSPlayer : PlayerAgent
    {
        public ZSocket requester;
        private int numThread = 2;
        private Node[] resultNodeList = new Node[2];
        private List<Action> moveList;
        public Behavior bh;

        bool playerSide;
        Playfield board;
        Node currentState;
        TranspositionTable tt;
        HandHashTable endTurnTt;
        bool isEndReached;
        float bestValue = float.MinValue;
        Playfield bestBoard;
        int rolloutDepth = 3;
        int firstNumberIter = 500;
        int playoutNumberIter = 500;
        bool log = false;
        int chanceCount = 0;
        int expandCount = 0;
        double expandTime = 0.0;
        int numExpand = 0;
        float constC = 0.7f;
        bool useNNEval = false;
        bool isOpenHand = true;
        //做partial order 或者 TT

        public MCTSPlayer(bool side, Playfield playfield, bool useNNEval)
        {
            this.playerSide = side;
            this.board = new Playfield(playfield);
            isEndReached = false;
            moveList = new List<Action>();
            bh = new BehaviorControl();
            this.useNNEval = useNNEval;
            //this.isOpenHand = isOpenHand;
        }

        public override void updateState(Playfield playfield)
        {
            this.board = new Playfield(playfield);
        }

        public override Action getMove()
        {
            if (moveList.Count == 0)
            {
                moveList = getBestPlayfield();
            }
 
            Action moveToPlay = moveList[0];
            moveList.RemoveAt(0);
            return moveToPlay;
        }

        public int expandDecision(Node p, int numIter) //0: chance node
        {
            bool lethalCheck = false;

            //Movegenerator.Instance.getMoveListForPlayfield(p.state, false, lethalCheck);
            p.state.moveList = new List<Action>(Movegenerator.Instance.getMoveList(p.state, lethalCheck, true, true));

            Playfield afterState = new Playfield(p.state);

            if (afterState.moveList.Count > 0)
            {
                p.children = new List<Node>(afterState.moveList.Count + 1);

                int i = 0;
                Node tn = null;
                Playfield nextState = new Playfield(afterState);

                foreach (Action a in afterState.moveList)
                {
                    nextState = new Playfield(afterState);
                    nextState.doAction(a);
                    tn = new Node(null, nextState, a, p.depth);
                    p.children.Add(tn);
                    i++;
                }
                return p.children.Count;
            }
            else
            {
               //No move, so it's a chance node
                afterState.endTurn(false, false);
                Node afterNode = new ChanceNode(p, afterState, null, p.depth + 1, 1);
                p.children.Add(afterNode);
                return 0;
            }
        }

        public Node advanceChance(ChanceNode p)
        {
            double prob = GameManager.getRNG().NextDouble();
            int i;
            double probSum = 0.0;
            for (i = 0; i < p.probability.Count; i++)
            {
                probSum += p.probability[i];
                if (prob < probSum)
                    break;
            }
            return p.children[i];
        }

        public void UCT(Node startNode)
        {
            int numIter = 8000;
            bool isEndReachedBefore = isEndReached;
            for (int i = 0; i < numIter; i++)
            {
                if (isEndReachedBefore != isEndReached)
                {
                    rolloutDepth = int.MaxValue;
                    break;
                }
                else
                {
                    //Helpfunctions.Instance.logg("try: " + i + " not reach");
                }
                UCTRun(currentState, constC);
                //multipleThreadUCTRun(currentState);

                if (i % 1000 == 0)
                    Console.WriteLine(i + " th iteration");

            }

            if (isEndReachedBefore != isEndReached)
            {
                board.moveList.Clear();
                currentState = new Node(null, board, null, 0);

                for (int i = 0; i < numIter; i++)
                {
                    UCTRun(currentState, constC);
                    //multipleThreadUCTRun(currentState);
                    if (i % 1000 == 0)
                        Console.WriteLine(i + " th iteration");
                    //currentState.state = new Playfield(board);
                }
            }      
        }

        public void sampleState(Node state)
        {
            if (this.playerSide)
            { 
            }
        }

        public List<Action> getBestPlayfield()
        {
            //Helpfunctions.Instance.startTimer();
            numExpand = 0;
            expandTime = 0.0;
            expandCount = 0;
            chanceCount = 0;
            Playfield selectedMove = null;
            bestValue = float.MinValue;
            //Playfield tempBoard = new Playfield(board);
            currentState = new Node(null, board, null, 0);
            List<Action> actionList = new List<Action>();

            if (expand(currentState, HeuristicType.LethalCheck, 500) == 1)
            {
                actionList.AddRange(currentState.children[0].state.getLastTurnAction());
                actionList.Add(null);
                return actionList;
            }

            int count = expandDecision(currentState, firstNumberIter);

            if (count == 0)
            {
                actionList.AddRange(currentState.children[0].state.getLastTurnAction());
                actionList.Add(null);
                return actionList;
            }

            //if (!this.isOpenHand)
            //{
            //    sampleWorld(currentState);
            //}

            UCT(currentState);

            int depth = currentState.depth;
            while (currentState.depth == depth)
            {
                int maxVisit = 0;
                Node selectedChild = null;
                foreach (Node child in currentState.children)
                {
                    if (child.numVisited > maxVisit)
                    {
                        maxVisit = child.numVisited;
                        selectedMove = child.state;
                        selectedChild = child;
                    }
                }

                currentState.printChildren();
                actionList.Add(selectedChild.move);
                currentState = selectedChild;

                if (selectedChild.numVisited <= 50)
                {
                    int result = expandDecision(currentState, firstNumberIter);
                    if (result != 0) //chance node
                    {
                        break;
                    }
                    else
                    {
                        UCT(currentState);
                    }
                }
            }
            return actionList;
        }

        //public Playfield getBestPlayfield()
        //{
        //    currentState = new Node(null, board, null, 0);

        //    int count = expandDecision(currentState, firstNumberIter);

        //    if (count == 0) //lethal
        //    {
        //        return currentState.children[0].state;
        //    }
        //    else if (count == 1)
        //    {
        //        return currentState.children[0].state;
        //    }

        //    float maxScore = 0;
        //    Playfield selectedMove = null;
        //    foreach (Node child in currentState.children)
        //    {
        //        //child.move.print();
        //        child.state.printLastTurnActions();
        //        float nnEval = getNNEval(child.state);
        //        Helpfunctions.Instance.logg("NN Score = " + nnEval);
        //        if (nnEval > maxScore)
        //        {
        //            maxScore = nnEval;
        //            selectedMove = child.state;
        //        }
        //        Helpfunctions.Instance.logg("============================================================");
        //    }

        //    selectedMove.printBoard();

        //    return selectedMove;
        //}

        //public void UCTRun(Node p, int threadNumber)
        //{
        //    List<Node> visited = new List<Node>();
        //    visited.Add(p);

        //    int depth = 0;
        //    Node parent = null;

        //    //List<Action> actions = new List<Action>();

        //    while (!isLeaf(p) && p.depth < rolloutDepth)
        //    {
        //        parent = p;
        //        p = select(p, 0.7f);

        //        //chanceNode
        //        ChanceNode chanceNode = p as ChanceNode;
        //        if (chanceNode != null)
        //        {
        //            visited.Add(p);
        //            p = advanceChance(chanceNode);
        //        }
        //        depth++;
        //        if (p.move != null && depth == 1)
        //        {
        //            //Helpfunctions.Instance.logg("=======================MOVE TO SAMPLE===================");
        //            //Helpfunctions.Instance.logg("Turn: " + p.state.isOwnTurn);
        //            //p.state.printBoard();
        //            //p.move.print();
        //        }
        //        //actions.Add(p.move);
        //        depth++;
        //        visited.Add(p);
        //    }

        //    //Console.WriteLine("OUT OF LOOP:" + threadNumber);

        //    float score;
        //    if (p.depth == rolloutDepth && !isEndReached)
        //    {
        //        score = p.state.getBoardValue();
        //        //score = getNNEval(p.state);
        //        //Console.WriteLine("NN value = " + score);
        //    }
        //    else
        //    {
        //        //int count = expand(p, HeuristicType.Boardvalue);
        //        //Helpfunctions.Instance.startTimer();
        //        //p.state.drawTurnStartCard();
        //        //ChanceNode cn = new ChanceNode(p, p.state, null, p.depth);
        //        int count = expandDecision(p, threadNumber);
        //        //Helpfunctions.Instance.logTime("expand");
        //        if (count == 0)
        //        {
        //            if (p.state.isOwnTurn == playerSide)
        //            {
        //                score = 1;
        //            }
        //            else
        //            {
        //                score = 0;
        //            }
        //        }
        //        else
        //        {
        //            //Helpfunctions.Instance.startTimer();
        //            score = sample(p);
        //            //Helpfunctions.Instance.logTime("sample time");
        //            //Helpfunctions.Instance.startTimer();
        //            //score = getNNEval(p);
        //            //Helpfunctions.Instance.logTime("NN time");
        //        }
        //    }
        //    //Helpfunctions.Instance.logg("score = " + score);

        //    //update score
        //    foreach (Node visitedPos in visited)
        //    {
        //        float lastMean = visitedPos.mean;
        //        visitedPos.mean = (score + lastMean * visitedPos.numVisited) / (visitedPos.numVisited + 1);
        //        visitedPos.numVisited++;
        //    }

        //    return;
        //}

        public void UCTRun(Node p, float c)
        {

            List<Node> visited = new List<Node>();
            visited.Add(p);

            int depth = 0;
            Node parent = null;

            //List<Action> actions = new List<Action>();
            //Console.WriteLine("Begin D:" + p.depth + ", T:" + p.state.isOwnTurn);

            while (!isLeaf(p) && p.depth < rolloutDepth)
            {
                parent = p;
                p = select(p, c);

                //chanceNode
                ChanceNode chanceNode = p as ChanceNode;
                if (chanceNode != null)
                {
                    visited.Add(p);
                    p = advanceChance(chanceNode);
                }               
                visited.Add(p);
                //Console.WriteLine("Select D:" + p.depth + ", T:" + p.state.isOwnTurn);
            }

            float score;
            if (p.depth >= rolloutDepth)
            {
                if (this.useNNEval)
                {
                    score = getDNNValue(p.state);
                }
                else
                {
                    score = bh.getPlayfieldValue(p.state, this.playerSide);
                }
                //score = bh.getPlayfieldValue(p.state, this.playerSide);
            }
            else
            {
                //int count = expand(p, HeuristicType.Boardvalue);
                //Helpfunctions.Instance.startTimer();
                //p.state.drawTurnStartCard();

                bool turnBefore = p.state.isOwnTurn;
                int depthBefore = p.depth;
                int count = expandDecision(p, playoutNumberIter);
                while (count == 0)
                {
                    visited.Add(p);
                    ChanceNode chanceNode = p.children[0] as ChanceNode;
                    p = advanceChance(chanceNode);
                    count = expandDecision(p, playoutNumberIter);
                    //Console.WriteLine("Ad D:" + p.depth + ", T:" + p.state.isOwnTurn);
                }

                //Helpfunctions.Instance.startTimer();
                score = sample(p);
                //Helpfunctions.Instance.logTime("sample time");
                //Helpfunctions.Instance.startTimer();
                //score = getNNEval(p);
                //Helpfunctions.Instance.logTime("NN time");
            }
            if (log)
                Console.WriteLine("score = " + score + "--------------------------------------");

            //update score
            foreach (Node visitedPos in visited)
            {
                float realScore = score;
                if (visitedPos.state.isOwnTurn != playerSide)
                {
                    realScore = 1.0f - score;
                }
                if (log)
                    Console.WriteLine("turn:" + visitedPos.state.isOwnTurn + " score:" + realScore);

                float lastMean = visitedPos.mean;
                visitedPos.mean = (realScore + lastMean * visitedPos.numVisited) / (visitedPos.numVisited + 1);
                visitedPos.numVisited++;
            }
        }

        private float getDNNValue(Playfield p)
        {
            dynamic encoded_feature = DNNEval.Instance.parsePlayfieldCNN(p, !this.playerSide);
            float stateValue = DNNEval.Instance.PredictValue(encoded_feature);
            return 1.0f - stateValue;
        }

        private string sendMessage(Playfield p)
        {
            List<PlayerKeyInfo> playerInfoList = new List<PlayerKeyInfo>(2);
            if (this.playerSide)
            {
                playerInfoList.Add(new PlayerKeyInfo(p.playerFirst, p.homeDeck, true, p));
                playerInfoList.Add(new PlayerKeyInfo(p.playerSecond, p.awayDeck, false, p));
            }
            else
            {
                playerInfoList.Add(new PlayerKeyInfo(p.playerSecond, p.awayDeck, true, p));
                playerInfoList.Add(new PlayerKeyInfo(p.playerFirst, p.homeDeck, false, p));
            }
            string message = JsonConvert.SerializeObject(playerInfoList);
            string result = null;
            //Helpfunctions.Instance.logTime("connect time");
            if (requester == null)
            {
                requester = new ZSocket(ZSocketType.REQ);
                requester.TcpKeepAlive = TcpKeepaliveBehaviour.Enable;
                requester.Connect("tcp://127.0.0.1:5556");
            }

            //using (var requester = new ZSocket(ZSocketType.REQ))
            //{
            // Connect
            requester.TcpKeepAlive = TcpKeepaliveBehaviour.Enable;
            //for (int n = 0; n < 10; ++n)
            //{
            //string requestText = "Hello";
            //Console.Write("Sending {0}...", message);
            //Helpfunctions.Instance.startTimer();
            // Send
            requester.Send(new ZFrame(message));

            // Receive
            using (ZFrame reply = requester.ReceiveFrame())
            {
                result = reply.ReadString();
                //Console.WriteLine(" Received: {0}", reply.ReadString());
            }
            //Helpfunctions.Instance.logTime("send time");
            //}

            //}
            return result;
        }

        public float sample(Node p)
        {
            Playfield startState = new Playfield(p.state);
            Action move = null;
            int turn = p.depth;
            if (log)
                Console.WriteLine("starting depth = " + turn);

            int score = startState.getGameResult();

            while (score == -1)
            {
                //if no move
                if (startState.moveList.Count == 0)
                {
                    startState.endTurn(false, false);
                    startState.drawTurnStartCard();
                    //if (this.useNNEval)
                    //{
                    //    DNNEval.Instance.getNNPolicy(startState, startState.isOwnTurn);
                    //}


                    turn++;
                    //Console.WriteLine("S D:" + turn + ", T:" + startState.isOwnTurn);

                    move = null;
                    if (turn == rolloutDepth) //evaluate at deapth == 5
                    {
                        //startState.printBoard();
                        float value;
                        if (this.useNNEval)
                        {
                            value = getDNNValue(startState);
                        }
                        else
                        {
                            value = bh.getPlayfieldValue(startState, this.playerSide);
                        }

                        if (log)
                            Console.WriteLine("ending depth = " + turn);
                        //float value = startState.getBoardValue();
                        if (log)
                            Console.WriteLine("sample turn:" + startState.isOwnTurn + " val:" + value);

                        if (startState.isOwnTurn == this.playerSide)
                        {
                            int debug = 1;
                        }

                        //startState.printBoard();
                        //float value = getNNEval(startState);
                        //Helpfunctions.Instance.logg("NN value = " + value);
                        //Console.WriteLine("Board value = " + value);
                        if (value > bestValue)
                        {
                            bestBoard = new Playfield(startState);
                            bestValue = value;
                        }
                        return value;
                    }
                 
                }
                else
                {
                    //startState.doAction(startState.moveList[4]);
                    int moveIndex = GameManager.getRNG().Next(startState.moveList.Count);
                    move = startState.moveList[moveIndex];
                    startState.doAction(move);
                    //Console.WriteLine("S D:" + turn + ", T:" + startState.isOwnTurn + ", M:" + move.actionType);
                }

                //Movegenerator.Instance.getMoveListForPlayfield(startState, false, false);
                startState.moveList = new List<Action>(Movegenerator.Instance.getMoveList(startState, false, true, true));
                score = startState.getGameResult();
            }

            isEndReached = true;
            if ((playerSide && score == 0) || (!playerSide && score == 1))
            {
                //Helpfunctions.Instance.logg("End game value = 1");
                return 1;
            }
            //Helpfunctions.Instance.logg("End game value = 0");
            return 0;
        }

        public int expand(Node p, HeuristicType ht, int numberIter)
        {
            Playfield afterState = new Playfield(p.state);

            ParetoMCTSPlayer m_player = new ParetoMCTSPlayer(new ParetoTreePolicy(0.7), GameManager.getRNG(), afterState, ht);
            m_player.run(afterState, numberIter, false);
            //m_player.m_root.printStats();
            //Helpfunctions.Instance.logg("turn: " + p.state.isOwnTurn);
            int memberSize = m_player.m_root.pa.m_members.size(); // will it always > 0?

            //if (ht == HeuristicType.Boardvalue)
            //    m_player.m_root.pa.printArchive();


            if (ht == HeuristicType.DrawCard)
            {
                Console.WriteLine("chance: " + memberSize);
                if (memberSize > 1)
                    chanceCount += memberSize - 1;
                m_player.m_root.pa.printArchive();
            }
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

                    if ((pf.isOwnTurn && pf.playerFirst.ownDeckSize > 0) ||
                        (!pf.isOwnTurn && pf.playerSecond.ownDeckSize > 0))
                    {
                        List<Action> actionList = pf.getActions();
                        Playfield tempPf = new Playfield(afterState);
                        for (int j = tempPf.getActions().Count; j < actionList.Count - 1; j++)
                        {
                            tempPf.doAction(actionList[j]);
                        }
                        afterNode = new ChanceNode(p, tempPf, actionList[actionList.Count - 1], p.depth, pf.moveTrigger.newHandcardList.Count);
                    }
                    else
                    {
                        afterNode = new Node(p, pf, null, p.depth);
                    }

                    if (memberSize > 1)
                    {
                        pf.printActions();
                    }
                    //pf.printActions();
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
                    if ((pf.isOwnTurn && pf.playerFirst.ownDeckSize > 0) ||
                        (!pf.isOwnTurn && pf.playerSecond.ownDeckSize > 0))
                    {
                        afterNode = new ChanceNode(p, pf, null, p.depth + 1, 1);
                    }
                    else
                    {
                        pf.drawTurnStartCard();
                        afterNode = new Node(p, pf, null, p.depth + 1);
                    }
                }
                p.children.Add(afterNode);
            }

            return p.children.Count;
        }

        //public int expand(Node p)
        //{

        //    GameManager.Instance.moveCount++;
        //    if (GameManager.Instance.moveCount == 225)
        //    {
        //         int debug = 1;
        //    }

        //    int state = 0;
        //    Playfield afterState = new Playfield(p.state);

        //    tt = new TranspositionTable();
        //    endTurnTt = new HandHashTable();
        //    List<Playfield> moves = new List<Playfield>();
        //    Movegenerator.Instance.getMoveListForPlayfield(afterState, null, false);

        //    int counter = GameManager.Instance.moveCount;

        //    getAllpossibleStates(afterState, ref moves);

        //    if (GameManager.Instance.moveCount - counter > 1000)
        //    {
        //        Helpfunctions.Instance.logg("MOVES = " + (GameManager.Instance.moveCount - counter));
        //        int debug = 1;
        //    }

        //    foreach (Playfield pf in moves)
        //    {
        //        if (pf.moveList.Count != 0)
        //        {
        //            int debug = 1;
        //        }
        //    }

        //    Helpfunctions.Instance.logg("try size = " + endTurnTt.TTable.Count);

        //    //while (endTurnTt.TTable.Count == 1)
        //    //{ // no moves available, change side
        //    //    tt.clearTable();
        //    //    endTurnTt.clearTable();
        //    //    afterState.endTurn(false, false);
        //    //    Movegenerator.Instance.getMoveListForPlayfield(afterState, null, false);
        //    //    getAllpossibleStates(afterState, ref moves);
        //    //    state = 1;
        //    //}

        //    if (endTurnTt.TTable.Count == 1)
        //        state = 1;

        //    //afterState.endTurn(false, false);
        //    //Node originalNode = new Node(new Playfield(afterState), null, p.depth + 1);

        //    //p.children.Add(originalNode);


        //    foreach (KeyValuePair<float, List<Playfield>> move in endTurnTt.TTable)
        //    {
        //        Node afterNode = new Node(new Playfield(move.Value[0]), null, p.depth + 1);
        //        //afterNode.state.doAction(move);
        //        p.children.Add(afterNode);
        //    }

        //    Helpfunctions.Instance.logg("expand size = " + endTurnTt.TTable.Count);
        //    //if (moves.Count > 100)
        //    //{
        //    //    foreach (Playfield pf in moves)
        //    //    {
        //    //        Helpfunctions.Instance.logg("key:" + endTurnTt.getHashkey(pf));
        //    //        //pf.debugHand();
        //    //    }
        //    //}
        //    return state;
        //}

        public void getAllpossibleStates(Playfield state, ref List<Playfield> statesList)
        {
            if (state.moveList.Count == 0)
            {
                Player mPlayer;
                String turn;
                //if (state.isOwnTurn)
                //{
                //    mPlayer = state.playerSecond;
                //    turn = "second";
                //}
                //else
                //{
                //    mPlayer = state.playerFirst;
                //    turn = "first";
                //}

                //Helpfunctions.Instance.logg(turn + ": pre mana = " + mPlayer.ownMaxMana);
                Playfield afterState = new Playfield(state);
                afterState.endTurn(false, false);
                afterState.drawTurnStartCard();

                if (endTurnTt.addToMap(afterState) == false)
                {
                    //GameManager.Instance.moveCount++;
                    //Helpfunctions.Instance.logg("move = " + GameManager.Instance.moveCount +
                    //    ",mana = " + mPlayer.ownMaxMana);
                    //Helpfunctions.Instance.logg("MOVES ===============================");
                    //state.printActions();
                    //Helpfunctions.Instance.logg("count: " + statesList.Count);
                    //statesList.Add(afterState);
                }
                return;
            }
            foreach (Action action in state.moveList)
            {
                Playfield afterState = new Playfield(state);
                afterState.doAction(action);
                //Movegenerator.Instance.getMoveListForPlayfield(afterState, false, false);
                afterState.moveList = new List<Action>(Movegenerator.Instance.getMoveList(afterState, false, true, true));


                //if (action.actionType == actionEnum.playcard && action.card.card.name == CardDB.cardName.arcaneintellect)
                //{
                //    if (tt.addToMap(state) == false)
                //    {
                //        state.endTurn(false, false);
                //        statesList.Add(state);
                //    }
                //    continue;
                //}
                if (tt.addToMap(afterState) == false)
                {
                    //    statesList.Add(afterState);

                    //Helpfunctions.Instance.logg("Turn: " + afterState.isOwnTurn);
                    getAllpossibleStates(afterState, ref statesList);
                }
                else
                {
                    Helpfunctions.Instance.logg("pruned");
                }
            }
        }


        public bool isLeaf(Node p)
        {
            return p.children.Count == 0;
        }

        public Node select(Node p, float c)
        {

            float bestValue = float.MinValue;
            Node selected = null;

            //if (p.state.isOwnTurn == playerSide)
            //{

            foreach (Node child in p.children)
            {

                if (child.numVisited == 0)
                {
                    return child;
                }

                float UCTValue = (float)(child.mean + c * Math.Sqrt(Math.Log(p.numVisited) / child.numVisited));

                //if (child.move != null && p.depth == 0) 
                //    child.state.debugMinions();
                //Helpfunctions.Instance.logg("UCTVale = " + UCTValue);

                if (UCTValue > bestValue)
                {
                    bestValue = UCTValue;
                    selected = child;
                }

                //    }
                //}
                //else
                //{

                //    bestValue = float.MaxValue;

                //    foreach (Node child in p.children)
                //    {

                //        if (child.numVisited == 0)
                //        {
                //            return child;
                //        }

                //        float UCTValue = (float)(child.mean - c * Math.Sqrt(Math.Log(p.numVisited) / child.numVisited));


                //        if (UCTValue < bestValue)
                //        {
                //            bestValue = UCTValue;
                //            selected = child;
                //        }

                //    }

            }

            if (selected == null)
            {
                selected = p.children[GameManager.getRNG().Next(p.children.Count)];
            }

            return selected;

        }

    }
}
