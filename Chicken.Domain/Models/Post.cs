using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Chicken.Domain.Tools;
using Newtonsoft.Json;

namespace Chicken.Domain.Models
{
    [JsonObject]
    public class Post : Entity
    {
        [JsonProperty(PropertyName = "id")]
        public long PostId { get; set; }

        [JsonProperty(PropertyName = "date")]
        [JsonConverter(typeof(UnixTimeStampToDateTimeConverter))]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [NotMapped]
        [JsonProperty(PropertyName = "comments")]
        public dynamic CommentsObject { get; set; }

        [NotMapped]
        [JsonProperty(PropertyName = "likes")]
        public dynamic LikesObject { get; set; }

        public int CommentsCount { get; set; }

        public int LikesCount { get; set; }

        public string Avatar { get; set; }

        public bool IsSpam { get; set; }

        public bool IsNew { get; set; }

        public bool IsSuggestion { get; set; }

        [JsonProperty(PropertyName = "attachments")]
        public virtual ICollection<Attachment> Attachments { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual Account Account { get; set; }

        public virtual Group Group { get; set; }

        public int GroupId { get; set; }

        public int? AccountId { get; set; }

        public void UpdateLikesAndComments()
        {
            CommentsCount = CommentsObject.count;
            LikesCount = LikesObject.count;
        }

        public void SetGroup(Group group)
        {
            this.GroupId = group.Id;
        }
    }
}
