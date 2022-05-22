
# Toy Robot Puzzle

Toy Robot Puzzle is a .NET Core console application written in C#.

This application allows for a simulation of a toy robot moving on a 6 x 6 square tabletop

# Getting Started
To install and run this application you will need .NET Core 6.0.

This project is using the following nugget packages: 
 * Microsoft.Extensions.Configuration
 * Microsoft.Extensions.DependencyInjection

To run the tests, you will need the following Nuget packages:
* xUnit
* Moq
* FluentAssertions

## Installation

Clone this repository into a new empty folder

Open the ToyRobot.sln file in Visual Studio

Make sure to restore the Nuget packages for each project

Build the solution and run the project ToyRobot.ConsoleApplication

### To publish a .exe file:

In Visual Studio, right click on the project ToyRobot.ConsoleApplication and select "Publish.."

Select Folder, Folder then Finish

On the publish screen Click on "More action" -> Settings to open the publish profile settings.

Set the deployment mode to "Self-contained" 
Target to your targeted environment
Tick "Produce single file" 
Save and publish.

## Solution Requirements

- This application allows for a simulation of a toy robot moving on a 6 x 6 square tabletop.
- There are no obstructions on the table surface.
- The robot is free to roam around the surface of the table, but must be prevented from falling to destruction. Any movement that would result in this must be prevented, however further valid movement commands must still be allowed.
- PLACE will put the toy robot on the table in position X,Y and facing NORTH, SOUTH, EAST or WEST. (0,0) can be considered as the SOUTH WEST corner and (5,5) as the NORTH EAST corner.
- The first valid command to the robot is a PLACE command. After that, any sequence of commands may be issued, in any order, including another PLACE command. The library should discard all commands in the sequence until a valid PLACE command has been executed.
- The PLACE command should be discarded if it places the robot outside of the table surface.
- Once the robot is on the table, subsequent PLACE commands could leave out the direction and only provide the coordinates. When this happens, the robot moves to the new coordinates without changing the direction.
- MOVE will move the toy robot one unit forward in the direction it is currently facing.
- LEFT and RIGHT will rotate the robot 90 degrees in the specified direction without changing the position of the robot.
- REPORT will announce the X,Y and orientation of the robot.

## Usage

This application allows for a simulation of a toy robot moving on a 6 x 6 square tabletop

You can change the size of the table in the config file Config/appSettings.json

Commands available:
PLACE X, Y, DIRECTION | Place the robot on the table facing a direction NORTH SOUTH EAST WEST
MOVE                  | Move the toy robot one unit forward in the direction it is currently facing
LEFT                  | Will rotate the robot 90 degrees LEFT
RIGHT                 | Will rotate the robot 90 degrees RIGHT
REPORT                | Displays the position and facing direction of the toy robot

### Example Input and Output
Example 1 Input
> PLACE 0,0,NORTH
> MOVE
> REPORT
Output: 0,1,NORTH

Example 2 Input
> PLACE 0,0,NORTH
> LEFT
> REPORT
Output: 0,0,WEST

Example 3 Input
> PLACE 1,2,EAST
> MOVE
> MOVE
> LEFT
> MOVE
> REPORT
Output: 3,3,NORTH

Example 4 Input
> PLACE 1,2,EAST
> MOVE
> LEFT
> MOVE
> PLACE 3,1
> MOVE
> REPORT
Output: 3,2,NORTH
