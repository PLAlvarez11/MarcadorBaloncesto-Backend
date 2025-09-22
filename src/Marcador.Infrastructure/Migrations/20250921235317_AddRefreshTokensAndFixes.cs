using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marcador.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokensAndFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Terminado",
                table: "Partidos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Terminado",
                table: "Partidos");
        }
    }
}
