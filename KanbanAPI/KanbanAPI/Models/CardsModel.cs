


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KanbanAPI.Models
{
    public class CardsModel
    {
        public int CardId { get; set; }
        public int ListId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string Text { get; set; }

    }
}