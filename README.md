
# Toy Robot Puzzle

Toy Robot Puzzle is a .NET Core console application written in C#.

# Getting Started
To install and run this application you will need Visual Studio with .NET Core 6.0

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

Make sure to restore the Nuget packages for each projects

Build the solution and run the project ToyRobot.ConsoleApplication

### To publish a .exe file:

In Visual Studio, right click on the project ToyRobot.ConsoleApplication and select "Publish.."

Select Folder, Folder then Finish

On the publish screen Click on "More action" -> Settings to open the publish profile settings.

Set the deployment mode to "Self-contained" and target to your targeted environment. Save and publish.

## Usage

It lets you place a toy robot on a 6x6 table and make it move and rotate.

Commands available:
PLACE X, Y, DIRECTION
MOVE
LEFT
RIGHT
REPORT

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
