# Fractal Generator
This application can render [fractals](https://en.wikipedia.org/wiki/Fractal) by putting the IFS code.

### INPUT IFS code
- Enter a, b, c, d, e, f, p in [IFS Matrix](https://en.wikipedia.org/wiki/Iterated_function_system) and add multiple rows for extending the matrix.
[Here](https://github.com/shreyanshanchlia/FractalGenerator/blob/main/IFS%20Samples.md) are some sample IFS codes to try out.

## Features
#### Update Check
Updates are checked automatically using the latest version specified [here](https://github.com/shreyanshanchlia/FractalGenerator/blob/main/LatestVersion.txt).

### Algorithms
* [Random iteration algorithm](https://en.wikipedia.org/wiki/Randomized_algorithm)
* [Deterministic algorithm](https://en.wikipedia.org/wiki/Deterministic_algorithm)

### Simplicity Features
1. Copy the matrix to clipboard for redeploying / showing off :stuck_out_tongue_winking_eye:
1. Paste the matrix from clipboard, and it will be automatically serialized into matrix for you.
1. Delete the matrix for deploying a new matrix. </br>

#### <b> Sample IFS Matrix (Terdragon) for deterministic algorithm: </b><br>
0.5 -0.289 0.289 0.5 0 0 <br>
0 0.577 -0.577 0 0.5 0.289 <br>
0.5 -0.289 0.289 0.5 0.5 -0.289

#### <b> Sample IFS Matrix (Barnsley Fern) for random iteration algorithm: </b><br>
0 0 0 0.16 0 0 0.01 <br>
0.85 0.04 -0.04 0.85 0 1.6 0.85 <br>
0.20 -0.26 0.23 0.22 0 1.6 0.07 <br>
-0.15 0.28 0.26 0.24 0 0.44 0.07

### Navigation and User Control
- on remaining idle (no mouse movement / keyboard input) for a particular time turns the menu off.
- Mouse scroll on fractal changes zoom level.
- Mouse pan changes camera position.

### Customize fractal

#### Random Iteration Algorithm.
1. Iteration control 
   1. Speed. <br>
   Number of times to run per frame.
   1. Total iterations. <br>
   Number of times to iterate.
1. Point of origin. <br>
Click to reset camera.

#### Random Iteration Algorithm.
1. Size of the fractal
1. Iteration control 
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
Made with unity 2019.4.17f1 <br>
For scripts, goto Assets -> Scripts. <br>
Fork and write issue before pull request, if not a collaborator. <br>
Readme commits will not be accepted unless very important. <br>
