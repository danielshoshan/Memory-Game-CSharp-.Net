using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ex02.ConsoleUtils;

namespace B24_Ex02_ItayRiche_207982265_DanielShoshan_206646713
{
    public class ConsoleUserInterface
    {
        private MemoryGameLogic m_MemoryGameLogic;

        public MemoryGameLogic MemoryGameLogic
        {
            get
            {
                return m_MemoryGameLogic;
            }
        }

        public ConsoleUserInterface(int i_NumberOfRows, int i_NumberOfColumns)
        {
            m_MemoryGameLogic = new MemoryGameLogic(i_NumberOfRows, i_NumberOfColumns);
        }

        public static void RunMemoryConsole()
        {
            WelcomeToTheGame();

            int[] boardSizes = getValidBoardSizeFromUser();
            int userGameTypeChoice = getValidTypeOfGameFromUser();

            runTheMemoryGame(boardSizes[0], boardSizes[1], userGameTypeChoice);
        }

        private static void WelcomeToTheGame()
        {
            string welcomeOutput = string.Format(@"******************************
*          Welcome           *
*   To Memory card Game!    *
******************************");
            Console.WriteLine(welcomeOutput);
        }

        private static int[] getValidBoardSizeFromUser()
        {
            int[] boardSizes = new int[2];
            bool isValidInput = false;
            string output, numberOfRowsFromUser, numberOfColumnsFromUser;
            string message;
            int numberOfRows, numberOfColumns;

            do
            {
                output = string.Format(@"Please enter the size of the game board
Number of rows: (Must be between {0} to {1})", MemoryGameLogic.k_MinNumOfRowsOrColumns, MemoryGameLogic.k_MaxNumOfRowsOrColumns);
                Console.WriteLine(output);
                numberOfRowsFromUser = Console.ReadLine();
                Console.WriteLine($"Number of columns: (Must be between {MemoryGameLogic.k_MinNumOfRowsOrColumns} to {MemoryGameLogic.k_MaxNumOfRowsOrColumns})");
                numberOfColumnsFromUser = Console.ReadLine();

                if (int.TryParse(numberOfRowsFromUser, out numberOfRows) &&
                    int.TryParse(numberOfColumnsFromUser, out numberOfColumns))
                {
                    if (MemoryGameLogic.IsValidBoardSize(numberOfRows, numberOfColumns))
                    {
                        boardSizes[0] = numberOfRows;
                        boardSizes[1] = numberOfColumns;
                        isValidInput = true;
                    }
                    else
                    {
                        message = string.Format(@"One or both of your numbers submissions is out of range.
Please enter numbers between {0} to {1}",
                        MemoryGameLogic.k_MinNumOfRowsOrColumns, MemoryGameLogic.k_MaxNumOfRowsOrColumns);
                        invalidInputMessageDisplay(message);
                    }
                }
                else
                {
                    if (!int.TryParse(numberOfRowsFromUser, out numberOfRows))
                    {
                        message = string.Format("Your submission for number of rows must be a numeric value.");
                        invalidInputMessageDisplay(message);
                    }
                    if (!int.TryParse(numberOfColumnsFromUser, out numberOfColumns))
                    {
                        message = string.Format("Your submission for number of columns must be a numeric value.");
                        invalidInputMessageDisplay(message);
                    }
                }

            } while (!isValidInput);

            return boardSizes;
        }

        private static void invalidInputMessageDisplay(string i_InvalidInputReason)
        {
            StringBuilder wrongInputMessage = new StringBuilder("Invalid Input!");
            wrongInputMessage.Append(i_InvalidInputReason);
            Console.WriteLine(wrongInputMessage);
        }

        private static int getValidTypeOfGameFromUser()
        {
            bool isValidInput = false;
            int typeOfGame;
            string output, userInput;
            string message = null;

            Screen.Clear();

            do
            {
                output = string.Format(@"Choose your game mode:
Enter 1 to play against the computer (AI) or
Enter 2 to Play with a friend against each other (2 players) ");
                Console.WriteLine(output);
                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out typeOfGame))
                {
                    if ((typeOfGame == 1 || typeOfGame == 2))
                    {
                        isValidInput = true;
                    }
                    else
                    {
                        message = string.Format(@"Your number submission for game mode is out of range. 
Please enter only 1 or 2.");
                    }
                }
                else
                {
                    message = string.Format(@"Your submission for game mode must be a numeric value.");
                }
                if (message != null)
                {
                    invalidInputMessageDisplay(message);
                    Console.WriteLine("Try again.");
                }
            } while (!isValidInput);

            return typeOfGame;
        }

        private static void runTheMemoryGame(int i_NumberOfRows, int i_NumberOfColumns, int i_TypeOfGame)
        {
            ConsoleUserInterface game = new ConsoleUserInterface(i_NumberOfRows, i_NumberOfColumns);
            string userChoiceMove;
            bool validTurnWasMade = false;
            int numberOfCardChoosen = 0;
            int rowOfChoosenCard = 0, columnOfChoosenCard = 0;
            int rowOfFirstCard = 0, columnOfSecondCard = 0;
            Screen.Clear();
            game.MemoryGameLogic.TypeOfGame = (eTypeOfGame)i_TypeOfGame;

            while (true)
            {
                do
                {
                    game.gameBoardDisplay();

                    if (game.MemoryGameLogic.TypeOfGame == eTypeOfGame.UserVsComputer && game.MemoryGameLogic.CurrentPlayer == eCurrentPlayer.player2)
                    {
                        if(!game.MemoryGameLogic.checkIfBoardFull())
                        {
                            (rowOfChoosenCard, columnOfChoosenCard) = game.MemoryGameLogic.SmartComputerMove();
                            validTurnWasMade = game.MemoryGameLogic.chooseCard(rowOfChoosenCard, columnOfChoosenCard);
                        }
                    }

                    else
                    {
                        userChoiceMove = game.GetValidTurnMoveFromUser();
                        if (game.IsUserWantToQuit(userChoiceMove))
                        {
                            game.MemoryGameLogic.PlayerQuit();
                            Screen.Clear();
                            break;
                        }
                        else
                        {
                            (rowOfChoosenCard, columnOfChoosenCard) = ConvertToIndices(userChoiceMove);
                            validTurnWasMade = game.MemoryGameLogic.chooseCard(rowOfChoosenCard, columnOfChoosenCard);
                        }
                    }

                    if (validTurnWasMade)
                    {
                        Screen.Clear();
                        game.gameBoardDisplay();
                        numberOfCardChoosen++;
                        if (numberOfCardChoosen == 2)
                        {
                            Thread.Sleep(2000);
                            if (MemoryGameLogic.IsCardMatch(game.MemoryGameLogic.MemoryBoard.board[rowOfFirstCard, columnOfSecondCard], game.MemoryGameLogic.MemoryBoard.board[rowOfChoosenCard, columnOfChoosenCard]))
                            {
                                if (game.MemoryGameLogic.CurrentPlayer == eCurrentPlayer.player1)
                                {
                                    game.MemoryGameLogic.ScoreTracker.Player1NumberOfMatches++;
                                }
                                else
                                {
                                    game.MemoryGameLogic.ScoreTracker.Player2NumberOfMatches++;
                                }
                                game.MemoryGameLogic.ChangePlayerTurn();
                            }
                            else
                            {
                                game.MemoryGameLogic.MemoryBoard.board[rowOfFirstCard, columnOfSecondCard].Selected = false;
                                game.MemoryGameLogic.MemoryBoard.board[rowOfChoosenCard, columnOfChoosenCard].Selected = false;
                            }
                            game.MemoryGameLogic.ChangePlayerTurn();
                            numberOfCardChoosen = 0;
                            Screen.Clear();
                        }
                        else
                        {
                            rowOfFirstCard = rowOfChoosenCard;
                            columnOfSecondCard = columnOfChoosenCard;
                            Screen.Clear();
                        }
                    }

                } while (!game.MemoryGameLogic.checkIfBoardFull());

                game.displayOfTheWinner();
                game.scoreTrackerDisplay();
                game.MemoryGameLogic.ScoreTracker.Player1NumberOfMatches = 0;
                game.MemoryGameLogic.ScoreTracker.Player2NumberOfMatches = 0;
                if (!getValidAnswerNewGameFromUser())
                {
                    break;
                }
                RunMemoryConsole();
                Environment.Exit(0);
            }
        }

        private void gameBoardDisplay()
        {
            string boradOutput = buildBoard();
            string headlineOutput = string.Format(@"      ------------------------------
            Memory Game Board  
      ------------------------------");

            Console.WriteLine(headlineOutput);
            Console.WriteLine(boradOutput);
        }

        private string buildBoard()
        {
            StringBuilder boardDisplay = new StringBuilder();

            boardDisplay.Append("   ");
            for (int j = 0; j < MemoryGameLogic.MemoryBoard.NumberOfColumns; j++)
            {
                boardDisplay.Append("   " + (char)('A' + j) + "  ");
            }
            boardDisplay.AppendLine();

            boardDisplay.Append("   ");
            for (int j = 0; j < MemoryGameLogic.MemoryBoard.NumberOfColumns; j++)
            {
                boardDisplay.Append("======");
            }
            boardDisplay.AppendLine("=");

            for (int i = 0; i < MemoryGameLogic.MemoryBoard.NumberOfRows; i++)
            {
                boardDisplay.Append((i + 1).ToString().PadLeft(2) + " |");
                for (int j = 0; j < MemoryGameLogic.MemoryBoard.NumberOfColumns; j++)
                {

                    if (!(MemoryGameLogic.MemoryBoard.board[i, j].Selected))
                    {
                        boardDisplay.Append("     |");
                    }

                    else
                    {
                        boardDisplay.Append("  " + MemoryGameLogic.MemoryBoard.board[i, j].DataOfCard + "  |");
                    }
                }

                boardDisplay.AppendLine();
                boardDisplay.Append("   ");

                for (int j = 0; j < MemoryGameLogic.MemoryBoard.NumberOfColumns; j++)
                {
                    boardDisplay.Append("======");
                }
                boardDisplay.AppendLine("=");
            }

            return boardDisplay.ToString();
        }

        public string GetValidTurnMoveFromUser()
        {
            string output, userInput;
            string message = "";
            string userChoice = null;
            bool isValidInput = false;

            Console.WriteLine(@"Player {0}, it's your turn!", MemoryGameLogic.CurrentPlayer);

            do
            {
                output = string.Format(@"Please select a card (column then row). 

To quit press 'Q'.");
                Console.WriteLine(output);
                userInput = Console.ReadLine();

                if (IsValidInputFromUser(userInput))
                {
                    userChoice = userInput;
                    isValidInput = true;
                }
                else if ((userInput == "Q"))
                {
                    isValidInput = true;
                    userChoice = "Q";
                }
                else
                {
                    message = string.Format("Your submission must be a big letter and a number value or the letter Q. See that you don't pick an exposed card already.");
                }
                if (message != "")
                {
                    invalidInputMessageDisplay(message);
                    Console.WriteLine("Try again.");
                }
            } while (!isValidInput);

            return userChoice;
        }

        public bool IsValidInputFromUser(string i_UserInput)
        {
            bool isValidInput = false;
            if (i_UserInput.Length == 2)
            {
                char letter = i_UserInput[0];
                string numberPart = i_UserInput.Substring(1);
                if (int.TryParse(numberPart, out int row) && row >= 1 && row <= MemoryGameLogic.MemoryBoard.NumberOfRows
                    && letter >= 'A' && letter < 'A' + MemoryGameLogic.MemoryBoard.NumberOfColumns)
                {
                    isValidInput = true;
                }
            }
            return isValidInput;
        }

        public static (int row, int column) ConvertToIndices(string i_UserInput)
        {
            char letter = i_UserInput[0];
            string numberPart = i_UserInput.Substring(1);
            int column = char.ToUpper(letter) - 'A';
            int.TryParse(numberPart, out int row);
            row = row - 1;

            return (row, column);
        }

        private void scoreTrackerDisplay()
        {
            string tablescoreTracker = string.Format(@"
******************************
*       Score Tracker        *
******************************
------------------------------
|        Total score:        |
------------------------------
|    Player    |     Wins    |
------------------------------
|  Player 1    |      {0}    |
|  Player 2    |      {1}    |
------------------------------",
            MemoryGameLogic.ScoreTracker.Player1NumberOfMatches,
            MemoryGameLogic.ScoreTracker.Player2NumberOfMatches);
            Console.WriteLine(tablescoreTracker);
        }

        public void displayOfTheWinner()
        {
            if (MemoryGameLogic.ScoreTracker.Player1NumberOfMatches > MemoryGameLogic.ScoreTracker.Player2NumberOfMatches)
            {
                Console.WriteLine("We have a Winner! Congratulations Player1");
            }
            else if (MemoryGameLogic.ScoreTracker.Player1NumberOfMatches < MemoryGameLogic.ScoreTracker.Player2NumberOfMatches)
            {
                Console.WriteLine("We have a Winner! Congratulations Player2");
            }
            else
            {
                Console.WriteLine("Game ended with a Tie!");
            }
        }

        public bool IsUserWantToQuit(string i_UserInput)
        {
            return (i_UserInput == "Q");
        }

        private static bool getValidAnswerNewGameFromUser()
        {
            int newGame;
            bool resumeWithNewGame = false;
            bool isValidInput = false;
            string stringtypeOfGame;
            string message = null;

            do
            {
                Console.WriteLine("Would you like to play again? Press 1 for YES and 0 for NO");
                stringtypeOfGame = Console.ReadLine();
                if (int.TryParse(stringtypeOfGame, out newGame))
                {
                    if (newGame == 1)
                    {
                        Screen.Clear();
                        resumeWithNewGame = true;
                        isValidInput = true;
                    }
                    else if (newGame == 0)
                    {
                        Screen.Clear();
                        isValidInput = true;
                    }
                    else
                    {
                        message = string.Format(@"Your number submission is out of range.
Please enter only 1 or 2.");
                    }
                }
                else
                {
                    message = string.Format("Your submission for new play must be a numeric value.");
                }
                if (message != null)
                {
                    invalidInputMessageDisplay(message);
                    Console.WriteLine("Try again.");
                }
            } while (!isValidInput);

            return resumeWithNewGame;
        }
    }
}
