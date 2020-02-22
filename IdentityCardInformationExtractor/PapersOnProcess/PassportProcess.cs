using IdentityCardInformationExtractor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityCardInformationExtractor.PapersOnProcess
{
    class PassportProcess
    {
        public IdentityCard IDCard;
        public string Text { get; set; }

        public PassportProcess(IdentityCard IDCard, string Text)
        {
            this.IDCard = IDCard;
            this.Text = Text;
        }

        public IdentityCard getIdentityCard() {
            return new IdentityCard();
        }
    }
}
