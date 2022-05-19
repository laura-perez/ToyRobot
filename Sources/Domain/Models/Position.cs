using ToyRobot.Domain.Models.Enums;

namespace ToyRobot.Domain.Models
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public DirectionFacing? Facing { get; set; }
    }
}
