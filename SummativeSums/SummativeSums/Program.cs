using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummativeSums {
    class Program {
        static void Main(string[] args) {
            const int NUMBER_OF_ARRAYS_TO_TEST = 3;
            //These arrays were given
            int[] array1 = { 1, 90, -33, -55, 67, -16, 28, -55, 15 };
            int[] array2 = { 999, -60, -77, 14, 160, 301 };
            int[] array3 = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, -99 };
            int[] arraySums = new int[NUMBER_OF_ARRAYS_TO_TEST];

            //Assign the sum of each in corresponding array
            arraySums[0] = GetArraySum(array1);
            arraySums[1] = GetArraySum(array2);
            arraySums[2] = GetArraySum(array3);
            
            //Print the array sums
            for (int i = 0; i < NUMBER_OF_ARRAYS_TO_TEST; i++) {
                Console.WriteLine($"#{i + 1} Array Sum: {arraySums[i]}");
            }

            //Keeps the program from exiting immediately
            Console.ReadLine();
        }

        //Method that will loop through the array and compute the sum
        static int GetArraySum(int[] array) {
            int arraySum = 0;
            for (int i = 0; i < array.Length; i++) {
                arraySum += array[i];
            }
            return arraySum;
        }
    }
}
