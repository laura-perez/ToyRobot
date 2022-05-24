using FluentAssertions;
using ToyRobot.Domain.Models;
using ToyRobot.Domain.Models.Enums;
using ToyRobot.Infrastructure.Parsers;
using Xunit;

namespace ToyRobot.Infrastructure.Tests
{
    public class CommandParserTest
    {
        private CommandParser _commandParser;

        public CommandParserTest()
        {
            _commandParser = new CommandParser();
        }

        [Fact]
        public void ValidPlaceInputReturnsObjectCommand()
        {
            //Arrange
            string inputTest = "PLACE 1 2 NORTH";

            Command expectedResult = new Command() {
                                CommandType = CommandType.Place,
                                Position = new Position() { 
                                X = 1,
                                Y = 2,
                                Facing = DirectionFacing.North
            }};

            //Act
            var result = _commandParser.Parse(inputTest);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Command));
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void ValidMoveInputReturnsObjectCommand()
        {
            //Arrange
            string inputTest = "MOVE";

            //Act
            var result = _commandParser.Parse(inputTest);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Command));
            result.CommandType.Should().Be(CommandType.Move);
        }

        [Fact]
        public void InvalidInputThrowsArgumentException()
        {
            //Arrange
            string inputTest = "blah";

            //Act & Assert
            var result = _commandParser.Invoking(x => x.Parse(inputTest))
                                        .Should().Throw<ArgumentException>("Please enter a valid command.");
        }
    }
}