using ToyRobot.Domain.Interfaces.Handlers;
using ToyRobot.Domain.Models;
using ToyRobot.Domain.Models.Enums;

namespace ToyRobot.Domain.Handlers
{
    public class ToyRobotHandler : IToyRobotHandler
    {
        private Robot _robot { get; set; }
        private Tabletop _tabletop { get; set; }

        /// <summary>
        /// </summary>
        public ToyRobotHandler(Robot robot, Tabletop tabletop) 
        {
            _robot = robot;
            _tabletop = tabletop;
        }

        /// <summary>
        /// PLACE the robot on the table
        /// </summary>
        /// <param name="position">the position the robot should be placed at</param>
        /// <returns>the robot</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="Exception"></exception>
        public Robot PlaceRobot(Position position)
        {
            if (!IsPositionValid(position))
            {
                throw new ArgumentOutOfRangeException("Please place Robot within the table boundaries.");
            }

            if (position.Facing is null)
            {
                //First PLACE command:
                if (_robot.Position is null)
                {
                    throw new ArgumentNullException("Please specify a facing direction for the first command place (NORTH EAST SOUTH WEST).");
                }

                //if the direction facing is not specified, keep the current direction facing.
                position.Facing = _robot.Position.Facing;
            }

            _robot.Position = position;
            return _robot;
        }

        /// <summary>
        /// moves the robot to one step in the direct it's already facing
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Robot MoveRobot()
        {
            //copy by value
            var newPosition = new Position()
            {
                X = _robot.Position.X,
                Y = _robot.Position.Y,
                Facing = _robot.Position.Facing
            };

            switch (_robot.Position.Facing)
            {
                case DirectionFacing.North:
                    newPosition.Y++;
                    break;
                case DirectionFacing.South:
                    newPosition.Y--;
                    break;
                case DirectionFacing.East:
                    newPosition.X++;
                    break;
                case DirectionFacing.West:
                    newPosition.X--;
                    break;
            }

            if (!IsPositionValid(newPosition))
            {
                throw new ArgumentOutOfRangeException("Robot is about to fall off the table. Please change facing direction or enter a new command PLACE");
            }
            else
            {
                _robot.Position = newPosition;
            }
            return _robot;
        }

        /// <summary>
        /// rotates the robot of 90 degrees in the direction specifies by the command (LEFT or RIGHT)
        /// </summary>
        /// <param name="toDirection"></param>
        /// <returns></returns>
        public Robot TurnRobot(CommandType toDirection)
        {
            _robot.Position.Facing = Rotate(_robot.Position.Facing.Value, toDirection);

            return _robot;
        }

        /// <summary>
        /// Checks is position is within the table boundaries
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool IsPositionValid(Position position)
        {
            if (0 <= position.X && position.X < _tabletop.Width)
            {
                if (0 <= position.Y && position.Y < _tabletop.Height)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// This methods does a 90 degrees rotation
        /// </summary>
        /// <param name="directionFacing">the initial facing direction of the robot</param>
        /// <param name="commandType">LEFT or RIGHT</param>
        /// <returns></returns>
        private DirectionFacing Rotate(DirectionFacing directionFacing, CommandType commandType)
        {
            var degrees = (int)directionFacing;

            if (commandType is CommandType.Left)
            {
                if (degrees == 0) degrees = 360;
                return (DirectionFacing)(degrees - 90);
            }
            else
            {
                if (degrees == 270) degrees = -90;
                return (DirectionFacing)(degrees + 90);
            }
        }
    }
}
