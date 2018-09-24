using System;
using System.IO;

namespace hashcode
{
	public class BigProgram
	{
		// predetermined integers from input file
		static int R = 1000;
		static int C = 1000;

		static int[,] inttop;
		static bool isFirstTime = true;
		static int slices = 0;
		static int i = 0;
		static int[,] sliceSaver = new int[83350, 4];

		public static void Main (string[] args)
		{
			int score = 0;

			//Instantiate a StreamReader to read from the text file.
			StreamReader sr = new StreamReader (@"big.in");
			// gets the first role from input file out of the way
			char[] row1 = sr.ReadLine ().ToCharArray();
			// string that stores all the elements from the input file (T and M)
			string toppings = sr.ReadToEnd ();
			// character array
			char[] chartop = toppings.ToCharArray();
			// integer array that converts T to 1, M to 0
			inttop = new int[R,C];

			// filling inttop with values
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

			// 2x7 slices
			int slicesCut = slicer (0, 0, 1, 6, 6, 9);
			slices = slicesCut + slices;
			score = slicesCut * 12;
			Console.WriteLine (slices + " " + score + "pts");

			// 7x2 slices
			slicesCut = slicer (0, 0, 6, 1, 6, 9);
			slices = slicesCut + slices;
			score = score + slicesCut * 12;
			Console.WriteLine (slices + " " + score + "pts");

			// 2x6 slices
			slicesCut = slicer (0, 0, 1, 5, 6, 7);
			slices = slicesCut + slices;
			score = score + slicesCut * 12;
			Console.WriteLine (slices + " " + score + "pts");

			// 6x2 slices
			slicesCut = slicer (0, 0, 5, 1, 6, 7);
			slices = slicesCut + slices;
			score = score + slicesCut * 12;
			Console.WriteLine (slices + " " + score + "pts");

			// 3x4 slices
			slicesCut = slicer (0, 0, 2, 3, 6, 7);
			slices = slicesCut + slices;
			score = score + slicesCut * 12;
			Console.WriteLine (slices + " " + score + "pts");

			// 4x3 slices
			slicesCut = slicer (0, 0, 3, 2, 6, 7);
			slices = slicesCut + slices;
			score = score + slicesCut * 12;
			Console.WriteLine (slices + " " + score + "pts");

			documenter ();
			Console.WriteLine ("DONE");
			Console.ReadKey ();
		}

		// the method that creates the output file and writes to it
		public static void documenter(){
			Console.WriteLine ("Saving output file...");
			for (int e=0;e<slices;e++) {
				string line = sliceSaver[e,0] + " " + sliceSaver[e,1] + " " + sliceSaver[e,2] + " " + sliceSaver[e,3];
				using (StreamWriter sw = new StreamWriter ("big_output.txt", true)) {
					if (isFirstTime) {
						sw.WriteLine (slices);
						isFirstTime = false;
					}
					sw.WriteLine (line);
				}
			}
		}

		// the method that cuts the pizza into suitable slices
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

		public static void saver(int r1, int c1, int r2, int c2) {
			sliceSaver[i,0] = r1;
			sliceSaver[i,1] = c1;
			sliceSaver[i,2] = r2;
			sliceSaver[i,3] = c2;
			i++;
		}

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
	}
}
