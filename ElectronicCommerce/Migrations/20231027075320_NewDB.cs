using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectronicCommerce.Migrations
{
    public partial class NewDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CATEGORY_PRODUCTS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PARENT_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORY_PRODUCTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK__CATEGORY___PAREN__3A4CA8FD",
                        column: x => x.PARENT_ID,
                        principalTable: "CATEGORY_PRODUCTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CUSTOMER_TYPES",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    CUSTOMER_TYPE_NAME = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DISCOUNT_VALUE = table.Column<int>(type: "int", nullable: true),
                    DISCOUNT_UNIT = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSTOMER_TYPES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GEOMANCIES",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GEOMANCIES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OverViewProductHomeFlags",
                columns: table => new
                {
                    PRODUCT_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IMAGE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PRICE = table.Column<int>(type: "int", nullable: false),
                    DISCOUNT_VALUE = table.Column<int>(type: "int", nullable: true),
                    UNIT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACTIVE = table.Column<bool>(type: "bit", nullable: true),
                    DIS_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_PRICES",
                columns: table => new
                {
                    PRODUCT_PRICE_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    BASE_PRICE = table.Column<int>(type: "int", nullable: true),
                    SALE_PRICE = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    IN_ACTIVE = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_PRICES", x => x.PRODUCT_PRICE_ID);
                });

            migrationBuilder.CreateTable(
                name: "PROMOTIONS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DISCOUNT_VALUE = table.Column<int>(type: "int", nullable: true),
                    DISCOUNT_UNIT = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: true),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    START_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    END_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    CODE = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    MAX_DISCOUNT = table.Column<int>(type: "int", nullable: true),
                    MIN_ORDER = table.Column<int>(type: "int", nullable: true),
                    ACTIVATE = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROMOTIONS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RolesModels",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ROLE_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TOTAL_USER = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SaleReportOptions",
                columns: table => new
                {
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SaleReports",
                columns: table => new
                {
                    Time = table.Column<int>(type: "int", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "STONE_TYPES",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STONE_TYPES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CUSTOMERS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    FULLNAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    USERNAME = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    PASSWORD = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DOB = table.Column<DateTime>(type: "date", nullable: true),
                    ID_CARD = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true),
                    CUSTOMER_TYPE_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    SCORE_PAY = table.Column<int>(type: "int", nullable: true),
                    ADDRESS = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    PHONE = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true),
                    AVATAR = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSTOMERS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CUSTOMERS_CUSTOMER_TYPES",
                        column: x => x.CUSTOMER_TYPE_ID,
                        principalTable: "CUSTOMER_TYPES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    USERNAME = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PASSWORD = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    FULLNAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DOB = table.Column<DateTime>(type: "date", nullable: false),
                    PHONE = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    ID_CARD = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true),
                    ID_ROLE = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_USERS_ROLES",
                        column: x => x.ID_ROLE,
                        principalTable: "ROLES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GEOMANCY_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    IMAGE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    COLOR = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    NOTE = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    BEST_SELLER = table.Column<bool>(type: "bit", nullable: false),
                    HOME_FLAG = table.Column<bool>(type: "bit", nullable: false),
                    ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    CAT_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    MAIN_STONE_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    SUB_STONE_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK__PRODUCTS__MAIN_S__3C34F16F",
                        column: x => x.MAIN_STONE_ID,
                        principalTable: "STONE_TYPES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__PRODUCTS__SUB_ST__3D2915A8",
                        column: x => x.SUB_STONE_ID,
                        principalTable: "STONE_TYPES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUCTS_CATEGORY_PRODUCTS",
                        column: x => x.CAT_ID,
                        principalTable: "CATEGORY_PRODUCTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUCTS_GEOMANCIES",
                        column: x => x.GEOMANCY_ID,
                        principalTable: "GEOMANCIES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PROMOTION_DETAIL",
                columns: table => new
                {
                    ID_PROMOTION_DETAIL = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    PROMOTION_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    CUSTOMER_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    CUS_TYPE_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PROMOTIO__486F6DC1627BF251", x => x.ID_PROMOTION_DETAIL);
                    table.ForeignKey(
                        name: "FK_PROMOTION_DETAIL_CUSTOMER_TYPES_CUS_TYPE_ID",
                        column: x => x.CUS_TYPE_ID,
                        principalTable: "CUSTOMER_TYPES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PROMOTION_DETAIL_CUSTOMERS",
                        column: x => x.CUSTOMER_ID,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PROMOTION_DETAIL_PROMOTIONS",
                        column: x => x.PROMOTION_ID,
                        principalTable: "PROMOTIONS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ORDER_PRODUCTS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    DATE_CREATED = table.Column<DateTime>(type: "date", nullable: true),
                    CUSTOMER_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    ADDRESS_DELIVERY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PAY = table.Column<int>(type: "int", nullable: true),
                    DATE_PAY = table.Column<DateTime>(type: "date", nullable: true),
                    PAY_TYPE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TOTAL_PAY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ORDER_STATE = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PHONE_NON_ACCOUNT = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true),
                    NAME_CUS_NON_ACCOUNT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SHIP_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    SHIP_FEE = table.Column<int>(type: "int", nullable: true),
                    ID_USER = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    MAIL_NON_CUS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PROMOTION_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    CUSTOMER_TYPE_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER_PRODUCTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ORDER_PRODUCTS_CUSTOMERS",
                        column: x => x.CUSTOMER_ID,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ORDER_PRODUCTS_USERS",
                        column: x => x.ID_USER,
                        principalTable: "USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromotionOrder",
                        column: x => x.PROMOTION_ID,
                        principalTable: "PROMOTIONS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    NAME_IMAGES = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PRODUCT_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IMAGES_PRODUCTS",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_DETAIL",
                columns: table => new
                {
                    PRODUCT_DETAIL_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    PRODUCT_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    QUANTITY = table.Column<int>(type: "int", nullable: true),
                    IMPORT_QUANTITY = table.Column<int>(type: "int", nullable: true),
                    PRODUCT_PRICE_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    SIZE = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_DETAIL", x => x.PRODUCT_DETAIL_ID);
                    table.ForeignKey(
                        name: "FK__PRODUCT_D__PRODU__625A9A57",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__PRODUCT_D__PRODU__634EBE90",
                        column: x => x.PRODUCT_PRICE_ID,
                        principalTable: "PRODUCT_PRICES",
                        principalColumn: "PRODUCT_PRICE_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_DISCOUNTS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    PRODUCT_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    DISCOUNT_VALUE = table.Column<int>(type: "int", nullable: true),
                    DISCOUNT_UNIT = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: true),
                    DATE_CREATED = table.Column<DateTime>(type: "date", nullable: true),
                    VALID_UNTIL = table.Column<DateTime>(type: "date", nullable: true),
                    ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    STONE_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    GEM_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_DISCOUNTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_pdc_gem",
                        column: x => x.GEM_ID,
                        principalTable: "GEOMANCIES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pdc_stone",
                        column: x => x.STONE_ID,
                        principalTable: "STONE_TYPES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUCT_DISCOUNTS_PRODUCTS",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "REVIEWS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    CUSTOMER_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    PRODUCT_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    TITLE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CONTENT = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RATE = table.Column<int>(type: "int", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    IS_UPDATE = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REVIEWS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_REVIEWS_CUSTOMERS",
                        column: x => x.CUSTOMER_ID,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_REVIEWS_PRODUCTS",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CARTS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    CUSTOMER_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    QUANTITY = table.Column<int>(type: "int", nullable: true),
                    ORDER_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    PRODUCT_DETAIL_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    IS_CHECK = table.Column<bool>(type: "bit", nullable: false),
                    SAVEPRICE = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK__CARTS__PRODUCT_D__0697FACD",
                        column: x => x.PRODUCT_DETAIL_ID,
                        principalTable: "PRODUCT_DETAIL",
                        principalColumn: "PRODUCT_DETAIL_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CARTS_CUSTOMERS",
                        column: x => x.CUSTOMER_ID,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CARTS_ORDER_PRODUCTS",
                        column: x => x.ORDER_ID,
                        principalTable: "ORDER_PRODUCTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ORDER_DETAILS",
                columns: table => new
                {
                    ORDER_DETAIL_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    ORDER_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    PRODUCT_DETAIL_ID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    QUANTITY = table.Column<int>(type: "int", nullable: true),
                    SALE_PRICE = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER_DETAILS", x => x.ORDER_DETAIL_ID);
                    table.ForeignKey(
                        name: "FK__ORDER_DET__ORDER__0A688BB1",
                        column: x => x.ORDER_ID,
                        principalTable: "ORDER_PRODUCTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ORDER_DET__PRODU__0B5CAFEA",
                        column: x => x.PRODUCT_DETAIL_ID,
                        principalTable: "PRODUCT_DETAIL",
                        principalColumn: "PRODUCT_DETAIL_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CARTS_CUSTOMER_ID",
                table: "CARTS",
                column: "CUSTOMER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CARTS_ORDER_ID",
                table: "CARTS",
                column: "ORDER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CARTS_PRODUCT_DETAIL_ID",
                table: "CARTS",
                column: "PRODUCT_DETAIL_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CATEGORY_PRODUCTS_PARENT_ID",
                table: "CATEGORY_PRODUCTS",
                column: "PARENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMERS_CUSTOMER_TYPE_ID",
                table: "CUSTOMERS",
                column: "CUSTOMER_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Images_PRODUCT_ID",
                table: "Images",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_DETAILS_ORDER_ID",
                table: "ORDER_DETAILS",
                column: "ORDER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_DETAILS_PRODUCT_DETAIL_ID",
                table: "ORDER_DETAILS",
                column: "PRODUCT_DETAIL_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_PRODUCTS_CUSTOMER_ID",
                table: "ORDER_PRODUCTS",
                column: "CUSTOMER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_PRODUCTS_ID_USER",
                table: "ORDER_PRODUCTS",
                column: "ID_USER");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_PRODUCTS_PROMOTION_ID",
                table: "ORDER_PRODUCTS",
                column: "PROMOTION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_DETAIL_PRODUCT_ID",
                table: "PRODUCT_DETAIL",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_DETAIL_PRODUCT_PRICE_ID",
                table: "PRODUCT_DETAIL",
                column: "PRODUCT_PRICE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_DISCOUNTS_GEM_ID",
                table: "PRODUCT_DISCOUNTS",
                column: "GEM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_DISCOUNTS_PRODUCT_ID",
                table: "PRODUCT_DISCOUNTS",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_DISCOUNTS_STONE_ID",
                table: "PRODUCT_DISCOUNTS",
                column: "STONE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_CAT_ID",
                table: "PRODUCTS",
                column: "CAT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_GEOMANCY_ID",
                table: "PRODUCTS",
                column: "GEOMANCY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_MAIN_STONE_ID",
                table: "PRODUCTS",
                column: "MAIN_STONE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_SUB_STONE_ID",
                table: "PRODUCTS",
                column: "SUB_STONE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PROMOTION_DETAIL_CUS_TYPE_ID",
                table: "PROMOTION_DETAIL",
                column: "CUS_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PROMOTION_DETAIL_CUSTOMER_ID",
                table: "PROMOTION_DETAIL",
                column: "CUSTOMER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PROMOTION_DETAIL_PROMOTION_ID",
                table: "PROMOTION_DETAIL",
                column: "PROMOTION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_REVIEWS_CUSTOMER_ID",
                table: "REVIEWS",
                column: "CUSTOMER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_REVIEWS_PRODUCT_ID",
                table: "REVIEWS",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_ID_ROLE",
                table: "USERS",
                column: "ID_ROLE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CARTS");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "ORDER_DETAILS");

            migrationBuilder.DropTable(
                name: "OverViewProductHomeFlags");

            migrationBuilder.DropTable(
                name: "PRODUCT_DISCOUNTS");

            migrationBuilder.DropTable(
                name: "PROMOTION_DETAIL");

            migrationBuilder.DropTable(
                name: "REVIEWS");

            migrationBuilder.DropTable(
                name: "RolesModels");

            migrationBuilder.DropTable(
                name: "SaleReportOptions");

            migrationBuilder.DropTable(
                name: "SaleReports");

            migrationBuilder.DropTable(
                name: "ORDER_PRODUCTS");

            migrationBuilder.DropTable(
                name: "PRODUCT_DETAIL");

            migrationBuilder.DropTable(
                name: "CUSTOMERS");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "PROMOTIONS");

            migrationBuilder.DropTable(
                name: "PRODUCTS");

            migrationBuilder.DropTable(
                name: "PRODUCT_PRICES");

            migrationBuilder.DropTable(
                name: "CUSTOMER_TYPES");

            migrationBuilder.DropTable(
                name: "ROLES");

            migrationBuilder.DropTable(
                name: "STONE_TYPES");

            migrationBuilder.DropTable(
                name: "CATEGORY_PRODUCTS");

            migrationBuilder.DropTable(
                name: "GEOMANCIES");
        }
    }
}
