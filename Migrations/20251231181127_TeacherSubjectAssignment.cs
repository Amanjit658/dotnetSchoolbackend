using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myFirstSchoolProject.Migrations
{
    /// <inheritdoc />
    public partial class TeacherSubjectAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcademicYearId",
                table: "TeacherClassSubjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherClassSubjects_AcademicYearId",
                table: "TeacherClassSubjects",
                column: "AcademicYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherClassSubjects_AcademicYears_AcademicYearId",
                table: "TeacherClassSubjects",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherClassSubjects_AcademicYears_AcademicYearId",
                table: "TeacherClassSubjects");

            migrationBuilder.DropIndex(
                name: "IX_TeacherClassSubjects_AcademicYearId",
                table: "TeacherClassSubjects");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "TeacherClassSubjects");
        }
    }
}
