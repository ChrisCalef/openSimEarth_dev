# openSimEarth Dev User - Game Directory Files

*** WARNING - DO NOT ATTEMPT TO RUN openSimEarth.exe HERE, YOU MUST COPY THESE FILES TO A T3D FULL TEMPLATE PROJECT! ***

This repository is for the use of Torque3D users and developers, who would like to merge openSimEarth into their own projects. 

If you simply want to use openSimEarth, you can download the latest zipped version here: 

  http://openSimEarth.com/downloads/openSimEarth.zip


INSTALLATION:

If you have just created a new Torque game project, then you should be able to simply copy all of the files in this repository (minus .git and .gitignore, of course) into your project's game directory.

With an existing Torque project, however, you may have to do a little manual script merging, if you have modified any of the following files:

art/datablocks/datablockExec.cs  -  exec physicsShape.cs

scripts/main.cs          - start SQL and begin openSimEarth ticking.

scripts/client/
	default.bind.cs  - a handful of new binds
	prefs.cs         - block of new prefs needs to be added to your prefs.cs
	serverConnection.cs  - one line - added gravity argument to physicsInitWorld

scripts/server/
	BadBehavior/*    - some new behavior trees.
	game.cs          - physicsInitWorld, and sql.
	init.cs          - load openSimEarth.cs
	scriptExec.cs    - load terrainPager.cs and BadBehavior/main.cs
	weapon.cs        - attach a physics castRay to Ryder & Lurker weapons.

Outside of these files, everything else should be able to drop right into its correct place, assuming you have not modified the basic T3D game directory structure.

The openSimEarth.exe and dll files have been included as a convenience for artists and scripters, but it is expected that many users of this repository will be compiling their own executables, using the openSimEarth Torque3D repository:

   https://github.com/ChrisCalef/Torque3D.git (branch openSimEarth)





 
This is the development repository for the main openSimEarth game directory. The contents of this repository are meant to be dropped into a T3D game directory.

## Related repositories

* openSimEarth branch of [Torque 3D](https://github.com/ChrisCalef/Torque3D)


# License

    Copyright (c) 2016 Chris Calef

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to
    deal in the Software without restriction, including without limitation the
    rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
    sell copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
    
    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
    FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
    IN THE SOFTWARE.

