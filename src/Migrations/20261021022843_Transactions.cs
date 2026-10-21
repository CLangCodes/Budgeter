using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetBuddy.Migrations
{
    /// <inheritdoc />
    public partial class Transactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionTypeId", "Description", "Amount", "Date", "Category" },
                values: new object[,]
                {
                    { 1, "Paycheck",       2500.00m, new DateTime(2026, 10, 1),  "Income" },
                    { 2, "Rent",           1200.00m, new DateTime(2026, 10, 1),  "Housing" },
                    { 2, "Groceries",        85.42m, new DateTime(2026, 10, 3),  "Food" },
                    { 2, "Electric Bill",    64.10m, new DateTime(2026, 10, 5),  "Utilities" }                       
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
