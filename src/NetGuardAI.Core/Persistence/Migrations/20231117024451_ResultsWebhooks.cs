using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetGuardAI.Core.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ResultsWebhooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Duration>(
                name: "IpCooldown",
                table: "ScanSettings",
                type: "interval",
                nullable: false,
                defaultValue: NodaTime.Duration.Zero);

            migrationBuilder.CreateTable(
                name: "ScanResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IpAddress = table.Column<IPAddress>(type: "inet", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    RawInfo = table.Column<string>(type: "text", nullable: false),
                    ProcessedInfo = table.Column<string>(type: "text", nullable: false),
                    ScanTime = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserWebhooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWebhooks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScanResults");

            migrationBuilder.DropTable(
                name: "UserWebhooks");

            migrationBuilder.DropColumn(
                name: "IpCooldown",
                table: "ScanSettings");
        }
    }
}
