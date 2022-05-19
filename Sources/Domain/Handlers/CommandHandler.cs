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
    public class CommandHandler : ICommandHandler
    {
        public Robot PlaceRobot(int x, int y, DirectionFacing? facing)
        {
            throw new NotImplementedException();
        }

    }
}
