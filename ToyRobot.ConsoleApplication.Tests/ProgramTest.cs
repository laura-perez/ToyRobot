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
        public void EmptyCommandReturnsInvalidMessage()
        {
            //Arrange
            string input = "";
            _mockcommandParser.Setup(x => x.Parse(input))
                                .Throws(new ArgumentException("Please enter a valid command."));

            _simulator = new ToyRobotSimulator(new Robot(), new Tabletop { Width = 6, Height = 6 }, _mockcommandParser.Object, _mocktoyRobotHandler.Object);

            //Act
            var result = _simulator.ExecuteCommand("");

            //Assert
            result.Should().NotBeNull();
            result.Should().Contain("PLEASE ENTER A VALID COMMAND");
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
                                    CommandType = CommandType.MOVE
                                });

            _simulator = new ToyRobotSimulator(new Robot(), new Tabletop { Width = 6, Height = 6 }, _mockcommandParser.Object, _mocktoyRobotHandler.Object);

            //Act
            var result = _simulator.ExecuteCommand(input);

            //Assert
            result.Should().NotBeNull();
            result.Should().Contain("ROBOT is not yet on the table: Please start with a command PLACE", result);
        }

        #region First time command PLACE

        [Fact]
        public void FirstCommandPlaceWithoutParametersReturnsInvalidMessage()
        {
            //Arrange
            var input = "PLACE";

            _mockcommandParser.Setup(x => x.Parse(input))
                                .Throws(new IndexOutOfRangeException("Command PLACE requires X Y and FACING (NORTH, EAST, SOUTH, WEST) parameters"));

            _simulator = new ToyRobotSimulator(new Robot(), new Tabletop { Width = 6, Height = 6 }, _mockcommandParser.Object, _mocktoyRobotHandler.Object);

            //Act
            var result = _simulator.ExecuteCommand(input);

            //Assert
            result.Should().NotBeNull();
            result.Should().Contain("ROBOT says: Command PLACE requires X Y and FACING (NORTH, EAST, SOUTH, WEST) parameters", result);
        }

        [Fact]
        public void FirstCommandPlaceWithoutFacingParametersReturnsInvalidMessage()
        {
            //Arrange
            var input = "PLACE 1 2";
            var mockCommand = new Command() { 
                                CommandType = CommandType.PLACE,
                                Position = new Position()
                                {
                                    X = 1,
                                    Y = 2
                                }
            };

            _mockcommandParser.Setup(x => x.Parse(input))
                                .Returns(mockCommand);

            _mocktoyRobotHandler.Setup(x => x.PlaceRobot(mockCommand.Position))
                                .Throws(new ArgumentNullException("Please specify a facing direction for the first command place (NORTH EAST SOUTH WEST)."));

            _simulator = new ToyRobotSimulator(new Robot(), new Tabletop { Width = 6, Height = 6 }, _mockcommandParser.Object, _mocktoyRobotHandler.Object);

            //Act
            var result = _simulator.ExecuteCommand(input);

            //Assert
            result.Should().NotBeNull();
            result.Should().Contain("ROBOT says: Please specify a facing direction for the first command place (NORTH EAST SOUTH WEST).", result);
        }

        [Fact]
        public void FirstSuccessfulCommandPlaceReturnsPositionMessage()
        {
            _simulator = new ToyRobotSimulator(new Robot(), new Tabletop { Width = 6, Height = 6 }, _mockcommandParser.Object, _mocktoyRobotHandler.Object);

            var result = _simulator.ExecuteCommand("PLACE 0 0 NORTH");

            Assert.Equal("Robot is now on the table with coords (0,0) and facing NORTH", result);
        }

        #endregion

        #region COMMAND PLACE

        [Fact]
        public void SubsequentSuccessfulCommandPlaceWithoutDirectionParameterReturnsPositionMessage()
        {
            //Arrange
            var mockRobot = new Robot()
            {
                Position = new Position()
                {
                    Facing = DirectionFacing.NORTH,
                    X = 0,
                    Y = 0
                }
            };

            var newPosition = new Position()
            {
                X = 2,
                Y = 4
            };

            string mockCommand = "PLACE 2 4";

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

            _mockcommandParser.Setup(x => x.Parse(mockCommand))
                                .Returns(new Command
                                {
                                    CommandType = CommandType.PLACE,
                                    Position = newPosition
                                });
            
            _simulator = new ToyRobotSimulator(mockRobot, new Tabletop { Width = 6, Height = 6 }, _mockcommandParser.Object, _mocktoyRobotHandler.Object);

            var result = _simulator.ExecuteCommand(mockCommand);

            Assert.Equal("Robot is now on the table with coords (2,4) and facing NORTH", result);
        }

        [Fact]
        public void CommandPlaceOutOfXBoundariesReturnsBoundariesMessage()
        {
            _simulator = new ToyRobotSimulator(new Robot(), new Tabletop { Width = 6, Height = 6 }, _mockcommandParser.Object, _mocktoyRobotHandler.Object);

            var result = _simulator.ExecuteCommand("PLACE 0 6 NORTH");

            Assert.Equal("Please place Robot within the table boundaries.", result);
        }

        [Fact]
        public void CommandPlaceOutOfYBoundariesReturnsBoundariesMessage()
        {
            _simulator = new ToyRobotSimulator(new Robot(), new Tabletop { Width = 6, Height = 6 }, _mockcommandParser.Object, _mocktoyRobotHandler.Object);

            var result = _simulator.ExecuteCommand("PLACE 6 0 NORTH");

            Assert.Equal("Please place Robot within the table boundaries.", result);
        }
        #endregion
    }
}