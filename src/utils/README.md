### Utils

The utils folder contains classes used by all different parts of the compiler for utility porpuses

## Position

The [Position](Position.cs) class is used so that errors can have the correct information regarding the start and end locations. Because of this, throughout the whole compilation we need to keep track of the positions of each logical unit.

## Error

[Errors](Error.cs) can be runntime or compile time and can happen at any point in the compilation process. We need to ensure that we are keepging track of as much as possible to give the user as much feedback on the type of issue which we are encountering.

