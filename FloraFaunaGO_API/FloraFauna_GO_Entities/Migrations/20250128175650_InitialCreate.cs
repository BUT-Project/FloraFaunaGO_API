using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloraFauna_GO_Entities.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Espece",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Nom_scientifique = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Image3D = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Famille = table.Column<string>(type: "TEXT", nullable: false),
                    Zone = table.Column<string>(type: "TEXT", nullable: false),
                    Climat = table.Column<string>(type: "TEXT", nullable: false),
                    Regime = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Espece", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Localisation",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Rayon = table.Column<double>(type: "REAL", nullable: false),
                    CaptureDetailsId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localisation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Succes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Objectif = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Succes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateur",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Pseudo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Mail = table.Column<string>(type: "TEXT", maxLength: 70, nullable: false),
                    Hash_mdp = table.Column<string>(type: "TEXT", nullable: false),
                    DateInscription = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateur", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EspeceLocalisation",
                columns: table => new
                {
                    EspeceId = table.Column<string>(type: "TEXT", nullable: false),
                    LocalisationId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EspeceLocalisation", x => new { x.EspeceId, x.LocalisationId });
                    table.ForeignKey(
                        name: "FK_EspeceLocalisation_Espece_EspeceId",
                        column: x => x.EspeceId,
                        principalTable: "Espece",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EspeceLocalisation_Localisation_LocalisationId",
                        column: x => x.LocalisationId,
                        principalTable: "Localisation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Captures",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Photo = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Numero = table.Column<uint>(type: "INTEGER", nullable: false),
                    EspeceId = table.Column<string>(type: "TEXT", nullable: false),
                    UtilisateurId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Captures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Captures_Espece_EspeceId",
                        column: x => x.EspeceId,
                        principalTable: "Espece",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Captures_Utilisateur_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuccesState",
                columns: table => new
                {
                    SuccesEntitiesId = table.Column<string>(type: "TEXT", nullable: false),
                    UtilisateurId = table.Column<string>(type: "TEXT", nullable: false),
                    PercentSucces = table.Column<double>(type: "REAL", nullable: false),
                    IsSucces = table.Column<bool>(type: "INTEGER", nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuccesState", x => new { x.SuccesEntitiesId, x.UtilisateurId });
                    table.ForeignKey(
                        name: "FK_SuccesState_Succes_SuccesEntitiesId",
                        column: x => x.SuccesEntitiesId,
                        principalTable: "Succes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuccesState_Utilisateur_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaptureDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Shiny = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCapture = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LocalisationId = table.Column<string>(type: "TEXT", nullable: false),
                    CaptureId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptureDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaptureDetails_Captures_CaptureId",
                        column: x => x.CaptureId,
                        principalTable: "Captures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaptureDetails_Localisation_LocalisationId",
                        column: x => x.LocalisationId,
                        principalTable: "Localisation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaptureDetails_CaptureId",
                table: "CaptureDetails",
                column: "CaptureId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptureDetails_LocalisationId",
                table: "CaptureDetails",
                column: "LocalisationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Captures_EspeceId",
                table: "Captures",
                column: "EspeceId");

            migrationBuilder.CreateIndex(
                name: "IX_Captures_UtilisateurId",
                table: "Captures",
                column: "UtilisateurId");

            migrationBuilder.CreateIndex(
                name: "IX_EspeceLocalisation_LocalisationId",
                table: "EspeceLocalisation",
                column: "LocalisationId");

            migrationBuilder.CreateIndex(
                name: "IX_SuccesState_UtilisateurId",
                table: "SuccesState",
                column: "UtilisateurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaptureDetails");

            migrationBuilder.DropTable(
                name: "EspeceLocalisation");

            migrationBuilder.DropTable(
                name: "SuccesState");

            migrationBuilder.DropTable(
                name: "Captures");

            migrationBuilder.DropTable(
                name: "Localisation");

            migrationBuilder.DropTable(
                name: "Succes");

            migrationBuilder.DropTable(
                name: "Espece");

            migrationBuilder.DropTable(
                name: "Utilisateur");
        }
    }
}
