using System;
using System.Collections.Generic;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace HRSim
{
    public class Node
    {
        public List<Node> children = new List<Node>();
        public int numVisited;
        public float mean;
        public Playfield state;
        public Action move;
        public bool isExpanded;
        public int depth;

        public Node(Playfield state, Action move, int depth)
        {
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

    public class MCTSPlayer : PlayerAgent
    {
        int playerSide;
        Playfield board;
        Node currentState;
        TranspositionTable tt;
        HandHashTable endTurnTt;
        //public float getPlayfieldValue(Playfield p, bool own)
        //{
        //    Player mPlayer, ePlayer;
        //    if (own)
        //    {
        //        mPlayer = p.playerFirst;
        //        ePlayer = p.playerSecond;
        //    }
        //    else {
        //        mPlayer = p.playerSecond;
        //        ePlayer = p.playerFirst;
        //    }

        //    //if (p.value >= -2000000) return p.value;
        //    int retval = 0;
        //    int hpboarder = 10;
        //    if (mPlayer.ownHeroName == HeroEnum.warlock && p.enemyHeroName != HeroEnum.mage) hpboarder = 6;
        //    int aggroboarder = 11;

        //    retval -= p.evaluatePenality;
        //    retval += p.owncards.Count * 5;

        //    retval += p.ownMaxMana;
        //    retval -= p.enemyMaxMana;

        //    retval += p.ownMaxMana * 20 - p.enemyMaxMana * 20;

        //    if (p.enemyHeroName == HeroEnum.mage || p.enemyHeroName == HeroEnum.druid) retval -= 2 * p.enemyspellpower;

        //    if (p.ownHero.Hp + p.ownHero.armor > hpboarder)
        //    {
        //        retval += p.ownHero.Hp + p.ownHero.armor;
        //    }
        //    else
        //    {
        //        retval -= 2 * (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor) * (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor);
        //    }


        //    if (p.enemyHero.Hp + p.enemyHero.armor > aggroboarder)
        //    {
        //        retval += -p.enemyHero.Hp - p.enemyHero.armor;
        //    }
        //    else
        //    {
        //        retval += 3 * (aggroboarder + 1 - p.enemyHero.Hp - p.enemyHero.armor);
        //    }

        //    if (p.ownWeaponAttack >= 1)
        //    {
        //        retval += p.ownWeaponAttack * p.ownWeaponDurability;
        //    }

        //    if (!p.enemyHero.frozen)
        //    {
        //        retval -= p.enemyWeaponDurability * p.enemyWeaponAttack;
        //    }
        //    else
        //    {
        //        if (p.enemyWeaponDurability >= 1)
        //        {
        //            retval += 12;
        //        }
        //    }

        //    //RR card draw value depending on the turn and distance to lethal
        //    //RR if lethal is close, carddraw value is increased
        //    if (sf.Ai.lethalMissing <= 5) //RR
        //    {
        //        retval += p.owncarddraw * 100;
        //    }
        //    if (p.ownMaxMana < 4)
        //    {
        //        retval += p.owncarddraw * 2;
        //    }
        //    else
        //    {
        //        retval += p.owncarddraw * 5;
        //    }

        //    //retval += p.owncarddraw * 5;
        //    retval -= p.enemycarddraw * 15;

        //    //int owntaunt = 0;
        //    int readycount = 0;
        //    int ownMinionsCount = 0;
        //    foreach (Minion m in p.ownMinions)
        //    {
        //        retval += 5;
        //        retval += m.Hp * 2;
        //        retval += m.Angr * 2;
        //        retval += m.handcard.card.rarity;
        //        if (!m.playedThisTurn && m.windfury) retval += m.Angr;
        //        if (m.divineshild) retval += 1;
        //        if (m.stealth) retval += 1;
        //        if (m.handcard.card.isSpecialMinion)
        //        {
        //            retval += 1;
        //            if (!m.taunt && m.stealth) retval += 20;
        //        }
        //        else
        //        {
        //            if (m.Angr <= 2 && m.Hp <= 2 && !m.divineshild) retval -= 5;
        //        }
        //        //if (!m.taunt && m.stealth && penman.specialMinions.ContainsKey(m.name)) retval += 20;
        //        //if (m.poisonous) retval += 1;
        //        if (m.divineshild && m.taunt) retval += 4;
        //        //if (m.taunt && m.handcard.card.name == CardDB.cardName.frog) owntaunt++;
        //        //if (m.handcard.card.isToken && m.Angr <= 2 && m.Hp <= 2) retval -= 5;
        //        //if (!penman.specialMinions.ContainsKey(m.name) && m.Angr <= 2 && m.Hp <= 2) retval -= 5;
        //        if (m.handcard.card.name == CardDB.cardName.direwolfalpha || m.handcard.card.name == CardDB.cardName.flametonguetotem || m.handcard.card.name == CardDB.cardName.stormwindchampion || m.handcard.card.name == CardDB.cardName.raidleader) retval += 10;
        //        if (m.handcard.card.name == CardDB.cardName.bloodmagethalnos) retval += 10;
        //        if (m.handcard.card.name == CardDB.cardName.nerubianegg)
        //        {
        //            if (m.Angr >= 1) retval += 2;
        //            if ((!m.taunt && m.Angr == 0) && (m.divineshild || m.maxHp > 2)) retval -= 10;
        //        }
        //        if (m.Ready) readycount++;
        //        if (m.Hp <= 4 && (m.Angr > 2 || m.Hp > 3)) ownMinionsCount++;
        //    }

        //    /*if (p.enemyMinions.Count >= 0)
        //    {
        //        int anz = p.enemyMinions.Count;
        //        if (owntaunt == 0) retval -= 10 * anz;
        //        retval += owntaunt * 10 - 11 * anz;
        //    }*/


        //    bool useAbili = false;
        //    bool usecoin = false;
        //    foreach (Action a in p.playactions)
        //    {
        //        if (a.actionType == actionEnum.attackWithHero && p.enemyHero.Hp <= p.attackFaceHP) retval++;
        //        if (a.actionType == actionEnum.useHeroPower) useAbili = true;
        //        if (p.ownHeroName == HeroEnum.warrior && a.actionType == actionEnum.attackWithHero && useAbili) retval -= 1;
        //        //if (a.actionType == actionEnum.useHeroPower && a.card.card.name == CardDB.cardName.lesserheal && (!a.target.own)) retval -= 5;
        //        if (a.actionType != actionEnum.playcard) continue;
        //        if ((a.card.card.name == CardDB.cardName.thecoin || a.card.card.name == CardDB.cardName.innervate)) usecoin = true;
        //        //save spell for all classes: (except for rouge if he has no combo)
        //        if (a.target == null) continue;
        //        if (p.ownHeroName != HeroEnum.thief && a.card.card.type == CardDB.cardtype.SPELL && (!a.target.own && a.target.isHero) && a.card.card.name != CardDB.cardName.shieldblock) retval -= 11;
        //        if (p.ownHeroName == HeroEnum.thief && a.card.card.type == CardDB.cardtype.SPELL && (a.target.isHero && !a.target.own)) retval -= 11;
        //    }
        //    if (usecoin && useAbili && p.ownMaxMana <= 2) retval -= 40;
        //    if (usecoin) retval -= 5 * p.manaTurnEnd;
        //    if (p.manaTurnEnd >= 2 && !useAbili)
        //    {
        //        retval -= 10;
        //        if (p.ownHeroName == HeroEnum.thief && (p.ownWeaponDurability >= 2 || p.ownWeaponAttack >= 2)) retval += 10;
        //    }
        //    //if (usecoin && p.mana >= 1) retval -= 20;

        //    int mobsInHand = 0;
        //    int bigMobsInHand = 0;
        //    foreach (Handmanager.Handcard hc in p.owncards)
        //    {
        //        if (hc.card.type == CardDB.cardtype.MOB)
        //        {
        //            mobsInHand++;
        //            if (hc.card.Attack >= 3) bigMobsInHand++;
        //        }
        //    }

        //    if (ownMinionsCount - p.enemyMinions.Count >= 4 && bigMobsInHand >= 1)
        //    {
        //        retval += bigMobsInHand * 25;
        //    }


        //    //bool hasTank = false;
        //    foreach (Minion m in p.enemyMinions)
        //    {
        //        retval -= this.getEnemyMinionValue(m, p);
        //        //hasTank = hasTank || m.taunt;
        //    }

        //    /*foreach (SecretItem si in p.enemySecretList)
        //    {
        //        if (readycount >= 1 && !hasTank && si.canbeTriggeredWithAttackingHero)
        //        {
        //            retval -= 100;
        //        }
        //        if (readycount >= 1 && p.enemyMinions.Count >= 1 && si.canbeTriggeredWithAttackingMinion)
        //        {
        //            retval -= 100;
        //        }
        //        if (si.canbeTriggeredWithPlayingMinion && mobsInHand >= 1)
        //        {
        //            retval -= 25;
        //        }
        //    }*/

        //    retval -= p.enemySecretCount;
        //    retval -= p.lostDamage;//damage which was to high (like killing a 2/1 with an 3/3 -> => lostdamage =2
        //    retval -= p.lostWeaponDamage;

        //    //if (p.ownMinions.Count == 0) retval -= 20;
        //    //if (p.enemyMinions.Count == 0) retval += 20;
        //    if (p.enemyHero.Hp <= 0) retval = 10000;
        //    //soulfire etc
        //    int deletecardsAtLast = 0;
        //    foreach (Action a in p.playactions)
        //    {
        //        if (a.actionType != actionEnum.playcard) continue;
        //        if (a.card.card.name == CardDB.cardName.soulfire || a.card.card.name == CardDB.cardName.doomguard || a.card.card.name == CardDB.cardName.succubus) deletecardsAtLast = 1;
        //        if (deletecardsAtLast == 1 && !(a.card.card.name == CardDB.cardName.soulfire || a.card.card.name == CardDB.cardName.doomguard || a.card.card.name == CardDB.cardName.succubus)) retval -= 20;
        //    }
        //    if (p.enemyHero.Hp >= 1 && p.guessingHeroHP <= 0)
        //    {
        //        if (p.turnCounter < 2) retval += p.owncarddraw * 100;
        //        retval -= 1000;
        //    }
        //    if (p.ownHero.Hp <= 0) retval = -10000;

        //    p.value = retval;
        //    return retval;
        //}

        //public int getEnemyMinionValue(Minion m, Playfield p)
        //{
        //    int retval = 5;
        //    retval += m.Hp * 2;
        //    if (!m.frozen && !((m.name == CardDB.cardName.ancientwatcher || m.name == CardDB.cardName.ragnarosthefirelord) && !m.silenced))
        //    {
        //        retval += m.Angr * 2;
        //        if (m.windfury) retval += m.Angr * 2;
        //        if (m.Angr >= 4) retval += 10;
        //        if (m.Angr >= 7) retval += 50;
        //    }

        //    if (m.Angr == 0) retval -= 7;

        //    retval += m.handcard.card.rarity;
        //    if (m.taunt) retval += 5;
        //    if (m.divineshild) retval += m.Angr;
        //    if (m.divineshild && m.taunt) retval += 5;
        //    if (m.stealth) retval += 1;

        //    if (m.poisonous) retval += 4;

        //    if (m.handcard.card.targetPriority >= 1 && !m.silenced)
        //    {
        //        retval += m.handcard.card.targetPriority;
        //    }
        //    if (m.name == CardDB.cardName.nerubianegg && m.Angr <= 3 && !m.taunt) retval = 0;
        //    return retval;
        //}

        public MCTSPlayer(int side, Playfield playfield)
        {
            this.playerSide = side;
            this.board = new Playfield(playfield);
        }

        public override void updateState(Playfield playfield)
        {
            this.board = new Playfield(playfield);
        }

        public override Action getMove()
        {

            currentState = new Node(board, null, 0);

            if (expand(currentState) == 1) return null; // no moves

            int counter = 0;

            for (int i = 0; i < 10000; i++)
            {
                //Helpfunctions.Instance.logg("try: " + i);
                //counter++;
                //if (counter == 10) {
                //    Helpfunctions.Instance.logg("try: " + i);
                //    counter = 0;
                //}
                //if (i == 752) {
                //    int debug = 1;
                //}
                UCTRun(currentState, 0.7f);
                currentState.state = new Playfield(board);
            }

            int maxVisit = 0;
            Action selectedMove = null;
            Node selectedChild = null;
            foreach (Node child in currentState.children)
            {

                if (child.numVisited > maxVisit)
                {
                    maxVisit = child.numVisited;
                    selectedMove = child.move;
                    selectedChild = child;
                }
            }

            //Helpfunctions.Instance.logg("Turn of child:" + selectedChild.state.isOwnTurn);
            //currentState.printChildren();

            return selectedMove;
        }

        public Playfield getBestPlayfield()
        {

            currentState = new Node(board, null, 0);

            if (expand(currentState) == 1) return new Playfield(board); // no moves

            int counter = 0;

            for (int i = 0; i < 10000; i++)
            {
                Helpfunctions.Instance.logg("try: " + i);
                //counter++;
                //if (counter == 10) {
                //    Helpfunctions.Instance.logg("try: " + i);
                //    counter = 0;
                //}
                //if (i == 752) {
                //    int debug = 1;
                //}
                UCTRun(currentState, 0.7f);
                currentState.state = new Playfield(board);
            }

            int maxVisit = 0;
            Playfield selectedMove = null;
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

            //Helpfunctions.Instance.logg("Turn of child:" + selectedChild.state.isOwnTurn);
            //currentState.printChildren();

            return selectedMove;
        }

        //public void getAllpossibleStates(Playfield state, ref List<Playfield> statesList)
        //{
        //    List<Action> moves = Movegenerator.Instance.getMoveList(state, false, true, true);
        //    if (moves.Count == 0)
        //    {
        //        if (tt.addToMap(state) == false)
        //        {
        //            statesList.Add(state);
        //        }
        //        return;
        //    }
        //    foreach (Action action in moves)
        //    {
        //        Playfield afterState = new Playfield(state);
        //        afterState.doAction(action);
        //        //if (tt.addToMap(afterState) == false)
        //        //{
        //        //    statesList.Add(afterState);
        //        getAllpossibleStates(afterState, ref statesList);
        //        //}
        //    }
        //}

        public void UCTRun(Node p, float c)
        {

            List<Node> visited = new List<Node>();
            visited.Add(p);

            int depth = 0;
            Node parent = null;

            List<Action> actions = new List<Action>();

            while (!isLeaf(p))
            {
                parent = p;
                p = select(p, c);
                actions.Add(p.move);
                depth++;
                visited.Add(p);
            }

            int count = expand(p);

            int score = sample(p);

            //update score
            foreach (Node visitedPos in visited)
            {
                float lastMean = visitedPos.mean;
                visitedPos.mean = (score + lastMean * visitedPos.numVisited) / (visitedPos.numVisited + 1);
                visitedPos.numVisited++;
            }
        }

        public int sample(Node p)
        {
            Playfield startState = new Playfield(p.state);
            Action move = null;
            int turn = 0;

            int score = startState.getGameResult();
            while (score == -1)
            {
                //List<Action> moves = Movegenerator.Instance.getMoveList(startState, false, false, true);
                //if (move != null)
                //{
                //GameManager.Instance.moveCount++;
                //if (GameManager.Instance.moveCount == 562)
                //{
                //    int debug = 1;
                //}
                    //var milliseconds = (DateTime.Now - DateTime.MinValue).TotalMilliseconds;
                    Movegenerator.Instance.getMoveListForPlayfield(startState, false);
                    //double time = (DateTime.Now - DateTime.MinValue).TotalMilliseconds - milliseconds;
                    //GameManager.Instance.myTimer += time;
                    //Helpfunctions.Instance.logg("my:" + time + " total:" + GameManager.Instance.myTimer);


                    //milliseconds = (DateTime.Now - DateTime.MinValue).TotalMilliseconds;
                    //List<Action> bruteForceMoves = Movegenerator.Instance.getMoveList(startState, false, true, true);
                    //time = (DateTime.Now - DateTime.MinValue).TotalMilliseconds - milliseconds;
                    //GameManager.Instance.sfTimer += time;
                    //Helpfunctions.Instance.logg("sf:" + time + " total:" + GameManager.Instance.sfTimer);

                    //if (bruteForceMoves.Count != startState.moveList.Count) {
                    //    startState.printBoard();
                    //    int debug = 1;
                    //    Helpfunctions.Instance.logg("BF Move List:------------------------------------");
                    //    foreach (Action action in bruteForceMoves)
                    //    {
                    //        action.print();
                    //    }
                    //    startState.printMoveList();
                    //}
                //}
                //Helpfunctions.Instance.logg("Count: " + startState.moveList.Count);
                if (startState.moveList.Count == 0)
                {
                    startState.endTurn(false, false);
                    turn++;
                    move = null;
                }
                else
                {
                    move = startState.moveList[GameManager.getRNG().Next(startState.moveList.Count)];
                    startState.doAction(move);
                }
                score = startState.getGameResult();
            }

            if (playerSide == score)
            {
                return 1;
            }
            return 0;
        }

        public int expand(Node p)
        {

            int state = 0;
            Playfield afterState = new Playfield(p.state);
            //List<Action> moves = Movegenerator.Instance.getMoveList(afterState, false, true, true);
            //GameManager.Instance.moveCount++;
            //if (GameManager.Instance.moveCount == 16)
            //{
            //    int debug = 1;
            //}
            Movegenerator.Instance.getMoveListForPlayfield(afterState, false);

            if (afterState.moveList.Count == 0)
            { // no moves available
                afterState.endTurn(false, false);
                Node originalNode = new Node(new Playfield(afterState), null, p.depth + 1);
                p.children.Add(originalNode);
                state = 1;
            }

            foreach (Action move in afterState.moveList)
            {
                Node afterNode = new Node(new Playfield(afterState), move, p.depth + 1);
                afterNode.state.doAction(move);
                if (move.actionType == actionEnum.attackWithMinion && move.own.entitiyID == 1008 && move.target.entitiyID == 1003)
                {
                    GameManager.Instance.moveCount++;
                    if (GameManager.Instance.moveCount == 3)
                    {
                        int debug = 1;
                    }
                }
                p.children.Add(afterNode);
            }

            return state;
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
            GameManager.Instance.moveCount++;
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
                Movegenerator.Instance.getMoveListForPlayfield(afterState, false);
                
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

            float bestValue = 0;
            Node selected = null;
            bool ownTurn = (playerSide == 0) ? true : false;

            if (p.state.isOwnTurn == ownTurn)
            {

                foreach (Node child in p.children)
                {

                    if (child.numVisited == 0)
                    {
                        return child;
                    }

                    float UCTValue = (float)(child.mean + c * Math.Sqrt(Math.Log(p.numVisited) / child.numVisited));

                    //child.move.print();
                    //Helpfunctions.Instance.logg("UCTVale = " + UCTValue);

                    if (UCTValue > bestValue)
                    {
                        bestValue = UCTValue;
                        selected = child;
                    }

                }
            }
            else
            {

                bestValue = 99999;

                foreach (Node child in p.children)
                {

                    if (child.numVisited == 0)
                    {
                        return child;
                    }

                    float UCTValue = (float)(child.mean - c * Math.Sqrt(Math.Log(p.numVisited) / child.numVisited));


                    if (UCTValue < bestValue)
                    {
                        bestValue = UCTValue;
                        selected = child;
                    }

                }

            }

            if (selected == null)
            {
                selected = p.children[GameManager.getRNG().Next(p.children.Count)];
            }

            return selected;

        }

    }
}
