using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects.DataClasses;

namespace Checkout.Infrastructure.Data.Base
{
    [Serializable]
    public abstract class BaseEntity : IEntityWithKey
    {
        public EntityKey EntityKey { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }
    }

    public class BaseGuidEntity : BaseEntity
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
    }

    public class BaseIdEntity : BaseEntity
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }

    public class BaseConcurrencyGuidEntity : BaseGuidEntity
    {
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }

    public class BaseConcurrencyIdEntity : BaseIdEntity
    {
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
