using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MercerStore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserImgUrl = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LogoImgUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppUserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandCategory",
                columns: table => new
                {
                    BrandsId = table.Column<int>(type: "integer", nullable: false),
                    CategoriesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandCategory", x => new { x.BrandsId, x.CategoriesId });
                    table.ForeignKey(
                        name: "FK_BrandCategory_Brands_BrandsId",
                        column: x => x.BrandsId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandCategory_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    MainImageUrl = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    BrandId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartProducts_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Model = table.Column<string>(type: "text", nullable: false),
                    ManufacturerCode = table.Column<string>(type: "text", nullable: false),
                    CaseType = table.Column<string>(type: "text", nullable: false),
                    MotherboardOrientation = table.Column<string>(type: "text", nullable: true),
                    Length = table.Column<string>(type: "text", nullable: false),
                    Width = table.Column<string>(type: "text", nullable: false),
                    Height = table.Column<string>(type: "text", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    PrimaryColor = table.Column<string>(type: "text", nullable: false),
                    Material = table.Column<string>(type: "text", nullable: false),
                    MetalThickness = table.Column<string>(type: "text", nullable: false),
                    SidePanelWindow = table.Column<bool>(type: "boolean", nullable: false),
                    WindowMaterial = table.Column<string>(type: "text", nullable: true),
                    FrontPanelMaterial = table.Column<string>(type: "text", nullable: true),
                    LightingType = table.Column<string>(type: "text", nullable: true),
                    LightingColor = table.Column<string>(type: "text", nullable: true),
                    LightingSource = table.Column<string>(type: "text", nullable: true),
                    LightingConnector = table.Column<string>(type: "text", nullable: true),
                    LightingControl = table.Column<string>(type: "text", nullable: true),
                    CompatibleMotherboardFormFactors = table.Column<string>(type: "text", nullable: true),
                    CompatiblePowerSupplyFormFactors = table.Column<string>(type: "text", nullable: true),
                    PowerSupplyPlacement = table.Column<string>(type: "text", nullable: true),
                    MaxPowerSupplyLength = table.Column<string>(type: "text", nullable: true),
                    HorizontalExpansionSlotsCount = table.Column<int>(type: "integer", nullable: false),
                    VerticalExpansionSlotsCount = table.Column<int>(type: "integer", nullable: false),
                    MaxGpuLength = table.Column<string>(type: "text", nullable: true),
                    MaxCpuCoolerHeight = table.Column<string>(type: "text", nullable: true),
                    Internal2_5DriveBaysCount = table.Column<int>(type: "integer", nullable: false),
                    Internal3_5DriveBaysCount = table.Column<int>(type: "integer", nullable: false),
                    External3_5DriveBaysCount = table.Column<int>(type: "integer", nullable: false),
                    DriveBays5_25Count = table.Column<int>(type: "integer", nullable: false),
                    IncludedFans = table.Column<string>(type: "text", nullable: true),
                    FrontFanSupport = table.Column<string>(type: "text", nullable: true),
                    RearFanSupport = table.Column<string>(type: "text", nullable: true),
                    TopFanSupport = table.Column<string>(type: "text", nullable: true),
                    BottomFanSupport = table.Column<string>(type: "text", nullable: true),
                    LiquidCoolingSupport = table.Column<bool>(type: "boolean", nullable: false),
                    FrontRadiatorSizes = table.Column<string>(type: "text", nullable: true),
                    TopRadiatorSizes = table.Column<string>(type: "text", nullable: true),
                    RearRadiatorSizes = table.Column<string>(type: "text", nullable: true),
                    IOPanelLocation = table.Column<string>(type: "text", nullable: true),
                    IOConnectors = table.Column<string>(type: "text", nullable: true),
                    BuiltInCardReader = table.Column<bool>(type: "boolean", nullable: false),
                    SidePanelFixationScrews = table.Column<bool>(type: "boolean", nullable: false),
                    CpuCoolerCutout = table.Column<bool>(type: "boolean", nullable: false),
                    CableManagementBehindMotherboardTray = table.Column<bool>(type: "boolean", nullable: false),
                    DustFilters = table.Column<bool>(type: "boolean", nullable: false),
                    BuiltInPowerSupply = table.Column<bool>(type: "boolean", nullable: false),
                    LowNoiseAntiVibrationCases = table.Column<bool>(type: "boolean", nullable: false),
                    Accessories = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoolingSystemDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Model = table.Column<string>(type: "text", nullable: false),
                    ManufacturerCode = table.Column<string>(type: "text", nullable: false),
                    SocketCompatibility = table.Column<string>(type: "text", nullable: false),
                    TDP = table.Column<int>(type: "integer", nullable: false),
                    ConstructionType = table.Column<string>(type: "text", nullable: false),
                    BaseMaterial = table.Column<string>(type: "text", nullable: false),
                    RadiatorMaterial = table.Column<string>(type: "text", nullable: false),
                    HeatPipesCount = table.Column<int>(type: "integer", nullable: false),
                    HeatPipeDiameter = table.Column<string>(type: "text", nullable: false),
                    NickelPlating = table.Column<bool>(type: "boolean", nullable: false),
                    RadiatorColor = table.Column<string>(type: "text", nullable: false),
                    FansIncludedCount = table.Column<int>(type: "integer", nullable: false),
                    MaxFansCount = table.Column<int>(type: "integer", nullable: false),
                    FanDimensions = table.Column<string>(type: "text", nullable: false),
                    FanColor = table.Column<string>(type: "text", nullable: false),
                    FanConnector = table.Column<string>(type: "text", nullable: false),
                    MaxRotationSpeed = table.Column<int>(type: "integer", nullable: false),
                    MinRotationSpeed = table.Column<int>(type: "integer", nullable: false),
                    RotationSpeedControl = table.Column<string>(type: "text", nullable: false),
                    MaxAirflow = table.Column<double>(type: "double precision", nullable: false),
                    MaxNoiseLevel = table.Column<double>(type: "double precision", nullable: false),
                    RatedCurrent = table.Column<double>(type: "double precision", nullable: false),
                    RatedVoltage = table.Column<int>(type: "integer", nullable: false),
                    BearingType = table.Column<string>(type: "text", nullable: false),
                    ThermalPasteIncluded = table.Column<bool>(type: "boolean", nullable: false),
                    LightingType = table.Column<string>(type: "text", nullable: true),
                    MountingKit = table.Column<string>(type: "text", nullable: true),
                    Height = table.Column<string>(type: "text", nullable: true),
                    Width = table.Column<string>(type: "text", nullable: true),
                    Length = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<double>(type: "double precision", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoolingSystemDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoolingSystemDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MotherboardDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Series = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    ReleaseYear = table.Column<int>(type: "integer", nullable: false),
                    FormFactor = table.Column<string>(type: "text", nullable: false),
                    Height = table.Column<string>(type: "text", nullable: false),
                    Width = table.Column<string>(type: "text", nullable: false),
                    Socket = table.Column<string>(type: "text", nullable: false),
                    Chipset = table.Column<string>(type: "text", nullable: false),
                    CompatibleIntelCores = table.Column<string>(type: "text", nullable: false),
                    MemoryType = table.Column<string>(type: "text", nullable: false),
                    MemoryFormFactor = table.Column<string>(type: "text", nullable: false),
                    MemorySlots = table.Column<int>(type: "integer", nullable: false),
                    MemoryChannels = table.Column<int>(type: "integer", nullable: false),
                    MaxMemoryVolume = table.Column<string>(type: "text", nullable: false),
                    MaxMemoryFrequency = table.Column<string>(type: "text", nullable: false),
                    PCIeVersion = table.Column<string>(type: "text", nullable: false),
                    PCIeSlots = table.Column<string>(type: "text", nullable: false),
                    SLICrossFireSupport = table.Column<bool>(type: "boolean", nullable: false),
                    SLICrossFireCards = table.Column<int>(type: "integer", nullable: false),
                    PCIeX1Slots = table.Column<int>(type: "integer", nullable: false),
                    NVMeSupport = table.Column<bool>(type: "boolean", nullable: true),
                    NVMePCIeVersion = table.Column<string>(type: "text", nullable: true),
                    M2Slots = table.Column<int>(type: "integer", nullable: true),
                    M2PCIeProcessorLines = table.Column<string>(type: "text", nullable: true),
                    SATAPorts = table.Column<int>(type: "integer", nullable: true),
                    SATA_RAIDSupport = table.Column<bool>(type: "boolean", nullable: true),
                    USBTypeAPorts = table.Column<string>(type: "text", nullable: true),
                    USBTypeCPort = table.Column<bool>(type: "boolean", nullable: true),
                    VideoOutputs = table.Column<string>(type: "text", nullable: true),
                    RJ45Ports = table.Column<int>(type: "integer", nullable: true),
                    AnalogAudioPorts = table.Column<int>(type: "integer", nullable: true),
                    SPDIFPort = table.Column<bool>(type: "boolean", nullable: true),
                    InternalUSBTypeAPorts = table.Column<string>(type: "text", nullable: true),
                    InternalUSBTypeCPort = table.Column<bool>(type: "boolean", nullable: true),
                    CPUFanPowerConnectors = table.Column<int>(type: "integer", nullable: true),
                    CaseFanPowerConnectors4Pin = table.Column<int>(type: "integer", nullable: true),
                    CaseFanPowerConnectors3Pin = table.Column<int>(type: "integer", nullable: true),
                    ARGBConnector5V_D_G = table.Column<bool>(type: "boolean", nullable: true),
                    RGBConnector12V_G_R_B = table.Column<int>(type: "integer", nullable: true),
                    WirelessModuleM2 = table.Column<bool>(type: "boolean", nullable: true),
                    RS232Connector = table.Column<bool>(type: "boolean", nullable: true),
                    AudioScheme = table.Column<string>(type: "text", nullable: true),
                    AudioChipset = table.Column<string>(type: "text", nullable: true),
                    NetworkSpeed = table.Column<string>(type: "text", nullable: true),
                    NetworkAdapter = table.Column<string>(type: "text", nullable: true),
                    WiFiStandard = table.Column<bool>(type: "boolean", nullable: true),
                    BluetoothVersion = table.Column<string>(type: "text", nullable: true),
                    WirelessAdapter = table.Column<bool>(type: "boolean", nullable: false),
                    MainPowerConnector = table.Column<string>(type: "text", nullable: true),
                    CPUPowerConnector = table.Column<string>(type: "text", nullable: true),
                    PowerPhaseCount = table.Column<int>(type: "integer", nullable: false),
                    PassiveCooling = table.Column<string>(type: "text", nullable: false),
                    ActiveCooling = table.Column<bool>(type: "boolean", nullable: false),
                    OnBoardButtons = table.Column<bool>(type: "boolean", nullable: false),
                    BoardLighting = table.Column<bool>(type: "boolean", nullable: false),
                    LightingSyncSoftware = table.Column<string>(type: "text", nullable: true),
                    PackageContents = table.Column<string>(type: "text", nullable: false),
                    BoxLength = table.Column<string>(type: "text", nullable: true),
                    BoxWidth = table.Column<string>(type: "text", nullable: true),
                    BoxHeight = table.Column<string>(type: "text", nullable: true),
                    BoxWeight = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotherboardDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MotherboardDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PowerSupplyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Model = table.Column<string>(type: "text", nullable: false),
                    ManufacturerCode = table.Column<string>(type: "text", nullable: false),
                    Wattage = table.Column<int>(type: "integer", nullable: false),
                    FormFactor = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    ModularCables = table.Column<bool>(type: "boolean", nullable: false),
                    BraidedCables = table.Column<bool>(type: "boolean", nullable: false),
                    CableColor = table.Column<string>(type: "text", nullable: false),
                    LightingType = table.Column<string>(type: "text", nullable: false),
                    MainPowerConnector = table.Column<string>(type: "text", nullable: false),
                    CPUConnectors = table.Column<string>(type: "text", nullable: false),
                    GPUPowerConnectors = table.Column<string>(type: "text", nullable: false),
                    SataPowerConnectors = table.Column<int>(type: "integer", nullable: false),
                    MolexPowerConnectors = table.Column<int>(type: "integer", nullable: false),
                    MainCableLength = table.Column<string>(type: "text", nullable: false),
                    CPUCableLength = table.Column<string>(type: "text", nullable: false),
                    GPUCableLength = table.Column<string>(type: "text", nullable: false),
                    SataCableLength = table.Column<string>(type: "text", nullable: false),
                    MolexCableLength = table.Column<string>(type: "text", nullable: false),
                    PowerOn12VLine = table.Column<int>(type: "integer", nullable: false),
                    CurrentOn12VLine = table.Column<int>(type: "integer", nullable: false),
                    CurrentOn3_3VLine = table.Column<int>(type: "integer", nullable: false),
                    CurrentOn5VLine = table.Column<int>(type: "integer", nullable: false),
                    StandbyCurrent5V = table.Column<int>(type: "integer", nullable: false),
                    CurrentOnNegative12VLine = table.Column<int>(type: "integer", nullable: false),
                    InputVoltageRange = table.Column<string>(type: "text", nullable: false),
                    CoolingSystem = table.Column<string>(type: "text", nullable: false),
                    FanSize = table.Column<string>(type: "text", nullable: false),
                    FanControl = table.Column<string>(type: "text", nullable: false),
                    HybridMode = table.Column<bool>(type: "boolean", nullable: false),
                    PlusCertification = table.Column<string>(type: "text", nullable: false),
                    PowerFactorCorrection = table.Column<string>(type: "text", nullable: false),
                    StandardCompliance = table.Column<string>(type: "text", nullable: false),
                    ProtectionTechnologies = table.Column<string>(type: "text", nullable: false),
                    PowerCableIncluded = table.Column<bool>(type: "boolean", nullable: false),
                    PackageContents = table.Column<string>(type: "text", nullable: false),
                    Features = table.Column<string>(type: "text", nullable: false),
                    Length = table.Column<string>(type: "text", nullable: true),
                    Width = table.Column<string>(type: "text", nullable: true),
                    Height = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<double>(type: "double precision", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerSupplyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PowerSupplyDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Socket = table.Column<string>(type: "text", nullable: false),
                    ManufacturerCode = table.Column<string>(type: "text", nullable: false),
                    ReleaseYear = table.Column<int>(type: "integer", nullable: false),
                    CoolingSystemIncluded = table.Column<bool>(type: "boolean", nullable: false),
                    ThermalInterfaceIncluded = table.Column<bool>(type: "boolean", nullable: false),
                    TotalCores = table.Column<int>(type: "integer", nullable: false),
                    PerformanceCores = table.Column<int>(type: "integer", nullable: false),
                    EnergyEfficientCores = table.Column<int>(type: "integer", nullable: false),
                    MaxThreads = table.Column<int>(type: "integer", nullable: false),
                    L2Cache = table.Column<string>(type: "text", nullable: false),
                    L3Cache = table.Column<string>(type: "text", nullable: false),
                    TechnologyProcess = table.Column<string>(type: "text", nullable: false),
                    Core = table.Column<string>(type: "text", nullable: false),
                    BaseFrequency = table.Column<string>(type: "text", nullable: false),
                    MaxTurboFrequency = table.Column<string>(type: "text", nullable: false),
                    MemoryType = table.Column<string>(type: "text", nullable: true),
                    MaxSupportedMemory = table.Column<string>(type: "text", nullable: true),
                    MemoryChannels = table.Column<int>(type: "integer", nullable: true),
                    MemoryFrequency = table.Column<string>(type: "text", nullable: true),
                    ECCSupport = table.Column<bool>(type: "boolean", nullable: true),
                    TDP = table.Column<int>(type: "integer", nullable: false),
                    MaxTemperature = table.Column<int>(type: "integer", nullable: false),
                    IntegratedGraphics = table.Column<bool>(type: "boolean", nullable: true),
                    PCIeController = table.Column<string>(type: "text", nullable: true),
                    PCIeLanes = table.Column<int>(type: "integer", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessorDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Color = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RamDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Model = table.Column<string>(type: "text", nullable: false),
                    ManufacturerCode = table.Column<string>(type: "text", nullable: false),
                    MemoryType = table.Column<string>(type: "text", nullable: false),
                    MemoryFormFactor = table.Column<string>(type: "text", nullable: false),
                    TotalMemoryVolume = table.Column<int>(type: "integer", nullable: true),
                    ModuleMemoryVolume = table.Column<int>(type: "integer", nullable: true),
                    ModuleCount = table.Column<int>(type: "integer", nullable: true),
                    RegisteredMemory = table.Column<bool>(type: "boolean", nullable: true),
                    ECCMemory = table.Column<bool>(type: "boolean", nullable: true),
                    RankType = table.Column<string>(type: "text", nullable: false),
                    Frequency = table.Column<string>(type: "text", nullable: false),
                    XMPProfiles = table.Column<string>(type: "text", nullable: false),
                    CASLatency = table.Column<int>(type: "integer", nullable: true),
                    RASToCASDelay = table.Column<int>(type: "integer", nullable: true),
                    RowPrechargeDelay = table.Column<int>(type: "integer", nullable: true),
                    ActivateToPrechargeDelay = table.Column<int>(type: "integer", nullable: true),
                    RadiatorPresence = table.Column<bool>(type: "boolean", nullable: true),
                    RadiatorColor = table.Column<string>(type: "text", nullable: true),
                    Height = table.Column<string>(type: "text", nullable: true),
                    LowProfile = table.Column<bool>(type: "boolean", nullable: true),
                    Voltage = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RamDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RamDetail_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UserId1 = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    ReviewText = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorageDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Model = table.Column<string>(type: "text", nullable: false),
                    ManufacturerCode = table.Column<string>(type: "text", nullable: false),
                    CapacityGB = table.Column<int>(type: "integer", nullable: false),
                    FormFactor = table.Column<string>(type: "text", nullable: false),
                    PhysicalInterface = table.Column<string>(type: "text", nullable: false),
                    M2Key = table.Column<string>(type: "text", nullable: false),
                    NVMe = table.Column<bool>(type: "boolean", nullable: false),
                    Controller = table.Column<string>(type: "text", nullable: true),
                    BitsPerCell = table.Column<string>(type: "text", nullable: true),
                    MemoryStructure = table.Column<string>(type: "text", nullable: true),
                    DRAMBuffer = table.Column<bool>(type: "boolean", nullable: true),
                    DRAMBufferSizeMB = table.Column<int>(type: "integer", nullable: true),
                    MaxSequentialReadSpeed = table.Column<string>(type: "text", nullable: false),
                    MaxSequentialWriteSpeed = table.Column<string>(type: "text", nullable: false),
                    TBW = table.Column<string>(type: "text", nullable: true),
                    DWPD = table.Column<double>(type: "double precision", nullable: true),
                    RadiatorIncluded = table.Column<bool>(type: "boolean", nullable: true),
                    PowerConsumption = table.Column<string>(type: "text", nullable: true),
                    Length = table.Column<string>(type: "text", nullable: true),
                    Width = table.Column<string>(type: "text", nullable: true),
                    Thickness = table.Column<string>(type: "text", nullable: true),
                    WeightGrams = table.Column<int>(type: "integer", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorageDetail_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideoCardDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ManufacturerCode = table.Column<string>(type: "text", nullable: true),
                    IsMiningPurpose = table.Column<bool>(type: "boolean", nullable: false),
                    IsLHR = table.Column<bool>(type: "boolean", nullable: false),
                    GPU = table.Column<string>(type: "text", nullable: false),
                    Microarchitecture = table.Column<string>(type: "text", nullable: false),
                    FabricationProcess = table.Column<string>(type: "text", nullable: false),
                    BaseClockMHz = table.Column<int>(type: "integer", nullable: false),
                    BoostClockMHz = table.Column<int>(type: "integer", nullable: false),
                    ALUs = table.Column<int>(type: "integer", nullable: true),
                    TextureUnits = table.Column<int>(type: "integer", nullable: true),
                    ROPs = table.Column<int>(type: "integer", nullable: true),
                    RayTracingSupport = table.Column<bool>(type: "boolean", nullable: false),
                    RayTracingCores = table.Column<int>(type: "integer", nullable: true),
                    TensorCores = table.Column<int>(type: "integer", nullable: true),
                    MemorySizeGB = table.Column<int>(type: "integer", nullable: false),
                    MemoryType = table.Column<string>(type: "text", nullable: false),
                    MemoryBusWidth = table.Column<int>(type: "integer", nullable: false),
                    MemoryBandwidthGBps = table.Column<int>(type: "integer", nullable: false),
                    MemoryFrequencyMHz = table.Column<int>(type: "integer", nullable: false),
                    DisplayConnectors = table.Column<string>(type: "text", nullable: true),
                    HDMIVersion = table.Column<string>(type: "text", nullable: true),
                    DisplayPortVersion = table.Column<string>(type: "text", nullable: true),
                    MaxMonitors = table.Column<int>(type: "integer", nullable: true),
                    MaxResolution = table.Column<string>(type: "text", nullable: true),
                    Interface = table.Column<string>(type: "text", nullable: true),
                    ConnectionFormFactor = table.Column<string>(type: "text", nullable: true),
                    PCILanes = table.Column<int>(type: "integer", nullable: true),
                    PowerConnectors = table.Column<string>(type: "text", nullable: true),
                    RecommendedPSUWattage = table.Column<int>(type: "integer", nullable: true),
                    CoolingType = table.Column<string>(type: "text", nullable: true),
                    FanCount = table.Column<int>(type: "integer", nullable: true),
                    LengthMM = table.Column<int>(type: "integer", nullable: true),
                    WidthMM = table.Column<int>(type: "integer", nullable: true),
                    ThicknessMM = table.Column<int>(type: "integer", nullable: true),
                    WeightGrams = table.Column<int>(type: "integer", nullable: true),
                    IsRGB = table.Column<bool>(type: "boolean", nullable: true),
                    IsRGBSync = table.Column<bool>(type: "boolean", nullable: true),
                    HasLCDDisplay = table.Column<bool>(type: "boolean", nullable: true),
                    HasBIOSSwitch = table.Column<bool>(type: "boolean", nullable: true),
                    DimensionsBracket = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoCardDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoCardDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrandCategory_CategoriesId",
                table: "BrandCategory",
                column: "CategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_CartId",
                table: "CartProducts",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_ProductId",
                table: "CartProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseDetails_ProductId",
                table: "CaseDetails",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoolingSystemDetails_ProductId",
                table: "CoolingSystemDetails",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardDetails_ProductId",
                table: "MotherboardDetails",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PowerSupplyDetails_ProductId",
                table: "PowerSupplyDetails",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessorDetails_ProductId",
                table: "ProcessorDetails",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RamDetail_ProductId",
                table: "RamDetail",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ProductId",
                table: "Ratings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId1",
                table: "Ratings",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_StorageDetail_ProductId",
                table: "StorageDetail",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoCardDetails_ProductId",
                table: "VideoCardDetails",
                column: "ProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BrandCategory");

            migrationBuilder.DropTable(
                name: "CartProducts");

            migrationBuilder.DropTable(
                name: "CaseDetails");

            migrationBuilder.DropTable(
                name: "CoolingSystemDetails");

            migrationBuilder.DropTable(
                name: "MotherboardDetails");

            migrationBuilder.DropTable(
                name: "PowerSupplyDetails");

            migrationBuilder.DropTable(
                name: "ProcessorDetails");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropTable(
                name: "RamDetail");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "StorageDetail");

            migrationBuilder.DropTable(
                name: "VideoCardDetails");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
