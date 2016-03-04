using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ECA.Business.Sevis
{
    public class SevisDownload
    {
        /// <summary>
        /// The Zipfile containing the DS-2019's for the batch
        /// </summary>
        public byte[] Zipfile;

        /// <summary>
        /// The SEVIS transaction log as XML
        /// </summary>
        public XElement TransactionLog;
    }
}
