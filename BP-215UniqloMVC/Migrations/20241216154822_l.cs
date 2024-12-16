using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BP_215UniqloMVC.Migrations
{
    /// <inheritdoc />
    public partial class l : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_AspNetUsers_UserId",
                table: "ProductComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_Products_ProductsId",
                table: "ProductComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRating_AspNetUsers_UserId",
                table: "ProductRating");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRating_Products_ProductsId",
                table: "ProductRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductRating",
                table: "ProductRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductComment",
                table: "ProductComment");

            migrationBuilder.RenameTable(
                name: "ProductRating",
                newName: "ProductRatings");

            migrationBuilder.RenameTable(
                name: "ProductComment",
                newName: "ProductComments");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRating_UserId",
                table: "ProductRatings",
                newName: "IX_ProductRatings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRating_ProductsId",
                table: "ProductRatings",
                newName: "IX_ProductRatings_ProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComment_UserId",
                table: "ProductComments",
                newName: "IX_ProductComments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComment_ProductsId",
                table: "ProductComments",
                newName: "IX_ProductComments_ProductsId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ProductComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "ProductComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductRatings",
                table: "ProductRatings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductComments",
                table: "ProductComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_AspNetUsers_UserId",
                table: "ProductComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_Products_ProductsId",
                table: "ProductComments",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRatings_AspNetUsers_UserId",
                table: "ProductRatings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRatings_Products_ProductsId",
                table: "ProductRatings",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_AspNetUsers_UserId",
                table: "ProductComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_Products_ProductsId",
                table: "ProductComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRatings_AspNetUsers_UserId",
                table: "ProductRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRatings_Products_ProductsId",
                table: "ProductRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductRatings",
                table: "ProductRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductComments",
                table: "ProductComments");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ProductComments");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "ProductComments");

            migrationBuilder.RenameTable(
                name: "ProductRatings",
                newName: "ProductRating");

            migrationBuilder.RenameTable(
                name: "ProductComments",
                newName: "ProductComment");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRatings_UserId",
                table: "ProductRating",
                newName: "IX_ProductRating_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRatings_ProductsId",
                table: "ProductRating",
                newName: "IX_ProductRating_ProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComments_UserId",
                table: "ProductComment",
                newName: "IX_ProductComment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComments_ProductsId",
                table: "ProductComment",
                newName: "IX_ProductComment_ProductsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductRating",
                table: "ProductRating",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductComment",
                table: "ProductComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_AspNetUsers_UserId",
                table: "ProductComment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_Products_ProductsId",
                table: "ProductComment",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRating_AspNetUsers_UserId",
                table: "ProductRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRating_Products_ProductsId",
                table: "ProductRating",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
