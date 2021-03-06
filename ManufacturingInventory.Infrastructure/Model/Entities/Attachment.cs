﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManufacturingInventory.Infrastructure.Model.Entities {
    public class Attachment {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? ValidThough { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string SourceReference { get; set; }
        public string FileReference { get; set; }
        public string Extension { get; set; }
        public bool Expires { get; set; }
        public byte[] RowVersion { get; set; }

        public int? PartId { get; set; }
        public virtual Part Part { get; set; }

        public int? DistributorId { get; set; }
        public virtual Distributor Distributor { get; set; }

        public int? ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }

        public int? PriceId { get; set; }
        public virtual Price Price { get; set; }

        public int? PartInstanceId { get; set; }
        public virtual PartInstance PartInstance { get; set; }


        public Attachment() {

        }

        public Attachment(DateTime now, string name, string source) {
            this.Created = now;
            this.Name = name;
            this.SourceReference = source;
        }

        public void Set(Attachment entity) {
            this.Created = entity.Created;
            this.ValidThough = entity.ValidThough;
            this.Name = entity.Name;
            this.DisplayName = entity.DisplayName;
            this.Description = entity.Description;
            this.SourceReference = entity.SourceReference;
            this.FileReference = entity.FileReference;
            this.Extension = entity.FileReference;
            this.Expires = entity.Expires;

            this.PartId = entity.PartId;
            this.ManufacturerId = entity.ManufacturerId;
            this.PartInstanceId = entity.PartInstanceId;
            this.PriceId = entity.PriceId;
            this.DistributorId = entity.DistributorId;
        }
    }
}
