using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyRobot.Domain.Models;

namespace ToyRobot.Domain.Interfaces.Parsers
{
    public interface ICommandParser
    {
        public bool TryParse(string input, out Command command);
    }
}
