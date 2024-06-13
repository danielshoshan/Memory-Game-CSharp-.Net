using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B24_Ex02_ItayRiche_207982265_DanielShoshan_206646713
{
    public class ScoreTracker
    {
        private int m_Player1NumberOfMatches;
        private int m_Player2NumberOfMatches;

        public int Player1NumberOfMatches
        {
            get
            {
                return m_Player1NumberOfMatches;
            }

            set
            {
                m_Player1NumberOfMatches = value;
            }
        }

        public int Player2NumberOfMatches
        {
            get
            {
                return m_Player2NumberOfMatches;
            }

            set
            {
                m_Player2NumberOfMatches = value;
            }
        }
    }
}
