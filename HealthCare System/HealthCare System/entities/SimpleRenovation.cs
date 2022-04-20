﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace HealthCare_System.entities
{
    class SimpleRenovation : Renovation
    {
        Room room;

        public SimpleRenovation(){}

        public SimpleRenovation(Renovation renovation) : base(renovation)
        {
            this.room = null;
        }

        public SimpleRenovation(int id, DateTime beginningDate, DateTime endingDate) : base(id, beginningDate, endingDate)
        {
            this.room = null;
        }

        public SimpleRenovation(int id, DateTime beginningDate, DateTime endingDate, Room room) : base(id, beginningDate, endingDate)
        {
            this.room = room;
        }

        public SimpleRenovation(SimpleRenovation renovation) : base(renovation)
        {
            this.room = renovation.room;
        }

        [JsonIgnore]
        public Room Room { get => room; set => room = value; }

        public override string ToString()
        {
            return "SimpleRenovation" + base.ToString();
        }
    }
}