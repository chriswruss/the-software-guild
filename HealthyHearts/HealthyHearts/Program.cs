using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyHearts {
    class Program {
        static void Main(string[] args) {
            int userAge, maxHeartRate;
            const int MAX_AGE = 122;
            float heartRateZoneLower, heartRateZoneUpper;
            string userInput; //Variable to get all user input
            bool isValidInput = true; //Used to make sure user input is an integer
            const float LOWER_ZONE_PERCENTAGE = 0.5f;
            const float UPPER_ZONE_PERCENTAGE = 0.85f;
            const int MAXIMUM_HEART_RATE = 220;

            //Get user age until a number
            do {
                Console.Write("Enter your age: ");
                userInput = Console.ReadLine();

                isValidInput = int.TryParse(userInput, out userAge); //Checks that user inputed an integer

                if (!isValidInput) { //Print error messsage if user did not enter an integer
                    Console.WriteLine("Please enter an integer value...");
                }
                else if (userAge <= 0) { //Print error message if user entered zero or less
                    Console.WriteLine("There's no way you're that young...");
                    isValidInput = false;
                }
                else if (userAge > MAX_AGE) {
                    Console.WriteLine("If you're that old, you need to contact Guiness, because you hold a record!");
                    Console.WriteLine("I'm going to assume there was a typo and send you back to the beginning...");
                    isValidInput = false;
                }
            } while (!isValidInput);

            //Compute max heart rate and targeted heart rate zones
            maxHeartRate = MAXIMUM_HEART_RATE - userAge;
            heartRateZoneLower = maxHeartRate * LOWER_ZONE_PERCENTAGE; //Lower heart rate zone is 50% of user max heart rate
            heartRateZoneUpper = maxHeartRate * UPPER_ZONE_PERCENTAGE; //Upper heart rate zone is 85% of user max heart rate

            //Convert the heart rate zones to ints for easy rounding
            heartRateZoneLower = Convert.ToInt32(heartRateZoneLower);
            heartRateZoneUpper = Convert.ToInt32(heartRateZoneUpper);

            //Print max heart rate and targeted heart rate zones to the user
            Console.WriteLine($"Your maximum heart rate should be {maxHeartRate} beats per minute.");
            Console.WriteLine($"Your target heart rate zone is {heartRateZoneLower} - {heartRateZoneUpper} beats per minute.");

            //Keeps program from exiting automatically
            Console.ReadLine();
        } //End of Main
    }
}
