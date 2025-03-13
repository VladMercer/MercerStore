using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercerStore.Web.Migrations
{
    /// <inheritdoc />
    public partial class updateProductPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FixedDiscountPrice",
                table: "ProductPricings",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixedDiscountPrice",
                table: "ProductPricings");
        }
    }
}
