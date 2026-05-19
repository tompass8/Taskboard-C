using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBoard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBoardDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Workspaces_WorkspaceId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Card_List_ListId",
                table: "Card");

            migrationBuilder.DropForeignKey(
                name: "FK_Label_Boards_BoardId",
                table: "Label");

            migrationBuilder.DropForeignKey(
                name: "FK_List_Boards_BoardId",
                table: "List");

            migrationBuilder.DropForeignKey(
                name: "FK_Workspaces_Users_OwnerId",
                table: "Workspaces");

            migrationBuilder.DropTable(
                name: "CardLabel");

            migrationBuilder.DropTable(
                name: "CardUser");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "WorkspaceMembers");

            migrationBuilder.DropIndex(
                name: "IX_Workspaces_OwnerId",
                table: "Workspaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_List",
                table: "List");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Label",
                table: "Label");

            migrationBuilder.DropIndex(
                name: "IX_Label_BoardId",
                table: "Label");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Card",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Label");

            migrationBuilder.RenameTable(
                name: "List",
                newName: "Lists");

            migrationBuilder.RenameTable(
                name: "Label",
                newName: "Labels");

            migrationBuilder.RenameTable(
                name: "Card",
                newName: "Cards");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Workspaces",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "WorkspaceId",
                table: "Boards",
                newName: "WorkspaceID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Boards",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Boards",
                newName: "Title");

            migrationBuilder.RenameIndex(
                name: "IX_Boards_WorkspaceId",
                table: "Boards",
                newName: "IX_Boards_WorkspaceID");

            migrationBuilder.RenameColumn(
                name: "BoardId",
                table: "Lists",
                newName: "BoardID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Lists",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Lists",
                newName: "Title");

            migrationBuilder.RenameIndex(
                name: "IX_List_BoardId",
                table: "Lists",
                newName: "IX_Lists_BoardID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Labels",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ListId",
                table: "Cards",
                newName: "ListID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Cards",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Cards",
                newName: "UpdatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Card_ListId",
                table: "Cards",
                newName: "IX_Cards_ListID");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Workspaces",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceID",
                table: "Boards",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Boards",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Boards",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerID",
                table: "Boards",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Boards",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BoardID",
                table: "Lists",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Lists",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Lists",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Lists",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Labels",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "CardID",
                table: "Labels",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ListID",
                table: "Cards",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Cards",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedUserID",
                table: "Cards",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Cards",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lists",
                table: "Lists",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Labels",
                table: "Labels",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cards",
                table: "Cards",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Notification = table.Column<string>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CardID = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Activities_Cards_CardID",
                        column: x => x.CardID,
                        principalTable: "Cards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activities_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoardMemberships",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    BoardID = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardMemberships", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BoardMemberships_Boards_BoardID",
                        column: x => x.BoardID,
                        principalTable: "Boards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardMemberships_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkspaceMemberships",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    WorkspaceID = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceMemberships", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkspaceMemberships_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkspaceMemberships_Workspaces_WorkspaceID",
                        column: x => x.WorkspaceID,
                        principalTable: "Workspaces",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boards_OwnerID",
                table: "Boards",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Labels_CardID",
                table: "Labels",
                column: "CardID");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_AssignedUserID",
                table: "Cards",
                column: "AssignedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CardID",
                table: "Activities",
                column: "CardID");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_UserID",
                table: "Activities",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_BoardMemberships_BoardID_UserID",
                table: "BoardMemberships",
                columns: new[] { "BoardID", "UserID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoardMemberships_UserID",
                table: "BoardMemberships",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceMemberships_UserID",
                table: "WorkspaceMemberships",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceMemberships_WorkspaceID_UserID",
                table: "WorkspaceMemberships",
                columns: new[] { "WorkspaceID", "UserID" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Users_OwnerID",
                table: "Boards",
                column: "OwnerID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Workspaces_WorkspaceID",
                table: "Boards",
                column: "WorkspaceID",
                principalTable: "Workspaces",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Lists_ListID",
                table: "Cards",
                column: "ListID",
                principalTable: "Lists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Users_AssignedUserID",
                table: "Cards",
                column: "AssignedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Labels_Cards_CardID",
                table: "Labels",
                column: "CardID",
                principalTable: "Cards",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lists_Boards_BoardID",
                table: "Lists",
                column: "BoardID",
                principalTable: "Boards",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Users_OwnerID",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Workspaces_WorkspaceID",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Lists_ListID",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Users_AssignedUserID",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Labels_Cards_CardID",
                table: "Labels");

            migrationBuilder.DropForeignKey(
                name: "FK_Lists_Boards_BoardID",
                table: "Lists");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "BoardMemberships");

            migrationBuilder.DropTable(
                name: "WorkspaceMemberships");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Boards_OwnerID",
                table: "Boards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lists",
                table: "Lists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Labels",
                table: "Labels");

            migrationBuilder.DropIndex(
                name: "IX_Labels_CardID",
                table: "Labels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cards",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_AssignedUserID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "CardID",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "AssignedUserID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Cards");

            migrationBuilder.RenameTable(
                name: "Lists",
                newName: "List");

            migrationBuilder.RenameTable(
                name: "Labels",
                newName: "Label");

            migrationBuilder.RenameTable(
                name: "Cards",
                newName: "Card");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Workspaces",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "WorkspaceID",
                table: "Boards",
                newName: "WorkspaceId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Boards",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Boards",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Boards_WorkspaceID",
                table: "Boards",
                newName: "IX_Boards_WorkspaceId");

            migrationBuilder.RenameColumn(
                name: "BoardID",
                table: "List",
                newName: "BoardId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "List",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "List",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Lists_BoardID",
                table: "List",
                newName: "IX_List_BoardId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Label",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ListID",
                table: "Card",
                newName: "ListId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Card",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Card",
                newName: "DueDate");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_ListID",
                table: "Card",
                newName: "IX_Card_ListId");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Workspaces",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Workspaces",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Workspaces",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkspaceId",
                table: "Boards",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Boards",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BoardId",
                table: "List",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "List",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Label",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<Guid>(
                name: "BoardId",
                table: "Label",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "ListId",
                table: "Card",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Card",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_List",
                table: "List",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Label",
                table: "Label",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Card",
                table: "Card",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CardLabel",
                columns: table => new
                {
                    CardsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LabelsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardLabel", x => new { x.CardsId, x.LabelsId });
                    table.ForeignKey(
                        name: "FK_CardLabel_Card_CardsId",
                        column: x => x.CardsId,
                        principalTable: "Card",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardLabel_Label_LabelsId",
                        column: x => x.LabelsId,
                        principalTable: "Label",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardUser",
                columns: table => new
                {
                    AssignedCardsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssigneesId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardUser", x => new { x.AssignedCardsId, x.AssigneesId });
                    table.ForeignKey(
                        name: "FK_CardUser_Card_AssignedCardsId",
                        column: x => x.AssignedCardsId,
                        principalTable: "Card",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardUser_Users_AssigneesId",
                        column: x => x.AssigneesId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CardId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Card_CardId",
                        column: x => x.CardId,
                        principalTable: "Card",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkspaceMembers",
                columns: table => new
                {
                    MembersId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkspacesId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceMembers", x => new { x.MembersId, x.WorkspacesId });
                    table.ForeignKey(
                        name: "FK_WorkspaceMembers_Users_MembersId",
                        column: x => x.MembersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkspaceMembers_Workspaces_WorkspacesId",
                        column: x => x.WorkspacesId,
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workspaces_OwnerId",
                table: "Workspaces",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Label_BoardId",
                table: "Label",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLabel_LabelsId",
                table: "CardLabel",
                column: "LabelsId");

            migrationBuilder.CreateIndex(
                name: "IX_CardUser_AssigneesId",
                table: "CardUser",
                column: "AssigneesId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CardId",
                table: "Comment",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId",
                table: "Comment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceMembers_WorkspacesId",
                table: "WorkspaceMembers",
                column: "WorkspacesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Workspaces_WorkspaceId",
                table: "Boards",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Card_List_ListId",
                table: "Card",
                column: "ListId",
                principalTable: "List",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Label_Boards_BoardId",
                table: "Label",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_List_Boards_BoardId",
                table: "List",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workspaces_Users_OwnerId",
                table: "Workspaces",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
