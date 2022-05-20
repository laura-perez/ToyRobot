using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyRobot.Domain.Interfaces.Handlers;
using ToyRobot.Domain.Interfaces.Parsers;
using ToyRobot.Domain.Models;
using ToyRobot.Domain.Models.Enums;

namespace ToyRobot.ConsoleApplication
{
    public class ToyRobotSimulator
    {
        private Robot robot;
        private Tabletop tabletop; //TODO: put table size in configuration
        private ICommandParser _commandparser;
        private IToyRobotHandler _toyRobotHandler;

        public ToyRobotSimulator(Robot robot, Tabletop tabletop, ICommandParser commandParser, IToyRobotHandler toyRobotHandler)
        {
            this.robot = robot;
            this.tabletop = tabletop;
            _commandparser = commandParser;
            _toyRobotHandler = toyRobotHandler;
        }

        public string ExecuteCommand(string input)
        {
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("Please enter a command.");
                }

                var command = _commandparser.Parse(input);

                //First PLACE command
                if (robot.Position is null && command.CommandType == CommandType.PLACE)
                {
                    robot = _toyRobotHandler.PlaceRobot(command.Position);
                    return "Robot is now on the table with coords (" + robot.Position.X + "," + robot.Position.Y + ")" + " and facing " + robot.Position.Facing.ToString();
                }
                //Subsequent commands
                else if (robot != null && robot.Position != null)
                {
                    switch (command.CommandType)
                    {
                        case CommandType.PLACE:

                            robot = _toyRobotHandler.PlaceRobot(command.Position);

                            return "Robot is now on the table with coords (" + robot.Position.X + "," + robot.Position.Y + ")";
                        case CommandType.MOVE:

                            robot = _toyRobotHandler.MoveRobot();

                            return "Robot went for a walk to position (" + robot.Position.X + "," + robot.Position.Y;
                        case CommandType.LEFT:

                            robot = _toyRobotHandler.TurnRobot(CommandType.LEFT);
                            return "Robot is now facing " + robot.Position.Facing.ToString();
                        case CommandType.RIGHT:

                            robot = _toyRobotHandler.TurnRobot(CommandType.RIGHT);
                            return "Robot is now facing " + robot.Position.Facing.ToString();
                        case CommandType.REPORT:

                            string toReturn = Environment.NewLine;

                            toReturn += @"          Position of Robot:";
                            toReturn += Environment.NewLine;
                            toReturn += DrawPositionASCII(robot.Position.X, robot.Position.Y);
                            toReturn += Environment.NewLine;
                            toReturn += "ROBOT is on the table at position (" + robot.Position.X + "," + robot.Position.Y + ") and is facing " + robot.Position.Facing?.ToString() + "";

                            return toReturn;
                    }
                }
                else
                {
                    throw new NullReferenceException("Please start with a command PLACE");
                }

                //This should never happen!
                throw new Exception("falling into the darkness!");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return ex.ParamName;
            }
            catch (ArgumentNullException ex)
            {
                return "ROBOT says: " + ex.ParamName;
            }
            catch (ArgumentException ex)
            {
                return invalidCommandAscii();
            }
            catch (NullReferenceException ex)
            {
                return "ROBOT is not yet on the table: " + ex.Message;
            }
            catch (Exception ex)
            {
                if (ex is IndexOutOfRangeException || ex is FormatException)
                {
                    return "ROBOT says: " + ex.Message;
                }
                else
                {
                    return "Unmanaged exception: " + ex.Message;
                }
            }
        }

        /// <summary>
        /// Draw "PLEASE ENTER A VALID COMMAND"
        /// </summary>
        /// <returns>the string output</returns>
        static string invalidCommandAscii()
        {
            return @"
 ___________________________________
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
            if (x < -1 || x > 5 || y < -1 || y > 5) return "Index Error"; // Check it's not out of range
            var b = new string(' ', 15) + "******";
            var d = new[] { b, b, b, b, b, b };                           // Generate display box, and fill with the default character
            d[y] = new string(' ', 15) + new string('*', x) + 'o' + new string('*', 5 - x);     // Replace the array's entry in y coordinate with a new string containing the new character
            return string.Join("\r\n", d.Reverse());
        }
    }
}
