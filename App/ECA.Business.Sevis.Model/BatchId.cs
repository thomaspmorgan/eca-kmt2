using System;

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Sevis.Model
{
    //http://trycatch.me/counting-in-base62/

    /// <summary>
    /// A BatchId is used to uniquely identify a sevis batch document without having to do a round trip to the database.  The string
    /// will never be more than 14 characters long and is suitable for batch ids.
    /// </summary>
    public class BatchId
    {
        /// <summary>
        /// The characters to encode with.
        /// </summary>
        public static string ALPHANUMERIC =
            "0123456789" +
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Creates a new BatchId with the given string representation of a batch id.
        /// </summary>
        /// <param name="batchId">The batch id as a string.</param>
        public BatchId(string batchId)
        {
            this.Id = FromBase(batchId, ALPHANUMERIC);
        }

        /// <summary>
        /// Creates a new BatchId with the given long value to encode.
        /// </summary>
        /// <param name="id">The id to encode.</param>
        public BatchId(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets the Id.
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Creates a new Batch Id using DateTime.UtcNow.Ticks as an id.
        /// </summary>
        /// <returns>The new batch id.</returns>
        public static BatchId NewBatchId()
        {
            //http://stackoverflow.com/questions/1668353/how-can-i-generate-a-cryptographically-secure-pseudorandom-number-in-c
            //https://msdn.microsoft.com/en-us/library/system.bitconverter.toint64(v=vs.110).aspx
            var now = DateTime.UtcNow;
            var ticks = now.Ticks;
            return new BatchId(ticks);
        }

        private string ToBase(long input, string baseChars)
        {
            Contract.Requires(baseChars != null, "The base chars string must not be null.");
            string r = string.Empty;
            int targetBase = baseChars.Length;
            do
            {
                r = string.Format("{0}{1}",
                    baseChars[(int)(input % targetBase)],
                    r);
                input /= targetBase;
            } while (input > 0);

            return r;
        }

        private long FromBase(string input, string baseChars)
        {
            Contract.Requires(baseChars != null, "The base chars string must not be null.");
            int srcBase = baseChars.Length;
            long id = 0;
            var r = input.Reverse().ToArray();

            for (int i = 0; i < r.Length; i++)
            {
                int charIndex = baseChars.IndexOf(r[i]);
                id += charIndex * (long)Math.Pow(srcBase, i);
            }

            return id;
        }

        /// <summary>
        /// Returns a string representation of this batch id.
        /// </summary>
        /// <returns>A string representation of this batch id.</returns>
        public override string ToString()
        {
            return ToBase(this.Id, ALPHANUMERIC);
        }

        /// <summary>
        /// Tests equality of this instance to the given instance.
        /// </summary>
        /// <param name="obj">The object to test equality of.</param>
        /// <returns>True, if the given object is equal to this object.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var otherType = obj as BatchId;
            if (otherType == null)
            {
                return false;
            }
            return this.Id == otherType.Id;
        }

        /// <summary>
        /// Returns a hash code of this id.
        /// </summary>
        /// <returns>A hash code of this instance.</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

    }
}
