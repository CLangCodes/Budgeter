using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SpendingForecast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "BudgetItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "BudgetItems",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItems_AccountId",
                table: "BudgetItems",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItems_Accounts_AccountId",
                table: "BudgetItems",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);

            // DateTime -> DateOnly: SQLite stores both as TEXT, so EF generates no column change.
            // Existing values look like "yyyy-MM-dd HH:mm:ss.FFFFFFF"; DateOnly expects "yyyy-MM-dd".
            migrationBuilder.Sql("UPDATE Transactions SET Date = substr(Date, 1, 10);");
            migrationBuilder.Sql("UPDATE Accounts SET CreatedAt = substr(CreatedAt, 1, 10), UpdatedAt = substr(UpdatedAt, 1, 10);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItems_Accounts_AccountId",
                table: "BudgetItems");

            migrationBuilder.DropIndex(
                name: "IX_BudgetItems_AccountId",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "BudgetItems");
        }
    }
}
