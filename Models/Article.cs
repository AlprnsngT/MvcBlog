namespace MvcBlog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("Article")]
    public partial class Article
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Article()
        {
            Comments = new HashSet<Comment>();
            Tickets = new HashSet<Ticket>();
        }

        public int ArticleId { get; set; }

        [StringLength(500)]
        public string Header { get; set; }

        [UIHint("tinymce_full_compressed"), AllowHtml]
        public string Content { get; set; }

        [StringLength(500)]
        public string Foto { get; set; }

        public DateTime? Date { get; set; }

        public int? CategoryId { get; set; }

        public int? UserId { get; set; }

        public int? Read { get; set; }

        public virtual Category Category { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
