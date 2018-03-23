﻿using CQELight.DAL.Attributes;
using CQELight.DAL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CQELight.DAL.EFCore.Integration.Tests
{
    [Table("Blog")]
    class Blog : DbEntity
    {
        [Index(true), Column("BlogURL"), Required]
        public virtual string Url { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<HyperLinkBlog> HyperLinks { get; set; } = new List<HyperLinkBlog>();

        [KeyStorageOf(nameof(AzureLocation))]
        public string AzureCountry { get; set; }
        [KeyStorageOf(nameof(AzureLocation))]
        public string AzureDataCenter { get; set; }
        [ForeignKey]
        public AzureLocation AzureLocation { get; set; }
    }

    [Table("AzureLocation")]
    [ComposedKey(nameof(Country), nameof(DataCenter))]
    class AzureLocation : ComposedKeyDbEntity
    {
        public string Country { get; set; }
        public string DataCenter { get; set; }
    }

    [Table("Hyperlinks")]
    class HyperLinkBlog : CustomKeyDbEntity
    {
        [PrimaryKey("Hyperlink"), MaxLength(1024)]
        public string Hyperlink { get; set; }
        [ForeignKey, Required]
        public Blog Blog { get; set; }
        [KeyStorageOf(nameof(Blog))]
        protected Guid Blog_Id { get; set; }
        public override bool IsKeySet() => !string.IsNullOrWhiteSpace(Hyperlink);
        public override object GetKeyValue() => Hyperlink;

    }
    [Table("User")]
    class User : DbEntity
    {
        [Column("Name")]
        public virtual string Name { get; set; }
        [Column("LastName")]
        public virtual string LastName { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }

    [Table("Post")]
    class Post : DbEntity
    {
        [MaxLength(65536), Column("Content"), Required]
        public virtual string Content { get; set; }
        [MaxLength(2048), Column("ShortAccess"), Index(true)]
        public virtual string QuickUrl { get; set; }
        [DefaultValue(1), Column("Version")]
        public virtual int Version { get; set; }
        [DefaultValue(true), Column("Published")]
        public virtual bool Published { get; set; }
        [Column("PublicationDate")]
        public virtual DateTime? PublicationDate { get; set; }
        public ICollection<PostTag> Tags { get; set; } = new List<PostTag>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        [ForeignKey]
        [NotNaviguable(NavigationMode.Update)]
        public User Writer { get; set; }
        [KeyStorageOf(nameof(Writer))]
        protected Guid? Writer_Id { get; set; }
        [ForeignKey(DeleteCascade = true), Required]
        public Blog Blog { get; set; }
        [KeyStorageOf(nameof(Blog))]
        protected Guid Blog_Id { get; set; }
    }


    [Table("PostTag")]
    [ComposedKey(nameof(Post), nameof(Tag))]
    class PostTag : ComposedKeyDbEntity
    {
        [KeyStorageOf(nameof(Post))]
        protected Guid Post_Id { get; set; }
        [ForeignKey, Required]
        public Post Post { get; protected set; }
        [KeyStorageOf(nameof(Tag))]
        protected Guid Tag_Id { get; set; }
        [ForeignKey, Required]
        public Tag Tag { get; protected set; }

        protected PostTag() { }

        public PostTag(Post post, Tag tag)
        {
            Post = post;
            Tag = tag;
        }
        public override bool IsKeySet() => Post != null && Tag != null;
        public override object GetKeyValue() => new { Post = Post, Tag = Tag };
    }

    [Table("Tag")]
    class Tag : DbEntity
    {
        [Index(true)]
        [Column("Value")]
        public virtual string Value { get; set; }
        public virtual ICollection<PostTag> Posts { get; set; } = new List<PostTag>();
    }

    [Table("Comment")]
    [ComplexIndex(new[] { nameof(Post), nameof(Owner), nameof(Value) }, false)]
    class Comment : DbEntity
    {
        [Column("Value")]
        public virtual string Value { get; set; }
        [ForeignKey, Required]
        [NotNaviguable(NavigationMode.Update)]
        protected virtual User Owner { get; set; }
        [KeyStorageOf(nameof(Owner))]
        protected virtual Guid Owner_Id { get; set; }
        [ForeignKey, Required]
        protected virtual Post Post { get; set; }
        [KeyStorageOf(nameof(Post))]
        protected virtual Guid Post_Id { get; set; }

        protected Comment() { }
        public Comment(string value, User owner, Post post)
        {
            Value = value;
            Owner = owner;
            Post = post;
        }
    }
}
