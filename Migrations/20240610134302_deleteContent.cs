using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercerStore.Migrations
{
    /// <inheritdoc />
    public partial class deleteContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseDetails_Products_ProductId",
                table: "CaseDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CoolingSystemDetails_Products_ProductId",
                table: "CoolingSystemDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MotherboardDetails_Products_ProductId",
                table: "MotherboardDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PowerSupplyDetails_Products_ProductId",
                table: "PowerSupplyDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessorDetails_Products_ProductId",
                table: "ProcessorDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_Products_ProductId",
                table: "ProductVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_UserId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Products_ProductId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoCardDetails_Products_ProductId",
                table: "VideoCardDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoCardDetails",
                table: "VideoCardDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariants",
                table: "ProductVariants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessorDetails",
                table: "ProcessorDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PowerSupplyDetails",
                table: "PowerSupplyDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MotherboardDetails",
                table: "MotherboardDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoolingSystemDetails",
                table: "CoolingSystemDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseDetails",
                table: "CaseDetails");

            migrationBuilder.RenameTable(
                name: "VideoCardDetails",
                newName: "VideoCardDetail");

            migrationBuilder.RenameTable(
                name: "Ratings",
                newName: "Rating");

            migrationBuilder.RenameTable(
                name: "ProductVariants",
                newName: "ProductVariant");

            migrationBuilder.RenameTable(
                name: "ProcessorDetails",
                newName: "ProcessorDetail");

            migrationBuilder.RenameTable(
                name: "PowerSupplyDetails",
                newName: "PowerSupplyDetail");

            migrationBuilder.RenameTable(
                name: "MotherboardDetails",
                newName: "MotherboardDetail");

            migrationBuilder.RenameTable(
                name: "CoolingSystemDetails",
                newName: "CoolingSystemDetail");

            migrationBuilder.RenameTable(
                name: "CaseDetails",
                newName: "CaseDetail");

            migrationBuilder.RenameIndex(
                name: "IX_VideoCardDetails_ProductId",
                table: "VideoCardDetail",
                newName: "IX_VideoCardDetail_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_UserId",
                table: "Rating",
                newName: "IX_Rating_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_ProductId",
                table: "Rating",
                newName: "IX_Rating_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariant",
                newName: "IX_ProductVariant_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessorDetails_ProductId",
                table: "ProcessorDetail",
                newName: "IX_ProcessorDetail_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PowerSupplyDetails_ProductId",
                table: "PowerSupplyDetail",
                newName: "IX_PowerSupplyDetail_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_MotherboardDetails_ProductId",
                table: "MotherboardDetail",
                newName: "IX_MotherboardDetail_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CoolingSystemDetails_ProductId",
                table: "CoolingSystemDetail",
                newName: "IX_CoolingSystemDetail_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseDetails_ProductId",
                table: "CaseDetail",
                newName: "IX_CaseDetail_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoCardDetail",
                table: "VideoCardDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rating",
                table: "Rating",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariant",
                table: "ProductVariant",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessorDetail",
                table: "ProcessorDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PowerSupplyDetail",
                table: "PowerSupplyDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MotherboardDetail",
                table: "MotherboardDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoolingSystemDetail",
                table: "CoolingSystemDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseDetail",
                table: "CaseDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseDetail_Products_ProductId",
                table: "CaseDetail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoolingSystemDetail_Products_ProductId",
                table: "CoolingSystemDetail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MotherboardDetail_Products_ProductId",
                table: "MotherboardDetail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PowerSupplyDetail_Products_ProductId",
                table: "PowerSupplyDetail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessorDetail_Products_ProductId",
                table: "ProcessorDetail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariant_Products_ProductId",
                table: "ProductVariant",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_AspNetUsers_UserId",
                table: "Rating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Products_ProductId",
                table: "Rating",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoCardDetail_Products_ProductId",
                table: "VideoCardDetail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseDetail_Products_ProductId",
                table: "CaseDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_CoolingSystemDetail_Products_ProductId",
                table: "CoolingSystemDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_MotherboardDetail_Products_ProductId",
                table: "MotherboardDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_PowerSupplyDetail_Products_ProductId",
                table: "PowerSupplyDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessorDetail_Products_ProductId",
                table: "ProcessorDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariant_Products_ProductId",
                table: "ProductVariant");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_AspNetUsers_UserId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Products_ProductId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoCardDetail_Products_ProductId",
                table: "VideoCardDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoCardDetail",
                table: "VideoCardDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rating",
                table: "Rating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariant",
                table: "ProductVariant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessorDetail",
                table: "ProcessorDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PowerSupplyDetail",
                table: "PowerSupplyDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MotherboardDetail",
                table: "MotherboardDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoolingSystemDetail",
                table: "CoolingSystemDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseDetail",
                table: "CaseDetail");

            migrationBuilder.RenameTable(
                name: "VideoCardDetail",
                newName: "VideoCardDetails");

            migrationBuilder.RenameTable(
                name: "Rating",
                newName: "Ratings");

            migrationBuilder.RenameTable(
                name: "ProductVariant",
                newName: "ProductVariants");

            migrationBuilder.RenameTable(
                name: "ProcessorDetail",
                newName: "ProcessorDetails");

            migrationBuilder.RenameTable(
                name: "PowerSupplyDetail",
                newName: "PowerSupplyDetails");

            migrationBuilder.RenameTable(
                name: "MotherboardDetail",
                newName: "MotherboardDetails");

            migrationBuilder.RenameTable(
                name: "CoolingSystemDetail",
                newName: "CoolingSystemDetails");

            migrationBuilder.RenameTable(
                name: "CaseDetail",
                newName: "CaseDetails");

            migrationBuilder.RenameIndex(
                name: "IX_VideoCardDetail_ProductId",
                table: "VideoCardDetails",
                newName: "IX_VideoCardDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_UserId",
                table: "Ratings",
                newName: "IX_Ratings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_ProductId",
                table: "Ratings",
                newName: "IX_Ratings_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariant_ProductId",
                table: "ProductVariants",
                newName: "IX_ProductVariants_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessorDetail_ProductId",
                table: "ProcessorDetails",
                newName: "IX_ProcessorDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PowerSupplyDetail_ProductId",
                table: "PowerSupplyDetails",
                newName: "IX_PowerSupplyDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_MotherboardDetail_ProductId",
                table: "MotherboardDetails",
                newName: "IX_MotherboardDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CoolingSystemDetail_ProductId",
                table: "CoolingSystemDetails",
                newName: "IX_CoolingSystemDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseDetail_ProductId",
                table: "CaseDetails",
                newName: "IX_CaseDetails_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoCardDetails",
                table: "VideoCardDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariants",
                table: "ProductVariants",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessorDetails",
                table: "ProcessorDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PowerSupplyDetails",
                table: "PowerSupplyDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MotherboardDetails",
                table: "MotherboardDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoolingSystemDetails",
                table: "CoolingSystemDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseDetails",
                table: "CaseDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseDetails_Products_ProductId",
                table: "CaseDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoolingSystemDetails_Products_ProductId",
                table: "CoolingSystemDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MotherboardDetails_Products_ProductId",
                table: "MotherboardDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PowerSupplyDetails_Products_ProductId",
                table: "PowerSupplyDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessorDetails_Products_ProductId",
                table: "ProcessorDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_Products_ProductId",
                table: "ProductVariants",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_UserId",
                table: "Ratings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Products_ProductId",
                table: "Ratings",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoCardDetails_Products_ProductId",
                table: "VideoCardDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
