using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyRobot.Domain.Models;
using ToyRobot.Domain.Models.Enums;

namespace ToyRobot.Domain.Interfaces.Handlers
{
    public interface ICommandHandler
    {
        public Robot PlaceRobot(int x, int y, DirectionFacing? facing); 
    }
}
