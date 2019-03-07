using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogGenetics {
    class Program {
        static void Main(string[] args) {
            string[] dogBreeds = { "Affenpinscher", "Alaskan Klee Kai", "Bullmastiff", "Cursinu", "Georgian Shepherd Dog", "Ibizan Hound",
                                   "Kyi-Leo", "Norfolk Spaniel", "Poodle", "Sakhalin Husky", "Spanish Mastiff", "Tyrolean Hound", "Xiasi Dog"};
            string dogName;
            bool isValidName = true;
            int remainingPercentage = 100;
            int breedArrayLocator; //Gets an int and looks in the dogBreed array at that index to pull a breed
            int breedCount = 0; //Used to keep track of how many breeds we've generated
            int percentageCount = 0; //Used to keep track of how many percentages we've generated
            int percentage; //Stores the percentage for that iteration
            const int MAX_NUMBER_OF_BREEDS = 5;
            string[,] userDogGenetics = new string[MAX_NUMBER_OF_BREEDS, 2]; //Stores the breed name in first column and percentage in second column

            //Get dog's name
            do {
                Console.Write("What's your dog's name? -> ");
                dogName = Console.ReadLine();

                //Check if the dog's name is null or empty
                if(string.IsNullOrWhiteSpace(dogName)) {
                    Console.WriteLine("That's not a name! Play nice...\n");
                    isValidName = false;
                }
                else {
                    isValidName = true;
                }
            } while (!isValidName);

            Random random = new Random(); //Used to generate random percentages for each dog breed

            //Start loop to get five dog breeds
            do {
                breedArrayLocator = random.Next(0, dogBreeds.Length);

                if (dogBreeds[breedArrayLocator] != null) { //Checks if null: when a breed is pulled, the index is set to null
                    //Stores the breed in the breed name column of the user dog genetics array
                    userDogGenetics[breedCount, 0] = dogBreeds[breedArrayLocator];
                    dogBreeds[breedArrayLocator] = null;
                    if (breedCount < MAX_NUMBER_OF_BREEDS) { 
                        breedCount++; //Increment breed count to ensure no further iterations past breed count limit
                    }
                }
            } while (breedCount < MAX_NUMBER_OF_BREEDS);
            
            //Start loop to generate percentages for each breed
            while(remainingPercentage > 0) {
                //Sets percentage to one if there is only one percent remaining
                if (remainingPercentage == 1) {
                    percentage = 1;
                    remainingPercentage = 0;
                }
                //Sets percentage to the remaining percent if this is our final calculation
                else if (percentageCount == MAX_NUMBER_OF_BREEDS - 1) {
                    percentage = remainingPercentage;
                    remainingPercentage = 0;
                }
                else {
                    //This generates a random number between one and remaining percent while ensuring there is enough percetange remaining to cover the rest of the breeds
                    percentage = random.Next(1, (remainingPercentage - ((MAX_NUMBER_OF_BREEDS - 1) - percentageCount))); 
                    remainingPercentage -= percentage;
                }
                //Stores the percentage in the percentage column of the user dog genetics array
                userDogGenetics[percentageCount, 1] = percentage.ToString();
                if (percentageCount < MAX_NUMBER_OF_BREEDS) {
                    percentageCount++; //Increment percentage count to ensure no further iterations past percentage count limit
                }
            }//End loop to generate percentages for each breed
            
            Console.WriteLine($"Well then, I have this highly reliable report on {dogName}'s prestigious background right here.\n");
            Console.WriteLine($"{dogName} is:\n");
            
            //Print results
            for(int i = 0; i < MAX_NUMBER_OF_BREEDS; i++) {
                Console.WriteLine($"{userDogGenetics[i, 1]}% {userDogGenetics[i, 0]}");
            }

            Console.WriteLine("\nWow, that's quite the dog!");
            
            //Keeps the program from immediately closing 
            Console.ReadLine(); 
        } //End of Main
    }
}
