using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _01_TaskAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserRelationCommited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_todos_users_user_id",
                table: "todos");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "todos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_todos_users_user_id",
                table: "todos",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_todos_users_user_id",
                table: "todos");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "todos",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "fk_todos_users_user_id",
                table: "todos",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
