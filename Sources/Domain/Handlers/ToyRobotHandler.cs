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

        public ToyRobotHandler(Robot robot, Tabletop tabletop) 
        {
            _robot = robot;
            _tabletop = tabletop;
        }

        public Robot PlaceRobot(Position position)
        {
            if (!isPositionValid(position))
            {
                throw new ArgumentOutOfRangeException("Please place Robot within the table boundaries.");
            }

            if (position.Facing is null)
            {
                //First PLACE command:
                if (_robot.Position is null)
                {
                    throw new Exception("Please specify a facing direction for the first command place");
                }

                //if the direction facing is not specified, keep the current direction facing.
                position.Facing = _robot.Position.Facing;
            }

            _robot.Position = position;
            return _robot;
        }

        public Robot MoveRobot()
        {
            Position newPosition = _robot.Position;

            switch (_robot.Position.Facing)
            {
                case DirectionFacing.NORTH:
                    newPosition.Y++;
                    break;
                case DirectionFacing.SOUTH:
                    newPosition.Y--;
                    break;
                case DirectionFacing.EAST:
                    newPosition.X++;
                    break;
                case DirectionFacing.WEST:
                    newPosition.X--;
                    break;
            }

            if (!isPositionValid(newPosition))
            {
                throw new ArgumentOutOfRangeException("Robot is about to fall off the table. Please change facing direction or enter a new command PLACE");
            }

            _robot.Position = newPosition;
            return _robot;
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
            if (0 <= position.X && position.X <= _tabletop.Width)
            {
                if (0 <= position.Y && position.Y <= _tabletop.Height)
                    return true;
            }
            return false;
        }

    }
}
