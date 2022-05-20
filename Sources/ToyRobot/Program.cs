using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ToyRobot.Domain.Handlers;
using ToyRobot.Domain.Interfaces.Handlers;
using ToyRobot.Domain.Interfaces.Parsers;
using ToyRobot.Domain.Models;
using ToyRobot.Domain.Models.Enums;
using ToyRobot.Infrastructure.Parsers;

namespace ToyRobot.ConsoleApplication
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Declare entities
            Robot robot = new Robot();
            Tabletop tabletop = new Tabletop();

            //Setup configuration builder
            IConfiguration Config = new ConfigurationBuilder()
                .AddJsonFile("Config/appSettings.json")
                .Build();

            //Get configuration values
            Config.GetSection("TableTop").Bind(tabletop);

            //setup Dependency Injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IToyRobotHandler, ToyRobotHandler>(s => new ToyRobotHandler(robot, tabletop))
                .AddSingleton<ICommandParser, CommandParser>()
                .BuildServiceProvider();

            var _commandParser = serviceProvider.GetService<ICommandParser>();
            var _toyRobotHandler = serviceProvider.GetService<IToyRobotHandler>();

            ToyRobotSimulator simulator = new ToyRobotSimulator(robot, tabletop, _commandParser, _toyRobotHandler);

            //Initialization of the game - waiting for PLACE command only
            Console.WriteLine(InitGameAscii());

            while (true) //infinite game
            {
                Console.WriteLine();
                Console.WriteLine("Your command is my command:");

                var input = Console.ReadLine();
                var output = simulator.ExecuteCommand(input);

                Console.WriteLine();
                Console.WriteLine(output);
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
* PLACE will put the toy robot on the table in position X Y FACING (NORTH, SOUTH, EAST or WEST)
* MOVE will move the toy robot one unit forward in the direction it is currently facing
* LEFT will rotate the robot 90 degrees left
* RIGHT will rotate the robot 90 degrees right
* REPORT will announce the X,Y and orientation of the robot


To get started, put the robot on the table:";
        }

    }
}
