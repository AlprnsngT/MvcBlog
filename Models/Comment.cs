namespace MvcBlog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        public int CommentId { get; set; }

        [StringLength(500)]
        public string Content { get; set; }

        public int? UserId { get; set; }

        public int? ArticleId { get; set; }

        public DateTime? Date { get; set; }

        public virtual Article Article { get; set; }

        public virtual User User { get; set; }
    }
}
