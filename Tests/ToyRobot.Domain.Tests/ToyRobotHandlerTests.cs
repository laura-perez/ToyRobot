using FluentAssertions;
using Moq;
using ToyRobot.Domain.Handlers;
using ToyRobot.Domain.Interfaces.Parsers;
using ToyRobot.Domain.Models;
using ToyRobot.Domain.Models.Enums;
using Xunit;

namespace ToyRobot.Domain.Tests
{
    public class ToyRobotHandlerTests
    {
        private ToyRobotHandler _toyRobotHandler;
        private Tabletop _tabletop;

        public ToyRobotHandlerTests()
        {
            _tabletop = new Tabletop() { Width = 6, Height = 6 };
        }

        [Fact]
        public void SuccessfulPlaceRobotReturnsRobot()
        {
            var currentPosition = new Position()
            {
                X = 0,
                Y = 0,
                Facing = DirectionFacing.North
            };
            var newPosition = new Position()
            {
                X = 3,
                Y = 3,
                Facing = DirectionFacing.South
            };

            _toyRobotHandler = new ToyRobotHandler(new Robot() { Position = currentPosition }, _tabletop);

            //Act
            var result = _toyRobotHandler.PlaceRobot(newPosition);

            //Assert
            result.Should().NotBeNull();
            result.Position.Should().NotBeNull();
            result.Position.X.Should().Be(3);
            result.Position.Y.Should().Be(3);
        }

        [Fact]
        public void SuccessfulMoveRobotReturnsRobot()
        {
            var currentPosition = new Position()
            {
                X = 0,
                Y = 0,
                Facing = DirectionFacing.North
            };

            _toyRobotHandler = new ToyRobotHandler(new Robot() { Position = currentPosition }, _tabletop);

            //Act
            var result = _toyRobotHandler.MoveRobot();

            //Assert
            result.Should().NotBeNull();
            result.Position.Should().NotBeNull();
            result.Position.X.Should().Be(0);
            result.Position.Y.Should().Be(1);
        }

        [Fact]
        public void MoveCommandOutOfBoundariesThrowsException()
        {
            var currentPosition = new Position()
            {
                X = 0,
                Y = 4,
                Facing = DirectionFacing.West
            };

            _toyRobotHandler = new ToyRobotHandler(new Robot() { Position = currentPosition }, _tabletop);

            //Act && Assert
            var result = _toyRobotHandler.Invoking(x => x.MoveRobot()).Should()
                                                .Throw<ArgumentOutOfRangeException>();
        }
    }
}