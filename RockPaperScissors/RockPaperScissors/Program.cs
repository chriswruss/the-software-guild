using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaperScissors {
    class Program {
        static void Main(string[] args) {

            string userInput;
            int numberOfRounds, userChoice, computerChoice;
            const int MIN_NUMBER_OF_ROUNDS = 1;
            const int MAX_NUMBER_OF_ROUNDS = 10;
            bool isValidInput = false;
            bool donePlaying = false;
            const int ROCK = 1;
            const int PAPER = 2;
            const int SCISSORS = 3;
            const int MIN_VALUE = 1;
            const int MAX_VALUE = 3;

            //Takes three values and tests if the value passed in is greater than the max or less than the min
            bool OutOfRange(int value, int min, int max) {
                bool outOfRange = false;

                if(value < min || value > max) {
                    Console.WriteLine($"Value outside of valid range ({min} - {max}).");
                    outOfRange = true;
                }

                return outOfRange;
            }
            void PrintWinner(int playerScore, int computerScore, int gamesTied) {
                if (playerScore > computerScore) {
                    Console.WriteLine("Congratulations! You won!");
                    return;
                }
                else if (playerScore < computerScore) {
                    Console.WriteLine("Better luck next time!");
                    return;
                }
                else {
                    Console.WriteLine("A tie is better than losing right?");
                }
            }
            void PrintComputerChoice() {
                switch (computerChoice) {
                    case 1:
                        Console.WriteLine("Computer chose Rock!");
                        break;
                    case 2:
                        Console.WriteLine("Computer chose Paper!");
                        break;
                    case 3:
                        Console.WriteLine("Computer chose Scissors!");
                        break;
                    default:
                        break;
                }
            }

            while(donePlaying == false) {

                //Variables initialized here so their values are reset after each round
                int playerWins = 0;
                int computerWins = 0;
                int ties = 0;
                int roundsPlayed = 0;
                bool reachedLastRound = false;

                //Get number of rounds the user wishes to play
                Console.Write("Enter number of rounds to be played: ");
                userInput = Console.ReadLine();
                isValidInput = int.TryParse(userInput, out numberOfRounds);

                //Checks user input to make sure they entered an integer
                if (!isValidInput) {
                    Console.WriteLine("User did not enter an integer value.");
                    Console.WriteLine("Exiting program...");
                    Console.ReadLine(); //Pause before returning so user can see the error message
                    return;
                }

                //Checks user input to make sure it's within bounds
                if (OutOfRange(numberOfRounds, MIN_NUMBER_OF_ROUNDS, MAX_NUMBER_OF_ROUNDS)) {
                    Console.WriteLine("Exiting program...");
                    Console.ReadLine(); //Pause before returning so user can see the error message
                    return;
                }

                while (reachedLastRound == false) {
                    roundsPlayed++; // Start of a round so increment rounds played

                    //Check if we're on the last round
                    if (roundsPlayed == numberOfRounds) {
                        reachedLastRound = true;
                    }

                    //Gets user's choice of rock, paper, or scissors
                    do {
                        Console.Write("What would you like to play -> (1)-Rock (2)-Paper (3)-Scissors <- : ");
                        userInput = Console.ReadLine();
                        isValidInput = int.TryParse(userInput, out userChoice);

                        //Check that user input is an integer and within the range
                        if (!isValidInput || OutOfRange(userChoice, MIN_VALUE, MAX_VALUE)) {
                            Console.WriteLine("Enter either 1 for rock, 2 for paper, or 3 for scissors.");
                            isValidInput = false;
                        }
                    } while (!isValidInput) ;

                    Random random = new Random();

                    computerChoice = random.Next(MIN_VALUE, MAX_VALUE - 1); // Generate a random choice for the computer

                    //Check for a tie
                    if (userChoice == computerChoice) {
                        ties++;
                    }
                    else if (userChoice == ROCK && computerChoice == PAPER) { //Player chose Rock, computer chose Paper
                        computerWins++;
                    }
                    else if (userChoice == ROCK && computerChoice == SCISSORS) { //Player chose Rock, computer chose Scissors
                        playerWins++;
                    }
                    else if (userChoice == PAPER && computerChoice == ROCK) { //Player chose Paper, computer chose Rock
                        playerWins++;
                    }
                    else if (userChoice == PAPER && computerChoice == SCISSORS) { //Player chose Paper, computer chose Scissors
                        computerWins++;
                    }
                    else if (userChoice == SCISSORS && computerChoice == ROCK) { //Player chose Scissors, computer chose Rock
                        computerWins++;
                    }
                    else if (userChoice == SCISSORS && computerChoice == PAPER) { //Player chose Scissors, computer chose Paper
                        playerWins++;
                    }

                    PrintComputerChoice();

                    //Print out the current running score
                    Console.WriteLine($"Player wins: {playerWins}, Computer wins: {computerWins}, Ties: {ties}\n");
                }

                //Print out the winner
                PrintWinner(playerWins, computerWins, ties);
                
                do {
                    //See if user wants to play again
                    Console.Write("Enter \"yes\" to play again or \"no\" to exit: ");
                    userInput = Console.ReadLine();
                } while (!userInput.Equals("yes", StringComparison.OrdinalIgnoreCase) && !userInput.Equals("no", StringComparison.OrdinalIgnoreCase)); //Make sure user enters yes or no

                //donePlaying is initialized to false, so we only have to check if we need to switch it to true
                if (userInput == "no") {
                    donePlaying = true;
                }
            }
        }
    }
}
