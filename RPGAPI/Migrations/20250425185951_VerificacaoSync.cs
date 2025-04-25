using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPGAPI.Migrations
{
    /// <inheritdoc />
    public partial class VerificacaoSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TB_PERSONAGENS_HABILIDADES_PersonagemId",
                table: "TB_PERSONAGENS_HABILIDADES");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TB_PERSONAGENS_HABILIDADES_PersonagemId",
                table: "TB_PERSONAGENS_HABILIDADES",
                column: "PersonagemId",
                unique: true);
        }
    }
}
