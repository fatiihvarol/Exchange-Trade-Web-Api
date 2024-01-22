using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shares", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentityNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TotalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portfolios_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PortfolioItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShareId = table.Column<int>(type: "int", nullable: false),
                    PortfolioId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortfolioItem_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PortfolioItem_Shares_ShareId",
                        column: x => x.ShareId,
                        principalTable: "Shares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortfolioId = table.Column<int>(type: "int", nullable: false),
                    ShareId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeLogs_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TradeLogs_Shares_ShareId",
                        column: x => x.ShareId,
                        principalTable: "Shares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioItem_PortfolioId",
                table: "PortfolioItem",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioItem_ShareId",
                table: "PortfolioItem",
                column: "ShareId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_UserId",
                table: "Portfolios",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradeLogs_PortfolioId",
                table: "TradeLogs",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeLogs_ShareId",
                table: "TradeLogs",
                column: "ShareId");
            
            
            // Seed Users
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Surname", "IdentityNumber", "Email", "UserName", "Password", "Role", "Budget", "InsertDate", "IsActive" },
                values: new object[,]
                {
                    { 1, "Admin", "AdminSurname", "AdminIdentity", "admin@example.com", "admin", "adminpassword", "admin", 10000.00m, DateTime.Now, true },
                    { 2, "User1", "User1Surname", "User1Identity", "user1@example.com", "user1", "user1password", "user", 5000.00m, DateTime.Now, true },
                    { 3, "User2", "User2Surname", "User2Identity", "user2@example.com", "user2", "user2password", "user", 6000.00m, DateTime.Now, true },
                    { 4, "User3", "User3Surname", "User3Identity", "user3@example.com", "user3", "user3password", "user", 7000.00m, DateTime.Now, true },
                    { 5, "User4", "User4Surname", "User4Identity", "user4@example.com", "user4", "user4password", "user", 8000.00m, DateTime.Now, true },
                    { 6, "User5", "User5Surname", "User5Identity", "user5@example.com", "user5", "user5password", "user", 9000.00m, DateTime.Now, true }
                });

            // Seed Portfolios
            migrationBuilder.InsertData(
                table: "Portfolios",
                columns: new[] { "Id", "UserId", "TotalBalance", "InsertDate", "IsActive" },
                values: new object[,]
                {
                    { 1, 2, 5000.00m, DateTime.Now, true },
                    { 2, 3, 6000.00m, DateTime.Now, true },
                    { 3, 4, 7000.00m, DateTime.Now, true },
                    { 4, 5, 8000.00m, DateTime.Now, true },
                    { 5, 6, 9000.00m, DateTime.Now, true }
                });

            // Seed Shares
            migrationBuilder.InsertData(
                table: "Shares",
                columns: new[] { "Id", "Symbol", "CurrentPrice", "TotalAmount", "InsertDate", "IsActive" },
                values: new object[,]
                {
                    { 1, "THY", 10.00m, 100, DateTime.Now, true },
                    { 2, "GMR", 20.00m, 150, DateTime.Now, true },
                    { 3, "AKB", 30.00m, 200, DateTime.Now, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortfolioItem");

            migrationBuilder.DropTable(
                name: "TradeLogs");

            migrationBuilder.DropTable(
                name: "Portfolios");

            migrationBuilder.DropTable(
                name: "Shares");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
