using FluentAssertions;
using Moq;
using ToyRobot.Domain.Interfaces.Handlers;
using ToyRobot.Domain.Interfaces.Parsers;
using ToyRobot.Domain.Models;
using ToyRobot.Domain.Models.Enums;
using Xunit;

namespace ToyRobot.ConsoleApplication.Tests
{
    public class ProgramTest
    {
        private ToyRobotSimulator _simulator;

        private readonly Mock<IToyRobotHandler> _mocktoyRobotHandler;
        private readonly Mock<ICommandParser> _mockcommandParser;

        public ProgramTest()
        {
            _mocktoyRobotHandler = new Mock<IToyRobotHandler>();
            _mockcommandParser = new Mock<ICommandParser>();
        }

        [Fact]
        public void InvalidCommandReturnsInvalidMessage()
        {
            //Arrange
            string input = "INVALIDCOMMAND";
            _mockcommandParser.Setup(x => x.Parse(input))
                                .Throws(new ArgumentException("Please enter a valid command."));

            _simulator = new ToyRobotSimulator(new Robot(), new Tabletop { Width = 6, Height = 6 }, _mockcommandParser.Object, _mocktoyRobotHandler.Object);

            var result = _simulator.ExecuteCommand(input);

            //Assert
            result.Should().NotBeNull();
            result.Should().Contain("PLEASE ENTER A VALID COMMAND");
        }

        [Fact]
        public void FirstCommandIsNotPlaceReturnsInvalidMessage()
        {
            //Arrange
            string input = "MOVE";
            _mockcommandParser.Setup(x => x.Parse(input))
                                .Returns(new Command { 
                                    CommandType = CommandType.Move
                                });

            _simulator = new ToyRobotSimulator(new Robot(), new Tabletop { Width = 6, Height = 6 }, _mockcommandParser.Object, _mocktoyRobotHandler.Object);

            //Act
            var result = _simulator.ExecuteCommand(input);

            //Assert
            result.Should().NotBeNull();
            result.Should().Contain("ROBOT is not yet on the table: Please start with a command PLACE");
        }

        #region COMMAND PLACE
        [Fact]
        public void FirstSuccessfulCommandPlaceReturnsPositionMessage()
        {
            //Arrange
            var input = "PLACE 1 2";
            var position = new Position()
            {
                X = 1,
                Y = 2,
                Facing = DirectionFacing.North
            };
            var mockCommand = new Command()
            {
                CommandType = CommandType.Place,
                Position = position
            };
            var robot = new Robot() { Position = position };

            _mockcommandParser.Setup(x => x.Parse(input))
                                .Returns(mockCommand);

            _mocktoyRobotHandler.Setup(x => x.PlaceRobot(mockCommand.Position))
                                .Returns(robot);

            _simulator = new ToyRobotSimulator(new Robot(), new Tabletop { Width = 6, Height = 6 }, _mockcommandParser.Object, _mocktoyRobotHandler.Object);

            //Act
            var result = _simulator.ExecuteCommand(input);

            //Assert
            result.Should().NotBeNull();
            result.Should().Contain("Robot is now on the table with coords (1,2) and facing North");
        }

        [Fact]
        public void SubsequentSuccessfulCommandPlaceWithoutDirectionParameterReturnsPositionMessage()
        {
            //Arrange
            string input = "PLACE 2 4";
            var position = new Position()
            {
                X = 1,
                Y = 2,
                Facing = DirectionFacing.North
            };
            var newPosition = new Position()
            {
                X = 2,
                Y = 4
            };
            var mockCommand = new Command()
            {
                CommandType = CommandType.Place,
                Position = newPosition
            };
            var mockRobot = new Robot() { Position = position };

            _mockcommandParser.Setup(x => x.Parse(input))
                                .Returns(mockCommand);

            _mocktoyRobotHandler.Setup(x => x.PlaceRobot(newPosition))
                                .Returns(new Robot 
                                { 
                                    Position = new Position()
                                    { 
                                        X = newPosition.X,
                                        Y = newPosition.Y,
                                        Facing = mockRobot.Position.Facing
                                    }
                                });
            
            _simulator = new ToyRobotSimulator(mockRobot, new Tabletop { Width = 6, Height = 6 }, _mockcommandParser.Object, _mocktoyRobotHandler.Object);

            //Act
            var result = _simulator.ExecuteCommand(input);

            //Assert
            result.Should().NotBeNull();
            result.Should().Contain("ROBOT is on the table at position (2,4) and is facing North");
        }

        [Fact]
        public void CommandPlaceOutOfBoundariesReturnsBoundariesMessage()
        {
            //Arrange
            string input = "PLACE 0 6";
            var position = new Position()
            {
                X = 0,
                Y = 0,
                Facing = DirectionFacing.North
            };
            var mockCommand = new Command()
            {
                CommandType = CommandType.Place,
                Position = new Position()
                {
                    X = 0,
                    Y = 6
                }
            };
            var mockRobot = new Robot() { Position = position };

            _mockcommandParser.Setup(x => x.Parse(input))
                                .Returns(mockCommand);

            _mocktoyRobotHandler.Setup(x => x.PlaceRobot(mockCommand.Position))
                                .Throws(new ArgumentOutOfRangeException("Please place Robot within the table boundaries."));

            _simulator = new ToyRobotSimulator(mockRobot, new Tabletop { Width = 6, Height = 6 }, _mockcommandParser.Object, _mocktoyRobotHandler.Object);

            var result = _simulator.ExecuteCommand(input);

            //Assert
            result.Should().NotBeNull();
            result.Should().Contain("Please place Robot within the table boundaries.");
        }


        #endregion
    }
}