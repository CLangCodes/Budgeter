using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AccountTransactions : Migration
    {
        const string defaultAccountId = "00000000-0000-0000-0000-000000000001";
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { 
                    "AccountId", 
                    "AccountType", 
                    "CreatedAt", 
                    "NickName", 
                    "StartingBalance", 
                    "UpdatedAt" },
                values: new object[] { 
                    new Guid(defaultAccountId), 
                    "Checking", 
                    DateTime.UtcNow, 
                    "Default Account", 
                    0m, 
                    DateTime.UtcNow });

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Transactions",
                type: "TEXT",
                nullable: true,
                defaultValue: new Guid(defaultAccountId));

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Transactions");
        }
    }
}
