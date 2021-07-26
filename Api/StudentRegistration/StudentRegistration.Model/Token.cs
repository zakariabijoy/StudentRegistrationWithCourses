using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.Model
{
    public class Token
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime ExpiryTime { get; set; }

    }
}
