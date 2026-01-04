using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myFirstSchoolProject.Migrations
{
    /// <inheritdoc />
    public partial class AllocateStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentClassAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClassAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentClassAllocations_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentClassAllocations_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentClassAllocations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassAllocations_AcademicYearId",
                table: "StudentClassAllocations",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassAllocations_ClassId",
                table: "StudentClassAllocations",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassAllocations_StudentId",
                table: "StudentClassAllocations",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentClassAllocations");
        }
    }
}
