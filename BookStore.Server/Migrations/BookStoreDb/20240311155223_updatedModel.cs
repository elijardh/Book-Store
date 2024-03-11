using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookStore.Server.Migrations.BookStoreDb
{
    /// <inheritdoc />
    public partial class updatedModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchasedBooksId",
                table: "BookModels",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PurchasedBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedBooks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookModels_PurchasedBooksId",
                table: "BookModels",
                column: "PurchasedBooksId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookModels_PurchasedBooks_PurchasedBooksId",
                table: "BookModels",
                column: "PurchasedBooksId",
                principalTable: "PurchasedBooks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookModels_PurchasedBooks_PurchasedBooksId",
                table: "BookModels");

            migrationBuilder.DropTable(
                name: "PurchasedBooks");

            migrationBuilder.DropIndex(
                name: "IX_BookModels_PurchasedBooksId",
                table: "BookModels");

            migrationBuilder.DropColumn(
                name: "PurchasedBooksId",
                table: "BookModels");
        }
    }
}
