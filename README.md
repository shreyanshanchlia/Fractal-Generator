# Fractal Generator
This application can render [fractals](https://en.wikipedia.org/wiki/Fractal) by putting the IFS code.

### INPUT IFS code
- Enter a, b, c, d, e, f, p in [IFS Matrix](https://en.wikipedia.org/wiki/Iterated_function_system) and add multiple rows for extending the matrix.

## Features

### Algorithms
* [Random iteration algorithm](https://en.wikipedia.org/wiki/Randomized_algorithm)

### Simplicity Features
1. Copy the matrix to clipboard for redeploying / showing off :stuck_out_tongue_winking_eye:
1. Paste the matrix from clipboard, and it will be automatically serialized into matrix for you.
1. Delete the matrix for deploying a new matrix. </br>

#### <b> Sample IFS Matrix (Barnsley Fern): </b><br>
0 0 0 0.16 0 0 0.01 <br>
0.85 0.04 -0.04 0.85 0 1.6 0.85 <br>
0.20 -0.26 0.23 0.22 0 1.6 0.07 <br>
-0.15 0.28 0.26 0.24 0 0.44 0.07

### Customize fractal
1. Size of the fractal
1. Iteration control
   1. Random Iteration.
   1. Speed of iteration. <br>
   Number of pixels drawn per frame.
   1. Total iterations. <br>
   Number of times to iterate in total.
1. Point of origin. <br>
Click on the plane to change the point of origin.

### Beautification Features
- Change Background Color - Use already given presets or type your hex code
- Change Fractal Color - Use already given presets or type your hex code

## Development Guidlines
Made with unity 2019.4.17f1
For scripts, goto Assets -> Scripts.
Fork and write issue before pull request, if not a collaborator.
Readme commits will not be accepted unless very important.
