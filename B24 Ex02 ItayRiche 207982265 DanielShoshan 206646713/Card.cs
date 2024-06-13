using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B24_Ex02_ItayRiche_207982265_DanielShoshan_206646713
{
    public class Card
    {
        private bool m_selected;
        private string m_DataOfCard;

        public Card(string i_DataOfCell)
        {
            m_selected = false;
            m_DataOfCard = i_DataOfCell;
        }

        public bool Selected
        {
            get
            {
                return m_selected;
            }
            set
            {
                m_selected = value;
            }
        }

        public string DataOfCard
        {
            get
            {
                return m_DataOfCard;
            }
            set
            {
                m_DataOfCard = value;
            }
        }
    }
}
