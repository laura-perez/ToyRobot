using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyRobot.Domain.Interfaces.Handlers;
using ToyRobot.Domain.Models;
using ToyRobot.Domain.Models.Enums;

namespace ToyRobot.Domain.Handlers
{
    public class ToyRobotHandler : IToyRobotHandler
    {
        private Robot _robot { get; set; }
        private Tabletop _tabletop { get; set; }

        public ToyRobotHandler(Tabletop tabletop) 
        {
            _tabletop = tabletop;
        }

        public Robot PlaceRobot(Position position)
        {
            if (!isPositionValid(position))
            {
                throw new ArgumentOutOfRangeException("The position is out of table range.");
            }

            //if the direction facing is not specified, keep the current direction facing.
            if (position.Facing is null)
            {
                position.Facing = _robot.Position.Facing;
            }

            _robot.Position = position;
            return _robot;
        }

        public Robot MoveRobot(Position position)
        {
            throw new NotImplementedException();
        }

        public Robot TurnRobot(CommandType toDirection)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks is position is within the table boundaries
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool isPositionValid(Position position)
        {
            if(0 <= position.X && position.X <= _tabletop.Width)
                return true;
            return false;
        }

    }
}
