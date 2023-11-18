using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3.Domain.Entities
{
    public class S3BucketItem: IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
    }
}
