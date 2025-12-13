using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_AspNetUsers_EmployeeId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_EmployeeId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Conversations");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Conversations",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Conversations");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Conversations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_EmployeeId",
                table: "Conversations",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_AspNetUsers_EmployeeId",
                table: "Conversations",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
