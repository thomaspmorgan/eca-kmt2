using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    public class Position
    {
        private const int POSITION_CODE_LENGTH = 3;

        public Position()
        {
            this.History = new History();
        }

        public int PositionId { get; set; }
        [MinLength(POSITION_CODE_LENGTH), MaxLength(POSITION_CODE_LENGTH)]
        public string PositionCode { get; set; }
        public string Description { get; set; }

        public History History { get; set; }
    }
}
