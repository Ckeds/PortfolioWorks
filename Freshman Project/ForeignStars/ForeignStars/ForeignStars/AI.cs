using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Collections.PriorityQueue;

namespace ForeignStars
{
    class AI : Player
    {

        #region attributes
        /// <summary>
        /// Will build these lists in the constructor based on other
        /// parameters (namely, players) passed in.
        /// </summary>
        List<Unit> friendlyUnits; //Likely unimplemented in build, for future reference.
        List<Unit> enemyUnits;
        int currentUnitIndex;
        bool animating;
        bool hasAttacked;
        bool turnStart;

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Allies"></param>
        /// <param name="Enemies"></param>
        /// <param name="OwnArmy"></param>
        public AI(List<Army> Allies, List<Army> Enemies, Army OwnArmy)
        {
            turnStart = true;
            friendlyUnits = new List<Unit>();

            foreach (Army ally in Allies)
            {
                foreach (Unit u in ally.Units)
                {
                    friendlyUnits.Add(u);
                }
            }

            enemyUnits = new List<Unit>();

            foreach (Army enemy in Enemies)
            {
                foreach (Unit u in enemy.Units)
                {
                    enemyUnits.Add(u);
                }
            }

            ownArmy = OwnArmy;

            currentUnitIndex = 0;
            currentUnit = ownArmy.Units[0];
            animating = false;
            hasAttacked = false;
            turnStart = true;

        }
        #endregion

        #region Methods
        public void MoveCalculation()
        {
            currentTile = ownArmy.Units[currentUnitIndex].Tile;
            selectedTile = otherCalculator();
        }

        private Tile otherCalculator()
        {
            List<Tile> potentialTiles = new List<Tile>();
            List<int> finaltileWeights = new List<int>();
            List<Stack<PathNode>> towardsEnemies = new List<Stack<PathNode>>();
            List<Tile> aggressiveTiles = new List<Tile>();

            foreach (Unit enemy in enemyUnits)
            {
                towardsEnemies.Add(AStar(ownArmy.Units[currentUnitIndex].Tile, enemy.Tile));
            }

            foreach (Stack<PathNode> path in towardsEnemies)
            {
                for (int x = 0; x < ownArmy.Units[currentUnitIndex].Movement + 1; x++)
                {
                    if (path.Count != 0)
                    {
                        PathNode tmpNode = path.Pop();
                        Tile tmpTile = tiles[tmpNode.Current.X, tmpNode.Current.Y];

                        if (!aggressiveTiles.Contains(tmpTile))
                        { aggressiveTiles.Add(tmpTile); }
                    }
                }
            }

            validateMove(currentUnit.Tile.GridwiseLocation.X, currentUnit.Tile.GridwiseLocation.Y, currentUnit.Movement + 1);

            foreach (Tile t in tiles)
            {
                if (t.HighlightSpace && !t.HasUnit())
                {
                    potentialTiles.Add(t);
                }
            }

            int index = 0;
            int weight;
            while (index < potentialTiles.Count)
            {
                weight = EnemiesNear(potentialTiles[index], true) + AlliesNear(potentialTiles[index])
                         + CanAttackFrom(potentialTiles[index]);

                if (aggressiveTiles.Contains(potentialTiles[index]))
                {
                    weight += 3;
                }
                finaltileWeights.Add(weight);
                index++;
            }

            index = 0;
            int finalIndex = 0;
            int bestTileWeight = 0;

            for (; index < finaltileWeights.Count; index++)
            {
                if (finaltileWeights[index] > bestTileWeight)
                {
                    finalIndex = index;
                    bestTileWeight = finaltileWeights[index];
                }
                else if (finaltileWeights[index] == bestTileWeight)
                {
                    Random rand = new Random();
                    int random = rand.Next(0, 2);

                    if (random == 0)
                    {
                        finalIndex = index;
                    }
                }
            }
            resetHighlight();
            if (potentialTiles.Count <= finalIndex)
            {
                return null;
            }
            selectedTile = potentialTiles[finalIndex];
            return potentialTiles[finalIndex];
        }

        public override void SetTiles(Tile[,] tileArray)
        {
            tiles = tileArray;
        }

        protected override void AfterAnimate()
        {
            currentUnit.Tile.Unit = null;
            selectedTile.Unit = currentUnit;

            animating = false;
            selectedTile.Unit = currentUnit;
            currentTile.Unit = null;
            beatenPath = null;

        }

        public override void ChangeTurnConditions()
        {
            currentUnitIndex = 0;
        }

        #endregion

        #region Supporting Methods

        private int InvalidTile(Tile movetile, Unit movingUnit)
        {
            if (movetile.HasUnit())
            {
                return -10000;
            }
            if (movetile.Collision > movingUnit.Collision)
            {
                return -10000;
            }
            return 0;

        }

        /// <summary>
        /// Calculates number of nearby enemy units, 'nearby' being defined as
        /// able to move to the tile. the threatening bool is included as a way
        /// to differentiate between just nearby (for enemy units able to support enemy units)
        /// or able to fire on that square (for enemy units nearby AI units)
        /// </summary>
        /// <param name="movetile">Tile being weighted</param>
        /// <param name="threatening">Whether or not ability to attack the tile is the focus, true being yes</param>
        /// <returns></returns>
        private int EnemiesNear(Tile movetile, bool threatening)
        {
            int x = 0;

            foreach (Unit enemy in friendlyUnits)
            {
                if (threatening)
                {
                    if (enemy.MaxMovement + enemy.Range >= calculateDistance(enemy.Tile, movetile))
                    { x++; }
                }
                else
                {
                    if (enemy.MaxMovement >= calculateDistance(enemy.Tile, movetile))
                    { x++; }
                }
            }


            return x;
        }

        /// <summary>
        /// Calculates number of friendly units (controlled or uncontrolled) nearby.
        /// 'Nearby' defined as 'can move to this tile in one turn'.
        /// </summary>
        /// <param name="movetile">Tile to be judged/weighted</param>
        /// <returns>Number of friendly (to AI) units able to support tile.</returns>
        private int AlliesNear(Tile movetile)
        {
            int x = 0;

            foreach (Unit friend in friendlyUnits)
            {
                if (friend.MaxMovement >= calculateDistance(friend.Tile, movetile))
                { x++; }
            }
            foreach (Unit own in ownArmy.Units)
            {
                if (own.Movement >= calculateDistance(own.Tile, movetile))
                { x++; }
            }

            return (x / 2);
        }



        /// <summary>
        /// Updates all units in a battle with their tiles, because the AI really needs
        /// to know about them this way.
        /// </summary>
        public void UpdateAllUnitTiles()
        {
            int index = 0;

            while (index < ownArmy.Units.Count)
            {
                Unit tmp = ownArmy.Units[index];
                tmp.Tile = tiles[tmp.UnitBoxX / GV.TileSize, tmp.UnitBoxY / GV.TileSize];
                index++;
            }

            index = 0;

            while (index < enemyUnits.Count)
            {
                Unit tmp = enemyUnits[index];
                tmp.Tile = tiles[tmp.UnitBoxX / GV.TileSize, tmp.UnitBoxY / GV.TileSize];
                index++;
            }

            index = 0;

            while (index < friendlyUnits.Count)
            {
                Unit tmp = friendlyUnits[index];
                tmp.Tile = tiles[tmp.UnitBoxX / GV.TileSize, tmp.UnitBoxY / GV.TileSize];
                index++;
            }
        }


        /// <summary>
        /// Calculates the 'dumb' distance between one tile and another, 
        /// </summary>
        /// <param name="start">One tile, doesn't matter which</param>
        /// <param name="end">Other tile, doesn't matter which</param>
        /// <returns>'dumb' distance, in terms of change in x + change in y on grid.</returns>
        private int calculateDistance(Tile start, Tile end)
        {
            int xDistance = 0;
            int yDistance = 0;

            if (start.GridwiseLocation.X >= end.GridwiseLocation.X)
            {
                xDistance = start.GridwiseLocation.X - end.GridwiseLocation.X;
            }
            else
            {
                xDistance = end.GridwiseLocation.X - start.GridwiseLocation.X;
            }

            if (start.GridwiseLocation.Y >= end.GridwiseLocation.Y)
            {
                yDistance = start.GridwiseLocation.Y - end.GridwiseLocation.Y;
            }
            else
            {
                yDistance = end.GridwiseLocation.Y - start.GridwiseLocation.Y;
            }


            return (xDistance + yDistance);

        }

        /// <summary>
        /// Checks whether a given unit can attack any enemies from a given tile.
        /// </summary>
        /// <param name="attack">The tile to launch the attack from</param>
        /// <param name="attacker">The unit that wants to know if it can attack</param>
        /// <returns>5 for 'true', 0 for 'no', modifier for weighting tiles.</returns>
        private int CanAttackFrom(Tile attack)
        {
            validateAttack(attack.GridwiseLocation.X, attack.GridwiseLocation.Y, currentUnit.Range);

            foreach (Unit enemy in enemyUnits)
            {
                if (enemy.Tile.HighlightSpace)
                {
                    resetHighlight();
                    return 5;
                }
            }

            return 0;
        }

        private Unit pickEnemy()
        {
            validateAttack(currentUnit.Tile.GridwiseLocation.X, currentUnit.Tile.GridwiseLocation.Y, currentUnit.Range);
            List<Unit> validShots = new List<Unit>();

            foreach (Unit enemy in enemyUnits)
            {
                if (enemy.Tile.HighlightSpace)
                {
                    validShots.Add(enemy);
                }
            }

            resetHighlight();
            int indexofTarget = 0;
            int currentHighestStrength = 0;

            for (int x = 0; x < validShots.Count; x++)
            {
                if ((validShots[x].Health + validShots[x].Attack + validShots[x].Defense) > currentHighestStrength)
                {
                    currentHighestStrength = (validShots[x].Health + validShots[x].Attack + validShots[x].Defense);
                    indexofTarget = x;
                }
                else if ((validShots[x].Health + validShots[x].Attack + validShots[x].Defense) == currentHighestStrength)
                {
                    Random ran = new Random();

                    int random = ran.Next(0, 2);

                    if (random == 1)
                    {
                        indexofTarget = x;
                    }
                }
            }
            if (validShots.Count <= indexofTarget)
            {
                return null;
            }
            return validShots[indexofTarget];
        }

        private void validateAttack(int x, int y, int m)
        {
            bool canHighlight = false;
            if (tiles[x, y].Unit == null)
            {
                tiles[x, y].HighlightSpace = true;
                canHighlight = true;
            }
            else if (tiles[x, y].Unit != null)
            {
                if (currentUnit.Tile.GridwiseLocation.X == x)
                {
                    tiles[x, y].HighlightSpace = true;
                    canHighlight = true;
                }
                else if (enemyUnits.Contains(tiles[x, y].Unit))
                {
                    tiles[x, y].HighlightSpace = true;
                    canHighlight = false;
                }
            }

            if (tiles[x, y].Collision > currentUnit.Collision)
            {
                tiles[x, y].HighlightSpace = false;
                canHighlight = false;
            }
            if (m > 0)
            {
                if (canHighlight)
                {
                    if (x > 0)
                        validateAttack(x - 1, y, m - 1);
                    if (y > 0)
                        validateAttack(x, y - 1, m - 1);
                    if (x < Math.Sqrt(tiles.Length) - 1)
                        validateAttack(x + 1, y, m - 1);
                    if (y < Math.Sqrt(tiles.Length) - 1)
                        validateAttack(x, y + 1, m - 1);

                }
            }
        }
        private void validateMove(int x, int y, int m)
        {
            bool canHighlight = false;
            if (tiles[x, y].Unit == null)
            {
                tiles[x, y].HighlightSpace = true;
                canHighlight = true;
            }
            else if (tiles[x, y].Unit != null)
            {
                if (tiles[x, y].Unit.Army == currentUnit.Army)
                {
                    tiles[x, y].HighlightSpace = true;
                    canHighlight = true;
                }
                else
                {
                    canHighlight = false;
                }
            }

            if (tiles[x, y].Collision > currentUnit.Collision)
            {
                tiles[x, y].HighlightSpace = false;
                canHighlight = false;
            }
            if (m > 0)
            {
                if (canHighlight)
                {
                    if (x > 0)
                        validateAttack(x - 1, y, m - 1);
                    if (y > 0)
                        validateAttack(x, y - 1, m - 1);
                    if (x < Math.Sqrt(tiles.Length) - 1)
                        validateAttack(x + 1, y, m - 1);
                    if (y < Math.Sqrt(tiles.Length) - 1)
                        validateAttack(x, y + 1, m - 1);

                }
            }
        }

        private void resetHighlight()
        {
            foreach (Tile t in tiles)
            {
                t.HighlightSpace = false;
                t.HighlightEnemy = false;
            }
        }


        protected virtual Stack<PathNode> AStar(Tile source, Tile destination)
        {
            List<Tile> closed = new List<Tile>();
            List<Tile> toDetermine;
            List<Tile> toAdd = new List<Tile>();
            Stack<PathNode> finalPath = null;
            MinPriorityQueue<PathNode> open = new MinPriorityQueue<PathNode>();
            List<PathNode> openList = new List<PathNode>();
            open.Enqueue(new PathNode(source, destination));
            openList.Add(open.Peek());
            PathNode currentNode = null;
            bool endReached = false;
            do
            {
                if(closed.Count > 400)
                {
                    Console.WriteLine("Dafuq? :" + closed.Count);
                }
                currentNode = open.Dequeue();
                openList.Remove(currentNode);
                if (currentNode.HCost == 0)
                {
                    endReached = true;
                    finalPath = currentNode.GetPath();
                }
                else
                {
                    closed.Add(tiles[currentNode.Current.X, currentNode.Current.Y]);
                    toDetermine = getAdjacent(currentNode);
                    foreach (Tile t in toDetermine)
                    {
                        bool remove = false;
                        if (t.Collision > source.Unit.Collision)
                            remove = true;
                        if (t.HasUnit())

                            if (closed.Contains(t))
                                remove = true;
                        //Add this if I want to have no duplicate pathnodes (currently, 
                        //multiple exist with different source nodes
                        /*
                        PathNode temp = new PathNode(t.GridwiseLocation, currentNode, destination.GridwiseLocation);
                        foreach (PathNode p in openList)
                        {
                            if (p.Current == temp.Current)
                            {
                                if (p.GCost > temp.GCost)
                                {
                                    p.Source = temp.Source;

                                    remove = true;
                                }
                            }

                        }'*/

                        if (!remove)
                            toAdd.Add(t);
                    }

                    foreach (Tile t in toAdd)
                    {
                        PathNode temp = new PathNode(t.GridwiseLocation, currentNode, destination.GridwiseLocation);
                        open.Enqueue(temp);
                    }
                    toAdd.Clear();
                }
            } while (!endReached);

            return finalPath;
        }



        #endregion

        #region GameMethods

        public override bool Update(Camera2D camera)
        {
            if (ownArmy.Units.Count > 0)
            {
                if (currentUnitIndex == 0 && turnStart)
                {
               

                    currentUnit = ownArmy.Units[currentUnitIndex];
                    MoveCalculation();
                    turnStart = false;
                    if (selectedTile != null)
                    {
                        animating = true;
                    }
                    hasAttacked = false;
                    turnStart = false;
                }

                if (!animating && hasAttacked)
                {
                    currentUnitIndex++;
                    if (currentUnitIndex >= ownArmy.Units.Count)
                    {

                        currentUnitIndex = 0;
                        currentTile = ownArmy.Units[0].Tile;
                        selectedTile = null;
                        animating = false;
                        turnStart = true;
                        hasAttacked = false;
                        turnStart = true;
                        ownArmy.Reset();
                        return true;
                    }
                    else
                    {
                        currentUnit = ownArmy.Units[currentUnitIndex];
                        hasAttacked = false;
                        MoveCalculation();
                        if (selectedTile != null)
                        {
                            animating = true;
                        }
                    }
                }
                else if (animating)
                {
                    if (!currentTile.HasUnit())
                    {
                        currentUnit.Tile.Unit = currentUnit;
                        currentTile = currentUnit.Tile;
                    }
                    AnimateMovement();
                }
                else if (!hasAttacked)//added &&!animating -zack
                {
                    Unit enemy = pickEnemy();
                    if (enemy != null)
                    {
                        int damageDealt = DamageCalculator.doDamage(currentUnit, enemy, false);
                        enemy.Health -= damageDealt;
                        //dealt = damageDealt;
                        damageDealt = DamageCalculator.doDamage(currentUnit, enemy, true);
                        currentUnit.Health -= damageDealt;
                        //received = damageDealt;
                        if (enemy.Health <= 0)
                        {
                            enemyUnits.Remove(enemy);
                        }
                    }

                    hasAttacked = true;

                }
                return false;

            }
                return true;
            
        }

        #endregion
    }
}
