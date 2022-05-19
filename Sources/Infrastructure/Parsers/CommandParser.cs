﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyRobot.Domain.Interfaces.Parsers;
using ToyRobot.Domain.Models;
using ToyRobot.Domain.Models.Enums;

namespace ToyRobot.Infrastructure.Parsers
{
    public class CommandParser : ICommandParser
    {
        /// <summary>
        /// This method tries to parse a console input string into a Command object
        /// </summary>
        /// <param name="inputArgs">the input string</param>
        /// <param name="command">the command object</param>
        /// <returns>a boolean if succeeded</returns>
        public Command Parse(string input)
        {
            Command command = new Command();
            var inputArgs = input.Split(' ');

            try
            {
                //parsing the first argument which should be of type CommandType
                command.CommandType = Enum.Parse<CommandType>(inputArgs[0].ToUpper());

                if (command.CommandType is CommandType.PLACE)
                {
                    command.Position = new Position();
                    command.Position.X = int.Parse(inputArgs[1]);
                    command.Position.Y = int.Parse(inputArgs[2]);

                    ////Position Facing Parsing - we don't want to throw and exception since this is an optional parameter
                    DirectionFacing direction;

                    if (!String.IsNullOrEmpty(inputArgs[3]) && Enum.TryParse(inputArgs[3].ToUpper(), out direction))
                    {
                        command.Position.Facing = direction;
                    }
                }

                return command;
            }
            //TODO: Parser specific Exception
            catch (Exception)
            {
                throw new Exception("Please enter a valid command!");
            }
        }
    }
}
