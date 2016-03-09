using System.Collections.Generic;

namespace ECA.Business.Queries.Models.Sevis
{
    public class ParticipantSevisBatchResultRecordDTO
    {

        public string sevisId { get; set; }

        public int participantId { get; set; }

        public int userId { get; set; }

        public List<ParticipantSevisBatchResultDTO> sevisResults { get; set; }

        //<Record sevisID = 'N0000000001' requestID='123' userID='1'>
        //    <Result status = '0' >
        //        < ErrorCode > S1056 </ ErrorCode >
        //        < ErrorMessage > Invalid student visa type for this action</ErrorMessage>
        //    </Result>
        //</Record>
    }
}
