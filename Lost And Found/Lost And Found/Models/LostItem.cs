using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System;

namespace Lost_And_Found.Models
{
    public class LostItem
    {
        [Id]
        public string Id { get; set; }
        public Timestamp TimePosted { get; set; }
        public string Description { get; set; }
        public string LastSeen { get; set; }
        public string Category { get; set; }
    }
}