﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManufacturingInventory.Common.Model.Entities {

    public partial class Parameter  {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] RowVersion { get; set; }

        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }

        public virtual ICollection<InstanceParameter> InstanceParameters { get; set; }

        public Parameter() {
            this.InstanceParameters = new ObservableHashSet<InstanceParameter>();
        }

        public Parameter(string name, string description) : this() {
            this.Name = name;
            this.Description = description;
        }
    }
}
