using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class IdentityRolesClaims2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationRoleApplicationUserRole",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationRoleIdentityRoleClaim<Guid>",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationUserApplicationUserRole",
                schema: "Identity");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId1",
                schema: "Identity",
                table: "AspNetUserRoles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                schema: "Identity",
                table: "AspNetUserRoles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId1",
                schema: "Identity",
                table: "AspNetUserRoles",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId1",
                schema: "Identity",
                table: "AspNetUserRoles",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId1",
                schema: "Identity",
                table: "AspNetUserRoles",
                column: "RoleId1",
                principalSchema: "Identity",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId1",
                schema: "Identity",
                table: "AspNetUserRoles",
                column: "UserId1",
                principalSchema: "Identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId1",
                schema: "Identity",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId1",
                schema: "Identity",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_RoleId1",
                schema: "Identity",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId1",
                schema: "Identity",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                schema: "Identity",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                schema: "Identity",
                table: "AspNetUserRoles");

            migrationBuilder.CreateTable(
                name: "ApplicationRoleApplicationUserRole",
                schema: "Identity",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApplicationUserRoleUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApplicationUserRoleRoleId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRoleApplicationUserRole", x => new { x.RolesId, x.ApplicationUserRoleUserId, x.ApplicationUserRoleRoleId });
                    table.ForeignKey(
                        name: "FK_ApplicationRoleApplicationUserRole_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalSchema: "Identity",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationRoleApplicationUserRole_AspNetUserRoles_ApplicationUserRoleUserId_ApplicationUserRoleRoleId",
                        columns: x => new { x.ApplicationUserRoleUserId, x.ApplicationUserRoleRoleId },
                        principalSchema: "Identity",
                        principalTable: "AspNetUserRoles",
                        principalColumns: new[] { "UserId", "RoleId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationRoleIdentityRoleClaim<Guid>",
                schema: "Identity",
                columns: table => new
                {
                    ApplicationRoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRoleIdentityRoleClaim<Guid>", x => new { x.ApplicationRoleId, x.ClaimsId });
                    table.ForeignKey(
                        name: "FK_ApplicationRoleIdentityRoleClaim<Guid>_AspNetRoleClaims_ClaimsId",
                        column: x => x.ClaimsId,
                        principalSchema: "Identity",
                        principalTable: "AspNetRoleClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationRoleIdentityRoleClaim<Guid>_AspNetRoles_ApplicationRoleId",
                        column: x => x.ApplicationRoleId,
                        principalSchema: "Identity",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserApplicationUserRole",
                schema: "Identity",
                columns: table => new
                {
                    UsersId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserRolesUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserRolesRoleId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserApplicationUserRole", x => new { x.UsersId, x.UserRolesUserId, x.UserRolesRoleId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserApplicationUserRole_AspNetUserRoles_UserRolesUserId_UserRolesRoleId",
                        columns: x => new { x.UserRolesUserId, x.UserRolesRoleId },
                        principalSchema: "Identity",
                        principalTable: "AspNetUserRoles",
                        principalColumns: new[] { "UserId", "RoleId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserApplicationUserRole_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "Identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleApplicationUserRole_ApplicationUserRoleUserId_ApplicationUserRoleRoleId",
                schema: "Identity",
                table: "ApplicationRoleApplicationUserRole",
                columns: new[] { "ApplicationUserRoleUserId", "ApplicationUserRoleRoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleIdentityRoleClaim<Guid>_ClaimsId",
                schema: "Identity",
                table: "ApplicationRoleIdentityRoleClaim<Guid>",
                column: "ClaimsId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserApplicationUserRole_UserRolesUserId_UserRolesRoleId",
                schema: "Identity",
                table: "ApplicationUserApplicationUserRole",
                columns: new[] { "UserRolesUserId", "UserRolesRoleId" });
        }
    }
}
