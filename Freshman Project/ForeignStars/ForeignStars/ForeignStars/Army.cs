using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForeignStars
{
    /// <summary>
    /// Location where we store units in an army for the player
    /// 
    /// Team Dynasty
    /// @author: William Powell
    /// </summary>
    public class Army
    {
        //Attributes
        private int team;
        private int armyNumber;
        private List<Unit> units;

        public int Team
        {
            get { return team; }
            set { team = value; }
        }

        public int ArmyNumber
        {
            get { return armyNumber; }
            set { armyNumber = value; }
        }

        public List<Unit> Units
        {
            get { return units; }
        }

        /// <summary>
        ///
        /// </summary>
        /// 
        public Army(List<Unit> units, int team, int armyNumber)
        {
            this.team = team;
            this.units = units;
            this.armyNumber = armyNumber;
            foreach (Unit u in this.units)
                u.Army = this;
        }
        public Army()
            : this(new List<Unit>(), 0, 0)
        { }
        public void Reset()
        {
            foreach (Unit u in units)
            {
                u.Movement = u.MaxMovement;
                u.HasAttacked = false;
            }
        }
        public override string ToString()
        {
            return "" + team;
        }
    }
}
