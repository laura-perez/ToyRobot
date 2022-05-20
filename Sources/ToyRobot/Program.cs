using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ToyRobot.Domain.Interfaces.Parsers;
using ToyRobot.Domain.Models;
using ToyRobot.Infrastructure.Parsers;
using ToyRobot.Domain.Models.Enums;
using ToyRobot.Domain.Interfaces.Handlers;
using ToyRobot.Domain.Handlers;

namespace ToyRobot.ConsoleApplication
{
    public class Program
    {
        static void Main(string[] args)
        {
            //TODO: move data persistence to repository
            Robot robot = new Robot();
            Tabletop tabletop = new Tabletop(6, 6); //TODO: put table size in configuration

            //init -very basic- Dependency Injection
            ICommandParser _commandParser = new CommandParser();
            IToyRobotHandler _toyRobotHandler = new ToyRobotHandler(robot, tabletop);

            //Initialization of the game - waiting for PLACE command only
            Console.WriteLine(InitGameAscii());

            while (true) //infinite game
            {
                try
                {
                    Console.WriteLine("Your command is my command:");
                    var input = Console.ReadLine();

                    if (input == null)
                    {
                        throw new ArgumentException("Please enter a command.");
                    }

                    var command = _commandParser.Parse(input);

                    //First PLACE command
                    if (robot.Position is null && command.CommandType == CommandType.PLACE)
                    {
                        robot = _toyRobotHandler.PlaceRobot(command.Position);
                        Console.WriteLine(@"Robot is now on the table with coords (" + robot.Position.X + "," + robot.Position.Y + ")" + " and facing " + robot.Position.Facing.ToString());
                    }
                    //Subsequent commands
                    else if (robot != null && robot.Position != null)
                    {
                        switch (command.CommandType)
                        {
                            case CommandType.PLACE:

                                robot = _toyRobotHandler.PlaceRobot(command.Position);

                                Console.WriteLine("Robot is now on the table with coords (" + robot.Position.X + "," + robot.Position.Y + ")");

                                break;
                            case CommandType.MOVE:

                                robot = _toyRobotHandler.MoveRobot();

                                Console.WriteLine("Robot went for a walk to position (" + robot.Position.X + "," + robot.Position.Y + ")");

                                break;
                            case CommandType.LEFT:

                                robot = _toyRobotHandler.TurnRobot(CommandType.LEFT);
                                Console.WriteLine("Robot is now facing " + robot.Position.Facing.ToString());
                                break;
                            case CommandType.RIGHT:
                                robot = _toyRobotHandler.TurnRobot(CommandType.RIGHT);
                                break;
                            case CommandType.REPORT:
                                Console.WriteLine(DrawPositionASCII(robot.Position.X, robot.Position.Y));

                                Console.WriteLine("ROBOT is on the table at position (" + robot.Position.X + "," + robot.Position.Y + ") and is facing " + robot.Position.Facing?.ToString() + "");
                                break;
                        }
                    }
                    else 
                    {
                        throw new NullReferenceException("Please start with a command PLACE");
                    }
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine("ROBOT says: " + ex.Message);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(invalidCommandAscii());
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine("ROBOT is not yet on the table: " + ex.Message);
                }
                catch (Exception ex)
                {
                    if (ex is IndexOutOfRangeException || ex is FormatException)
                    {
                        Console.WriteLine("ROBOT says: " + ex.Message);
                    }
                    else
                    {
                        Console.WriteLine("Unmanaged exception: " + ex.Message);
                    }
                }
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

        /// <summary>
        /// Draw "PLEASE ENTER A VALID COMMAND"
        /// </summary>
        /// <returns>the string output</returns>
        static string invalidCommandAscii()
        {
            return @"
              ____________________________________
|.=================================.|
||  PLEASE ENTER A VALID COMMAND.  ||
||  PLEASE ENTER A VALID COMMAND.  ||
||  PLEASE ENTER A VALID COMMAND.  ||
||  PLEASE ENTER A,                ||
||              /                  ||
||     [____]  /\                  ||
||     ]    [ / /                  ||
||   ___\__/_/ /                   ||
||__|__|    |_/____________________||
.|===|_|_/\_|======================||
     | | __ | 
     |_|[::]|  
     \_|_||_| 
       |_||_|    
      _|_||_|_ 
     |___||___| 
            ";
        }

        /// <summary>
        /// Draw a grid of the size 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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
