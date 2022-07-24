namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddChatTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatRoom",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastMessageID = c.Int(nullable: false),
                        RoomName = c.String(),
                        ChatRoomType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ChatRoomUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OwnerUserID = c.Int(nullable: false),
                        ChatRoomID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ChatRoom", t => t.ChatRoomID, cascadeDelete: true)
                .Index(t => t.ChatRoomID);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ChatRoomID = c.Int(nullable: false),
                        OwnerUserID = c.Int(nullable: false),
                        Content = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ChatRoom", t => t.ChatRoomID, cascadeDelete: true)
                .Index(t => t.ChatRoomID);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatNotification", "ID", "dbo.Notification");
            DropForeignKey("dbo.Notification", "OwnerUserID", "dbo.User");
            DropForeignKey("dbo.Message", "ChatRoomID", "dbo.ChatRoom");
            DropForeignKey("dbo.ChatRoomUser", "ChatRoomID", "dbo.ChatRoom");
            DropIndex("dbo.ChatNotification", new[] { "ID" });
            DropIndex("dbo.Message", new[] { "ChatRoomID" });
            DropIndex("dbo.ChatRoomUser", new[] { "ChatRoomID" });
            DropIndex("dbo.Notification", new[] { "OwnerUserID" });
            DropTable("dbo.Message");
            DropTable("dbo.ChatRoomUser");
            DropTable("dbo.ChatRoom");
        }
    }
}
