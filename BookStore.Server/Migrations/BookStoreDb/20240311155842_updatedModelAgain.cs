using System.Collections.Generic;
using BookStore.Server.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Server.Migrations.BookStoreDb
{
    /// <inheritdoc />
    public partial class updatedModelAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookModels_PurchasedBooks_PurchasedBooksId",
                table: "BookModels");

            migrationBuilder.DropIndex(
                name: "IX_BookModels_PurchasedBooksId",
                table: "BookModels");

            migrationBuilder.DropColumn(
                name: "PurchasedBooksId",
                table: "BookModels");

            migrationBuilder.AddColumn<List<BookModel>>(
                name: "Books",
                table: "PurchasedBooks",
                type: "jsonb[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Books",
                table: "PurchasedBooks");

            migrationBuilder.AddColumn<int>(
                name: "PurchasedBooksId",
                table: "BookModels",
                type: "integer",
                nullable: true);

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
    }
}
