using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookRentalService.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalCountToBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentalCount",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentalCount",
                table: "Books");
        }
    }
}
