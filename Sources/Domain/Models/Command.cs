using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyRobot.Domain.Models.Enums;

namespace ToyRobot.Domain.Models
{
    public class Command
    {
        public CommandType CommandType { get; set; }

        public Position Position { get; set; }
    }
}
