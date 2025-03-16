using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListInfo.API.DBLayer.Migrations
{
    /// <inheritdoc />
    public partial class CTestMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Link",
                table: "Upload",
                newName: "Path");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Upload",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailOwner",
                table: "Upload",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IdOwner",
                table: "Upload",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "ToDoLists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdOwner",
                table: "ToDoLists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Upload");

            migrationBuilder.DropColumn(
                name: "EmailOwner",
                table: "Upload");

            migrationBuilder.DropColumn(
                name: "IdOwner",
                table: "Upload");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "ToDoLists");

            migrationBuilder.DropColumn(
                name: "IdOwner",
                table: "ToDoLists");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Upload",
                newName: "Link");
        }
    }
}
