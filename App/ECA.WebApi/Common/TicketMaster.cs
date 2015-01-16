using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Common
{
    public static class TicketMaster
    {
        private class Ticket
        {
            public string Owner { get; private set; }
            public string Value { get; private set; }
            public DateTimeOffset Timestamp { get; private set; }

            public Ticket(string owner)
            {
                Value = Guid.NewGuid().ToString();
                Owner = owner;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public override string ToString()
            {
                return Value;
            }
        }

        private static List<Ticket> tickets = new List<Ticket>();

        public static string Create(string owner)
        {
            var ticket = new Ticket(owner);
            tickets.RemoveAll(t => t.Owner == owner);
            tickets.Add(ticket);
            return ticket.Value;
        }

        public static void Delete(string owner)
        {
            tickets.RemoveAll(t => t.Owner == owner);
        }

        public static bool IsValid(string value)
        {
            return !string.IsNullOrEmpty(value) && tickets.Exists(t => t.Value == value);
        }
    }
}