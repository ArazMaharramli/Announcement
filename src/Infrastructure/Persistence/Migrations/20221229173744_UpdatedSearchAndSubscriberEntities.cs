using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSearchAndSubscriberEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Searches_Subscribers_SubscriberId",
                table: "Searches");

            migrationBuilder.DropIndex(
                name: "IX_Searches_SubscriberId",
                table: "Searches");

            migrationBuilder.DropColumn(
                name: "SubscriberId",
                table: "Searches");

            migrationBuilder.CreateTable(
                name: "SearchSubscriber",
                columns: table => new
                {
                    SearchesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubscribersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchSubscriber", x => new { x.SearchesId, x.SubscribersId });
                    table.ForeignKey(
                        name: "FK_SearchSubscriber_Searches_SearchesId",
                        column: x => x.SearchesId,
                        principalTable: "Searches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SearchSubscriber_Subscribers_SubscribersId",
                        column: x => x.SubscribersId,
                        principalTable: "Subscribers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SearchSubscriber_SubscribersId",
                table: "SearchSubscriber",
                column: "SubscribersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchSubscriber");

            migrationBuilder.AddColumn<string>(
                name: "SubscriberId",
                table: "Searches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Searches_SubscriberId",
                table: "Searches",
                column: "SubscriberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Searches_Subscribers_SubscriberId",
                table: "Searches",
                column: "SubscriberId",
                principalTable: "Subscribers",
                principalColumn: "Id");
        }
    }
}
