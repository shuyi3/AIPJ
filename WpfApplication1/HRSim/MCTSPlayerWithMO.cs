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
                Helpfunctions.Instance.logg("visited:" + child.numVisited);
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
            if (GameManager.Instance.moveCount == 105924)
            {
                int debug = 1;
            }

            chanceNodeNum = GameManager.Instance.moveCount;
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
        private float c = 0.7f;
        private Node[] resultNodeList = new Node[2];
        private List<Action> moveList = new List<Action>();
        public Behavior bh;

        bool playerSide;
        Playfield board;
        Node currentState;
        TranspositionTable tt;
        HandHashTable endTurnTt;
        bool isEndReached;
        float bestValue = float.MinValue;
        Playfield bestBoard;
        int rolloutDepth = 5;
        int firstNumberIter = 3000;
        int playoutNumberIter = 500;
        bool log = false;
        int chanceCount = 0;
        int expandCount = 0;
        double expandTime = 0.0;
        int numExpand = 0;
        
        public MCTSPlayer(bool side, Playfield playfield)
        {
            this.playerSide = side;
            this.board = new Playfield(playfield);
            isEndReached = false;
            moveList = null;
            bh = new BehaviorControl();
        }

        public override void updateState(Playfield playfield)
        {
            this.board = new Playfield(playfield);
        }

        public override Action getMove()
        {
            if (moveList == null)
            {
                moveList = getBestPlayfield();
            }
            if (moveList.Count == 0)
            {
                moveList = null;
                return null;
            }
            Action moveToPlay = moveList[0];
            moveList.RemoveAt(0);
            return moveToPlay;
        }

        //public override Action getMove()
        //{
        //    bestValue = float.MinValue;
        //    bestBoard = new Playfield(board);
        //    currentState = new Node(null, board, null, 0);

        //    if (expand(currentState, HeuristicType.Boardvalue) == 1) return null; // no moves
        //    bool isEndReachedBefore = isEndReached;
        //    for (int i = 0; i < 3000; i++)
        //    {
        //        //Helpfunctions.Instance.logg("try: " + i);
        //        //counter++;
        //        //if (counter == 10) {
        //        //    Helpfunctions.Instance.logg("try: " + i);
        //        //    counter = 0;
        //        //}
        //        //if (i == 752) {
        //        //    int debug = 1;
        //        //}
        //        if (isEndReachedBefore != isEndReached)
        //        {
        //            //Helpfunctions.Instance.logg("try: " + i + " reach");
        //            break;
        //        }
        //        else
        //        {
        //            //Helpfunctions.Instance.logg("try: " + i + " not reach");
        //        }
        //        UCTRun(currentState, 0.7f);
        //        //currentState.state = new Playfield(board);
        //    }

        //    if (isEndReachedBefore != isEndReached)
        //    {
        //        currentState = new Node(null, board, null, 0);

        //        if (expand(currentState, HeuristicType.Boardvalue) == 1) return null; // no moves
        //        for (int i = 0; i < 10000; i++)
        //        {
        //            UCTRun(currentState, 0.7f);
        //            currentState.state = new Playfield(board);
        //        }
        //    }

        //    int maxVisit = 0;
        //    Action selectedMove = null;
        //    Node selectedChild = null;
        //    foreach (Node child in currentState.children)
        //    {
        //        //child.move.print();
        //        //Helpfunctions.Instance.logg("count = " + child.numVisited);
        //        if (child.numVisited > maxVisit)
        //        {
        //            maxVisit = child.numVisited;
        //            selectedMove = child.move;
        //            selectedChild = child;
        //        }
        //    }

        //    //Helpfunctions.Instance.logg("Turn of child:" + selectedChild.state.isOwnTurn);
        //    //currentState.printChildren();
        //    Helpfunctions.Instance.logg("best value:" + bestValue);
        //    bestBoard.printBoard();

        //    return selectedMove;
        //}

        public int multiThreadexpandDecision(Node p) //0: lethal 
        {
            List<System.Threading.Thread> tasks = new List<System.Threading.Thread>(numThread);
            for (int k = 0; k < numThread; k++)
            {
                Node curNode = new Node(p);
                resultNodeList[k] = curNode;
                int i = k;
                //System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(this.parallelUCTRun));
                System.Threading.Thread thread = new System.Threading.Thread(
                    () => expandDecision(curNode, firstNumberIter));
                thread.Start();
                tasks.Add(thread);
            }

            for (int j = 0; j < numThread; j++)
            {
                tasks[j].Join();
                Console.WriteLine("thread " + j + " joined");
                foreach (Node child in resultNodeList[j].children)
                {
                    //child.move.print();
                    child.state.printLastTurnActions();
                    Helpfunctions.Instance.logg("count = " + child.numVisited);
                    Helpfunctions.Instance.logg("============================================================");
                }
            }

            //if (expand(p, HeuristicType.LethalCheck) == 1) return 0;
            //expand(p, HeuristicType.Boardvalue); // no moves
            return p.children.Count;
        }

        public int expandDecision(Node p, int numIter) //0: lethal 
        {
            Helpfunctions.Instance.startTimer();
            if (expand(p, HeuristicType.LethalCheck, playoutNumberIter) == 1) return 0;
            expand(p, HeuristicType.Boardvalue, numIter); // no moves

            int beforeCount = p.children.Count;

            expandCount += beforeCount;
            //if (beforeCount > 30)
            //{
            //    Helpfunctions.Instance.logg("============================================================");
            //    foreach (Node child in p.children)
            //    {
            //        //child.move.print();
            //        if (child.move == null)
            //        {
            //            child.state.printLastTurnActions();
            //        }
            //        else
            //        {
            //            Helpfunctions.Instance.logg("Chance Node");
            //            child.state.printActions();
            //            child.move.print();
            //        }
            //        Helpfunctions.Instance.logg("============================================================");
            //    }
            //}

            if (p.children.Count > 10) //get top 10
            {
                foreach (Node child in p.children)
                {
                    child.SFVal = bh.getPlayfieldValue(child.state, this.playerSide);
                }
                p.children = p.children.OrderByDescending(x => x.SFVal).Take(10).ToList();
            }

            expand(p, HeuristicType.DrawCard, numIter);
            Console.WriteLine("beforeCount: " + beforeCount + ", expand bf:" + p.children.Count);
            double timeElapsed = Helpfunctions.Instance.logTime();
            expandTime += timeElapsed;
            numExpand++;
            return p.children.Count;
        }

        public int expandDecision(Node p, int numberIter, int threadNumber) //0: lethal //-2 is expanded by other threads
        {
            lock (p)
            {
                if (isLeaf(p))
                {
                    //Console.WriteLine("Node is expanded by " + threadNumber);
                    if (expand(p, HeuristicType.LethalCheck, numberIter) == 1) return 0;
                    expand(p, HeuristicType.Boardvalue, numberIter); // no moves
                    //expand(p, HeuristicType.DrawCard);

                    return p.children.Count;
                }
                //Console.WriteLine("try expand by " + threadNumber + ", but failed");
                return -2;
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

        //public Node advanceChance(ChanceNode p)
        //{

        //    List<Action> actions = p.state.getActions();
        //    Playfield tempState;

        //    if (!p.firstExpanded)
        //    {
        //        tempState = new Playfield(p.parent.state);
        //        for (int i = 0; i < actions.Count - 1; ++i)
        //        {
        //            tempState.doAction(actions[i]);
        //        }
        //        p.state = tempState;
        //    }
        //    else
        //    {
        //        tempState = new Playfield(p.state); 
        //    }
            
        //    tempState.doAction(actions[actions.Count - 1]);
        //    ActionResult ar = new ActionResult(p.state.moveTrigger.newHandcardList.ToArray());
        //    Node nextNode;

        //    if (!p.moveNodeMap.ContainsKey(ar))
        //    {
        //        //new node
        //        nextNode = new Node(p, tempState, actions[actions.Count - 1], p.depth + 1);
        //        p.children.Add(nextNode);
        //        p.moveNodeMap.Add(ar, p.children.Count - 1);
        //    }
        //    else
        //    {
        //        nextNode = p.children[p.moveNodeMap[ar]];
        //    }

        //    //expand the node here
        //    return nextNode;         
        //}

        //private void parallelUCTRun(Node currentState, int threadNumber)
        //{
        //    bool isEndReachedBefore = isEndReached;        
        //    for (int i = 0; i < 500; i++)
        //    {
        //        //Console.WriteLine("isEndReached = " + isEndReached);
        //        if (isEndReachedBefore != isEndReached)
        //        {
        //            break;
        //        }
        //        else
        //        {
        //            //Helpfunctions.Instance.logg("try: " + i + " not reach");
        //        }
        //        //UCTRun(currentState, 0.7f);
        //        UCTRun(currentState, threadNumber);
        //        if (i % 200 == 0)
        //            Console.WriteLine("thread " + threadNumber + ":" + i + " th iteration");
        //    }
        //}

        //private void multipleThreadUCTRun(Node startNode)
        //{
        //    List<System.Threading.Thread> tasks = new List<System.Threading.Thread>(numThread);
        //    for (int k = 0; k < numThread; k++)
        //    {
        //        //Node curNode = new Node(startNode);
        //        //resultNodeList[k] = curNode;
        //        int i = k;
        //        //System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(this.parallelUCTRun));
        //        System.Threading.Thread thread = new System.Threading.Thread(
        //            () => parallelUCTRun(startNode, i));
        //        thread.Start();
        //        tasks.Add(thread);
        //    }

        //    //System.Threading.Thread.Sleep(1);

        //    for (int j = 0; j < numThread; j++)
        //    {
        //        tasks[j].Join();
        //        Console.WriteLine("thread " + j + " joined");
        //        foreach (Node child in startNode.children)
        //        {
        //            //child.move.print();
        //            child.state.printLastTurnActions();
        //            Helpfunctions.Instance.logg("count = " + child.numVisited);
        //            Helpfunctions.Instance.logg("============================================================");
        //        }
        //    }
        //}

        public List<Action> getBestPlayfield()
        {
            //Helpfunctions.Instance.startTimer();
            numExpand = 0;
            expandTime = 0.0;
            expandCount = 0;
            chanceCount = 0;
            Playfield selectedMove = null;
            bestValue = float.MinValue;
            //bestBoard = new Playfield(board);
            currentState = new Node(null, board, null, 0);

            Player mPlayer = board.isOwnTurn ? board.playerFirst : board.playerSecond;

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                Console.WriteLine("Keep cards " + hc.card.name + "?"); // Prompt
                string line = Console.ReadLine();
                if (line != "1") //keep
                {
                    board.keepCardList.Add(hc.entity);
                    Console.WriteLine("keep " + hc.entity);
                }
                else
                {
                    Console.WriteLine("Play " + hc.entity);
                }
            }

            int count = expandDecision(currentState, firstNumberIter);

            if (count == 0) //lethal
            {
                return currentState.children[0].state.getLastTurnAction();
            }
            else if (count == 1)
            {
                return currentState.children[0].state.getLastTurnAction();
            }

            //bool isEndReachedBefore = isEndReached;        
            //multipleThreadUCTRun(currentState);

            //if (isEndReachedBefore != isEndReached)
            //{
            //    currentState = new Node(null, board, null, 0);

            //    //if (expand(currentState, HeuristicType.Boardvalue) == 1) return null; // no moves
            //    if (expandDecision(currentState) == 1)
            //    {
            //        return currentState.children[0].state;
            //    }                
            //    multipleThreadUCTRun(currentState);                
            //}
            int numIter = 500;
            bool isEndReachedBefore = isEndReached;
            for (int i = 0; i < numIter; i++)
            {
                if (isEndReachedBefore != isEndReached)
                {
                    break;
                }
                else
                {
                    //Helpfunctions.Instance.logg("try: " + i + " not reach");
                }
                UCTRun(currentState, 0.7f);
                //multipleThreadUCTRun(currentState);
            
                if (i % 200 == 0)
                    Console.WriteLine(i + " th iteration");

            }

            if (isEndReachedBefore != isEndReached)
            {
                currentState = new Node(null, board, null, 0);

                //if (expand(currentState, HeuristicType.Boardvalue) == 1) return null; // no moves
                if (expandDecision(currentState, firstNumberIter) == 1)
                {
                    return currentState.children[0].state.getLastTurnAction();
                }
                for (int i = 0; i < numIter; i++)
                {
                    UCTRun(currentState, 0.7f);
                    //multipleThreadUCTRun(currentState);
                    if (i % 200 == 0)
                        Console.WriteLine(i + " th iteration");
                    //currentState.state = new Playfield(board);
                }
            }

            int maxVisit = 0;           
            Node selectedChild = null;
            foreach (Node child in currentState.children)
            {
                //child.move.print();
                if (child.move == null)
                {
                    child.state.printLastTurnActions();
                }
                else
                {
                    Helpfunctions.Instance.logg("Chance Node");
                    child.state.printActions();
                    child.move.print();
                }
                Helpfunctions.Instance.logg("count = " + child.numVisited);
                Helpfunctions.Instance.logg("mean = " + child.mean);
                float sfVal = bh.getPlayfieldValue(child.state, this.playerSide);

                Helpfunctions.Instance.logg("sfVal:" + sfVal);
                if (child.numVisited > maxVisit)
                {
                    maxVisit = child.numVisited;
                    selectedMove = child.state;
                    selectedChild = child;
                }
                Helpfunctions.Instance.logg("============================================================");
            }

            Helpfunctions.Instance.logg("avg expandCount = " + expandCount / numExpand);
            Helpfunctions.Instance.logg("ChanceCount = " + chanceCount);
            Helpfunctions.Instance.logg("avg expand time = " + expandTime / numExpand);

            //Helpfunctions.Instance.logg("Turn of child:" + selectedChild.state.isOwnTurn);
            //currentState.printChildren();
            selectedMove.printBoard();

            if (selectedMove.isOwnTurn == this.playerSide)
            {
                List<Action> actionList = selectedMove.getActions();
                actionList.Add(selectedChild.move);
                return actionList;
            }
            else
            {
                return selectedMove.getLastTurnAction();
            }

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
                //depth++;
                if (p.move != null && depth == 1)
                {
                    //Helpfunctions.Instance.logg("=======================MOVE TO SAMPLE===================");
                    //Helpfunctions.Instance.logg("Turn: " + p.state.isOwnTurn);
                    //p.state.printBoard();
                    //p.move.print();
                }
                //actions.Add(p.move);
                depth++;
                visited.Add(p);
            }

            float score;
            if (p.depth == rolloutDepth && !isEndReached)
            {
                //score = p.state.getBoardValue();
                GameManager.Instance.moveCount++;
                score = bh.getPlayfieldValue(p.state, this.playerSide);
                //score = getNNEval(p.state);
                //Helpfunctions.Instance.logg("NN value = " + score);
                //Console.WriteLine("NN value = " + score);
            }
            else
            {
                //int count = expand(p, HeuristicType.Boardvalue);
                //Helpfunctions.Instance.startTimer();
                p.state.drawTurnStartCard();
                int count = expandDecision(p, playoutNumberIter);
                //Console.WriteLine("turn:" + p.state.isOwnTurn);
                //Helpfunctions.Instance.logTime("expand");
                if (count == 0)
                {
                    if (p.state.isOwnTurn == playerSide)
                    {
                        score = 1;
                    }
                    else
                    {
                        score = 0;
                    }
                }
                else
                {
                    //Helpfunctions.Instance.startTimer();
                    score = sample(p);
                    //Helpfunctions.Instance.logTime("sample time");
                    //Helpfunctions.Instance.startTimer();
                    //score = getNNEval(p);
                    //Helpfunctions.Instance.logTime("NN time");
                }
            }
            if (log)
                Console.WriteLine("score = " + score + "--------------------------------------");

            //update score
            foreach (Node visitedPos in visited)
            {
                float realScore = score;
                if (visitedPos.state.isOwnTurn == playerSide)
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

        public float getNNEval(Playfield p)
        {
            string result = sendMessage(p);
            return float.Parse(result);
        }

        private string sendMessage(Playfield p)
        {
            List<PlayerKeyInfo> playerInfoList = new List<PlayerKeyInfo>(2);
            if (this.playerSide)
            {
                playerInfoList.Add(new PlayerKeyInfo(p.playerFirst, p.homeDeck));
                playerInfoList.Add(new PlayerKeyInfo(p.playerSecond, p.awayDeck));
            }
            else
            {
                playerInfoList.Add(new PlayerKeyInfo(p.playerSecond, p.awayDeck));
                playerInfoList.Add(new PlayerKeyInfo(p.playerFirst, p.homeDeck));
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

            //Console.WriteLine("start sample ...");

            int score = startState.getGameResult();
            while (score == -1)
            {
                Movegenerator.Instance.getMoveListForPlayfield(startState, false ,false);                 
                if (startState.moveList.Count == 0)
                {
                    startState.endTurn(false, false);
                    startState.drawTurnStartCard();
                    //Console.WriteLine("turn:" + turn);
                    turn++;

                    if (!isEndReached)
                    {
                        move = null;
                        if (turn == rolloutDepth) //evaluate at deapth == 5
                        {
                            //startState.printBoard();
                            float value = bh.getPlayfieldValue(startState, this.playerSide);

                            //float value = startState.getBoardValue();
                            if (log)
                                Console.WriteLine("sample turn:" + startState.isOwnTurn + " val:" + value);
                            //startState.printBoard();
                            if (startState.isOwnTurn == playerSide)
                            {
                                int debug = 1;
                            }
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
                }
                else
                {
                    move = startState.moveList[GameManager.getRNG().Next(startState.moveList.Count)];
                    startState.doAction(move);
                }
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

        public float sampleTurn(Node p, float retVal)
        {
            Action move = null;
            int turn = 0;
            float count = retVal;
            int score = p.state.getGameResult();
            Node nextNode = p;
            while (score == -1)
            {
                if (turn != 0)
                    count = expandDecision(p, playoutNumberIter);

                if (count == 0) break;
                //choose a random state, no need to end turn   

                nextNode = nextNode.children[GameManager.getRNG().Next(nextNode.children.Count)];
                score = nextNode.state.getGameResult();
                nextNode.state.drawTurnStartCard();
                turn++;
            }

            isEndReached = true;
            if ((playerSide && score == 0) || (!playerSide && score == 1))
            {
                return 1;
            }
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
                {
                    chanceCount += memberSize - 1;
                    m_player.m_root.pa.printArchive();
                }
            }
            for(int i = 0; i < memberSize; i++) // this is other's turn
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
                Movegenerator.Instance.getMoveListForPlayfield(afterState, false, false);
                
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
