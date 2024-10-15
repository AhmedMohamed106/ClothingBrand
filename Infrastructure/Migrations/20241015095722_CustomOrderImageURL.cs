using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingBrand.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CustomOrderImageURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "customClothingOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "customClothingOrders");
        }
    }
}
