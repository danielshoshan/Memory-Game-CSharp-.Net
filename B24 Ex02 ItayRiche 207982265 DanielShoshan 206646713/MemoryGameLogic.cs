using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace B24_Ex02_ItayRiche_207982265_DanielShoshan_206646713
{
    public class MemoryGameLogic
    {
        private GameBoard m_MemoryBoard;
        private ScoreTracker m_ScoreTracker;
        private eTypeOfGame m_TypeOfGame;
        private eCurrentPlayer m_CurrentPlayer;
        private Dictionary<string, List<(int row, int column)>> m_Memory;

        public const int k_MaxNumOfRowsOrColumns = 6;
        public const int k_MinNumOfRowsOrColumns = 4;

        public MemoryGameLogic(int i_NumberOfRows, int i_NumberOfColumns)
        {
            if (IsValidBoardSize(i_NumberOfRows, i_NumberOfColumns))
            {
                m_MemoryBoard = new GameBoard(i_NumberOfRows, i_NumberOfColumns);
                m_ScoreTracker = new ScoreTracker();
                m_Memory = new Dictionary<string, List<(int row, int column)>>();
            }
        }

        public GameBoard MemoryBoard
        {
            get
            {
                return m_MemoryBoard;
            }
        }

        public ScoreTracker ScoreTracker
        {
            get
            {
                return m_ScoreTracker;
            }
        }

        public eTypeOfGame TypeOfGame
        {
            get
            {
                return m_TypeOfGame;
            }
            set
            {
                m_TypeOfGame = value;
            }
        }

        public eCurrentPlayer CurrentPlayer
        {
            get
            {
                return (eCurrentPlayer)m_CurrentPlayer;
            }
            set
            {
                m_CurrentPlayer = value;
            }
        }

        public static bool IsValidBoardSize(int i_NumberOfRows, int i_NumberOfColumns)
        {
            return ((i_NumberOfRows <= k_MaxNumOfRowsOrColumns) && (i_NumberOfRows >= k_MinNumOfRowsOrColumns) &&
                   (i_NumberOfColumns <= k_MaxNumOfRowsOrColumns) && (i_NumberOfColumns >= k_MinNumOfRowsOrColumns) &&
                   ((i_NumberOfRows * i_NumberOfColumns) % 2) == 0);
        }

        public static bool IsCardMatch(Card i_card1, Card i_card2)
        {
            return (i_card1.DataOfCard == i_card2.DataOfCard);
        }

        public void ChangePlayerTurn()
        {
            if (m_CurrentPlayer == eCurrentPlayer.player1)
            {
                m_CurrentPlayer = eCurrentPlayer.player2;
            }
            else
            {
                m_CurrentPlayer = eCurrentPlayer.player1;
            }
        }

        public bool chooseCard(int i_RowNumber, int i_ColumnNumber)
        {
            bool cardSelected = false;
            if (!m_MemoryBoard.board[i_RowNumber, i_ColumnNumber].Selected)
            {
                m_MemoryBoard.board[i_RowNumber, i_ColumnNumber].Selected = true;
                cardSelected = true;
                UpdateMemory(i_RowNumber, i_ColumnNumber, m_MemoryBoard.board[i_RowNumber, i_ColumnNumber].DataOfCard);
            }
            return cardSelected;
        }

        public (int row, int column) SmartComputerMove()
        {
            foreach (var entry in m_Memory)
            {
                if (entry.Value.Count == 2)
                {
                    var firstCard = entry.Value[0];
                    entry.Value.RemoveAt(0);
                    return firstCard;
                }
            }

            return RandomComputerMove();
        }

        public (int row, int column) RandomComputerMove()
        {
            int selectedCard;
            Random random = new Random();
            List<int> availableMoves = new List<int>();

            for (int i = 0; i < m_MemoryBoard.NumberOfRows; i++)
            {
                for (int j = 0; j < m_MemoryBoard.NumberOfColumns; j++)
                {
                    if (!m_MemoryBoard.board[i, j].Selected)
                    {
                        availableMoves.Add(i * m_MemoryBoard.NumberOfColumns + j + 1);
                    }
                }
            }

            selectedCard = availableMoves[random.Next(availableMoves.Count)];
            int numberOfRow = (int)((selectedCard - 1) / m_MemoryBoard.NumberOfColumns);
            int numberOfColumn = ((selectedCard - 1) % m_MemoryBoard.NumberOfColumns);

            return (numberOfRow, numberOfColumn);
        }

        public bool IsGameEnd()
        {
            bool gameEnd = true;

            for (int i = 0; i < m_MemoryBoard.NumberOfRows; i++)
            {
                for (int j = 0; j < m_MemoryBoard.NumberOfColumns; j++)
                {
                    if (!(m_MemoryBoard.board[i, j].Selected))
                    {
                        gameEnd = false;
                        break;
                    }
                }
            }

            return gameEnd;
        }

        public void PlayerQuit()
        {
            CurrentPlayer = eCurrentPlayer.player1;
            m_MemoryBoard.ClearBoard();
        }

        public bool checkIfBoardFull()
        {
            bool boardFull = true;
            for (int i = 0; i < m_MemoryBoard.NumberOfRows; i++)
            {
                for (int j = 0; j < m_MemoryBoard.NumberOfColumns; j++)
                {
                    if (!(MemoryBoard.board[i, j].Selected))
                    {
                        boardFull = false;
                        break;
                    }
                }
            }
            return boardFull;
        }

        private void UpdateMemory(int row, int column, string value)
        {
            if (!m_Memory.ContainsKey(value))
            {
                m_Memory[value] = new List<(int row, int column)>();
            }

            m_Memory[value].Add((row, column));
        }
    }
}
