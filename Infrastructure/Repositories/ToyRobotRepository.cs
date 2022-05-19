using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyRobot.Domain.Interfaces.Repository;
using ToyRobot.Domain.Models;

namespace ToyRobot.Infrastructure.Repositories
{
    public class ToyRobotRepository : IToyRobotRepository
    {
        public Robot GetRobot()
        {
            throw new NotImplementedException();
        }
    }
}
