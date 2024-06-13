using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B24_Ex02_ItayRiche_207982265_DanielShoshan_206646713
{
    public class GameBoard
    {
        private Card[,] m_board;
        private int m_NumberOfRows;
        private int m_NumberOfColumns;

        public Card[,] board
        {
            get
            {
                return m_board;
            }
            set
            {
                m_board = value;
            }
        }

        public int NumberOfRows
        {
            get
            {
                return m_NumberOfRows;
            }
        }

        public int NumberOfColumns
        {
            get
            {
                return m_NumberOfColumns;
            }
        }

        public GameBoard(int i_NumberOfRows, int i_NumberOfColumns)
        {
            Random random = new Random();
            m_NumberOfRows = i_NumberOfRows;
            m_NumberOfColumns = i_NumberOfColumns;

            board = new Card[m_NumberOfRows, m_NumberOfColumns];

            List<string> LettersForBoard = MakeList(i_NumberOfRows, i_NumberOfColumns);
            for (int i = 0; i < m_NumberOfRows; i++)
            {
                for (int j = 0; j < m_NumberOfColumns; j++)
                {
                    int randomIndex = random.Next(LettersForBoard.Count);
                    string selectedLetter = LettersForBoard[randomIndex];
                    LettersForBoard.RemoveAt(randomIndex);
                    board[i, j] = new Card(selectedLetter);
                }
            }
        }

        public List<string> MakeList(int i_NumberOfRows, int i_NumberOfColumns)
        {
            List<string> letters = new List<string>();
            char letter = 'A';
            for (int i = 0; i < (i_NumberOfRows * i_NumberOfColumns) / 2; i++)
            {
                letters.Add(letter.ToString());
                letters.Add(letter.ToString());
                letter++;
            }

            Random random = new Random();
            letters = letters.OrderBy(a => random.Next()).ToList();
            return letters;
        }

        public void ClearBoard()
        {
            for (int i = 0; i < m_NumberOfRows; i++)
            {
                for (int j = 0; j < m_NumberOfColumns; j++)
                {
                    m_board[i, j].DataOfCard = null;
                    m_board[i, j].Selected = false;
                }
            }
        }
    }
}
