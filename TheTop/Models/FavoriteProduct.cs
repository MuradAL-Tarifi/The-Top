﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TheTop.Models
{
    public partial class FavoriteProduct
    {
        public int Id { get; set; }
        public bool IsFavorite { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Users User { get; set; }
    }
}