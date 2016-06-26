using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace rest_API.Models
{
    public class rest_APIContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public rest_APIContext() : base("name=rest_APIContext")
        {
        }

        public System.Data.Entity.DbSet<rest_API.Models.Event> Events { get; set; }

        public System.Data.Entity.DbSet<rest_API.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<rest_API.Models.Event_Going> Event_Going { get; set; }

        public System.Data.Entity.DbSet<rest_API.Models.Friend> Friends { get; set; }

        public System.Data.Entity.DbSet<rest_API.Models.Invite> Invites { get; set; }
    }
}
