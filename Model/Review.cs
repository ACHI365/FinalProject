using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinalProject.Model.Relations;

namespace FinalProject.Model
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }

        public string ReviewName { get; set; }
        public Group Group { get; set; }
        public string ReviewText { get; set; }
        public double Grade { get; set; }
        public DateTime CreationTime { get; set; } 
        public string ImageUrl { get; set; }
        public double ReviewScore { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int PieceId { get; set; }
        public Piece Piece { get; set; }

        public List<Like> Likes { get; set; } = new List<Like>();
        public List<ReviewTag> ReviewTags { get; set; } = new List<ReviewTag>();
    }
}
