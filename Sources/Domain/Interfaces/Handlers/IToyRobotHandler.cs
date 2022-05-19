using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyRobot.Domain.Models;
using ToyRobot.Domain.Models.Enums;

namespace ToyRobot.Domain.Interfaces.Handlers
{
    public interface IToyRobotHandler
    {

        public Robot PlaceRobot(Position position);

        public Robot MoveRobot(Position position);

        public Robot TurnRobot(CommandType toDirection);
    }
}
