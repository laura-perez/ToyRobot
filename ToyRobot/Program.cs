using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ToyRobot.Domain.Interfaces.Parsers;
using ToyRobot.Domain.Models;
using ToyRobot.Infrastructure.Parsers;
using ToyRobot.Domain.Models.Enums;

namespace ToyRobot.ConsoleApplication
{
    public class Program
    {
        static void Main(string[] args)
        {
            //init -very basic- Dependency Injection
            ICommandParser _commandParser = new CommandParser();

            //Initialization of the game - waiting for PLACE command only
            Console.WriteLine(InitGameAscii());
            var input = Console.ReadLine();

            Command command;
            _commandParser.TryParse(input, out command);

            while (command.CommandType is not CommandType.PLACE)
            { 
                
            }




            while (_commandParser.TryParse(input, out command))
            {
                Console.WriteLine("This command is not Valid. Please start the game with the command PLACE");
                input = Console.ReadLine();
            }

        }
        static string InitGameAscii()
        {
            return @" 
                         [____]
                         ]()()[
                       ___\__/_
                      |__|    |__|
                       |_|_/\_|_| 
                       | | __ | |
                       |_|[::]|_|   
                       \_|_||_|_/ 
                         |_||_|    
                        _|_||_|_ 
                       |___||___| 

                   Hi! I am Toy Robot!

*********
* RULES *
*********

You are playing on a 6x6 square tabletop

COMMANDS:
* PLACE will put the toy robot on the table in position X,Y and facing NORTH, SOUTH, EAST or WEST
* MOVE will move the toy robot one unit forward in the direction it is currently facing
* LEFT will rotate the robot 90 degrees left
* RIGHT will rotate the robot 90 degrees right
* REPORT will announce the X,Y and orientation of the robot


To get started, put the robot on the table:";
        }

        static string DrawPositionASCII(int x, int y)
        {
            if (x < 1 || x > 6 || y < 1 || y > 6) return "Index Error"; // Check it's not out of range
            var b = "******";
            var d = new[] { b, b, b, b, b };                           // Generate display box, and fill with the default character
            x--;                                                        // Convert the X to a 0 based index
            d[y - 1] = new string('*', x) + 'o' + new string('*', 5 - x);     // Replace the array's entry in y coordinate with a new string containing the new character
            return string.Join("\r\n", d.Reverse());
        }
    }
}
