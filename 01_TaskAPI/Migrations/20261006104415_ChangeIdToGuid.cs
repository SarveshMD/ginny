using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace _01_TaskAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_todos",
                table: "todos"
            );

            migrationBuilder.AddColumn<Guid>(
                name: "new_id",
                table: "todos",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()"
            );

            migrationBuilder.DropColumn(
                name: "id",
                table: "todos"
            );

            migrationBuilder.RenameColumn(
                name: "new_id",
                table: "todos",
                newName: "id"
            );

            migrationBuilder.AddPrimaryKey(
                name: "pk_todos",
                table: "todos",
                column: "id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                    name: "pk_todos",
                    table: "todos");

            migrationBuilder.DropColumn(
                name: "id",
                table: "todos");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "todos",
                type: "integer",
                nullable: false)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "pk_todos",
                table: "todos",
                column: "id");
        }
    }
}
