﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CuriousReadersData.Migrations
{
    public partial class CommentRatingColumnAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Comments");
        }
    }
}
