# Google HashCode Practice Problem Solution

This is the source code for a solution to the Google HashCode sample problem question. It is a problem where we have to find the most efficient way to slice a pizza into rectangular pieces. See the [Pizza](pizza.pdf) file for more details.

## Getting Started

Its very easy to test out this code. All you need is an IDE that can run C# code.
[Xamarin Studio](https://developer.xamarin.com/releases/studio/xamarin.studio_6.3/xamarin.studio_6.3/) was used for writing and running this code last year (2017), but it is now recommended to use Visual Studio. A free version, [Visual Studio Community](https://visualstudio.microsoft.com/) is available for download.

## Explaining the solution

There are four scripts in total, each of which takes its own input files. ie [BigProgram](BigProgram.cs) takes the [big.in](big.in) file as an input, and so on. The reason for using seperate scripts for each file was to fine tune and optimise the functions for each individual input. Since each input has different pizza slice combinations, the functions and values were hard-coded for each one.

The input file comes in a Row * Column grid of T and M, with the first line having 4 digits representing row, column, minimum number of each ingredient in a slice, and the maximum number of cells in a slice, respectively, like:
```
3 5 1 6
TTTTT
TMMMT
TTTTT
```

So this means that each slice must contain at least one T and one M, and the max cells in a slice cannot go beyond 6, leaving as little waste as possible. Given that, in this solution, the pizza is represented in the code as a two-dimensional integer array. The Ts and Ms are converted to 1s and 0s, respectively, to simplify computation done for toppings (to be explained further).

### Global variables

There are some global variables that are declared at the beginning of each script as follows:
```
static int R;
static int C;
static int[,] inttop;
static int i = 0;
static int slices = 0;
static int[,] sliceSaver = new int[83550, 4];
```
* *R* is the number of rows in the input file, *C* is columns
* *inttop* is the integer array that holds the coordinates of each cell in the pizza grid, T represented as 1, M as 0
* *i* is used to track the size of pizza slice being cut (more on this later)
* *slices* keeps count of the number of slices that have been cut
* *sliceSaver* holds the actual coordinates of slices that have been cut, to check for overlapping when other slices are being cut

### Functions and Code Snippets

The code below is a for loop that turns the Ts and Ms to binary values and stores them in our integer array. This is the first important step before we can start slicing our pizza.
```
int z=0;
for (int x=0;x<R;x++) {
  for (int y=0;y<C;y++) {
    if (chartop [z] == 'T') {
      inttop [x, y] = 1;
    } else if (chartop [z] == 'M')
      inttop [x, y] = 0;
    else{
      y--;
    }
    z++;
  }
}
```

After we create and populate our array, we can then start to cut slices out of it. To do this, we have a *slicer* function that takes in 4 coordinates, and 2 min and max values, for a total of 6 integer parameters.
```
public static int slicer(int r1, int c1, int r2, int c2, int min, int max){
  int sliceCell = 0;
  int slice = 0;

  for (int r=0;r<R-r2;r++) {
    for (int c=0;c<C-c2;c++) {
      for (int x=r1+r;x<=r2+r;x++) {
        for (int y=c1+c;y<=c2+c;y++) {
          if (inttop [x, y] == 1) {
            sliceCell++;
          }
        }
      }
      if (sliceCell < max) {
        if (sliceCell >= min) {
          bool isSlice = checker (r1 + r, c1 + c, r2 + r, c2 + c);
          if (isSlice)
            slice++;
        }
      }
      sliceCell = 0;
    }
  }
  return slice;
}
```

*r1, c1, r2, c2* are the coordinates to 2 diagonally opposite corners that make up the slice of pizza, *r1, c1* being the top left point, *r2, c2* the bottom right point. This gives the program the bounds of the slice of pizza to be cut. The *min* is used to set the minimum amount of T (tomatoes) we want on each slice, and *max* the maximum. Given that we're processing the toppings as integers, it makes it easy to count the number of toppings on each slice by simply summing the 1s that make up the Ts. This will give us the number for tomatoes, and the remainder from the whole slice represents the Ms (mushroooms) which are 0s.

The *checker* function is used to check if a newly made slice is being cut out of the part of the grid that has already been sliced. It does so by iterating through the *sliceSaver* array which stores the valid slices cut from the pizza. If a pizza slice does not overlap, it is added to the *sliceSaver* array and the number of slices is incremented.
```
public static bool checker(int r1, int c1, int r2, int c2) {
  bool check = true;
  for (int a = 0; a <= i; a++) {
    if (a == i)
      break;
    for (int r = r1; r <= r2; r++) {
      for (int c = c1; c <= c2; c++) {
        if ((sliceSaver [a, 0] <= r && r <= sliceSaver [a, 2]) && (sliceSaver [a, 1] <= c && c <= sliceSaver [a, 3])) {
          check = false;
        }
      }
    }
  }
  if (check)
    saver (r1, c1, r2, c2);
  return check;
}
```

After a successful check, the slice is saved using the *saver* function.
```
public static void saver(int r1, int c1, int r2, int c2) {
  sliceSaver[i,0] = r1;
  sliceSaver[i,1] = c1;
  sliceSaver[i,2] = r2;
  sliceSaver[i,3] = c2;
  i++;
}
```

Lastly, we have to output our data on the number of slices, as well as the slices themselves into a file to upload to Google for marking. This is done in the *documenter* function.
```
// the method that creates the output file and writes to it
public static void documenter(){
  Console.WriteLine ("Saving output file...");
  for (int e=0;e<slices;e++) {
    string line = sliceSaver[e,0] + " " + sliceSaver[e,1] + " " + sliceSaver[e,2] + " " + sliceSaver[e,3];
    using (StreamWriter sw = new StreamWriter ("example_output.txt", true)) {
      if (isFirstTime) {
        sw.WriteLine (slices);
        isFirstTime = false;
      }
      sw.WriteLine (line);
    }
  }
}
```

## Conclusion

There is definitely a lot of room for improvement for this solution. It was done early 2017 and hasn't been improved since, but some areas that can do with improvement are:

* Reading the R and C values directly from the file rather than hard-coding them
* Using better data structures and algorithms to process through the inputs, it can take several minutes to run the big input, no doubt due to inefficiencies in the coding structure
* Automating the generation of slicing bounds mathematically depending on the size of the pizza, rather than hard-coding the bounds

Hopefully this was an insightful read on one method to overcome this problem. All the best in your own endeavors!

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
