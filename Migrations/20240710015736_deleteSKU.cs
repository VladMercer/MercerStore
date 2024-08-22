using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercerStore.Migrations
{
    /// <inheritdoc />
    public partial class deleteSKU : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SKU",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "Products",
                type: "text",
                nullable: true);
        }
    }
}
