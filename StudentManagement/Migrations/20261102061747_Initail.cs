using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagement.Migrations
{
    /// <inheritdoc />
    public partial class Initail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gpa = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "علوم الحاسب" },
                    { 2, "الهندسة" },
                    { 3, "إدارة الأعمال" },
                    { 4, "الصيدلة" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "BirthDate", "DepartmentId", "Email", "EnrollmentDate", "FullName", "Gpa" },
                values: new object[,]
                {
                    { 1, new DateTime(2001, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "student1@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "محمد علي", 2.1m },
                    { 2, new DateTime(2002, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "student2@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "منى إبراهيم", 2.2m },
                    { 3, new DateTime(2003, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "student3@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "سارة السيد", 2.3m },
                    { 4, new DateTime(2004, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "student4@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "يوسف محمود", 2.4m },
                    { 5, new DateTime(2000, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "student5@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "نور فتحي", 2.5m },
                    { 6, new DateTime(2001, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "student6@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "عمر حسن", 2.6m },
                    { 7, new DateTime(2002, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "student7@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "هبة علي", 2.7m },
                    { 8, new DateTime(2003, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "student8@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "كريم إبراهيم", 2.8m },
                    { 9, new DateTime(2004, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "student9@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ليلى السيد", 2.9m },
                    { 10, new DateTime(2000, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "student10@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "أحمد محمود", 3.0m },
                    { 11, new DateTime(2001, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "student11@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "محمد فتحي", 3.1m },
                    { 12, new DateTime(2002, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "student12@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "منى حسن", 3.2m },
                    { 13, new DateTime(2003, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "student13@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "سارة علي", 3.3m },
                    { 14, new DateTime(2004, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "student14@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "يوسف إبراهيم", 3.4m },
                    { 15, new DateTime(2000, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "student15@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "نور السيد", 3.5m },
                    { 16, new DateTime(2001, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "student16@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "عمر محمود", 3.6m },
                    { 17, new DateTime(2002, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "student17@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "هبة فتحي", 3.7m },
                    { 18, new DateTime(2003, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "student18@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "كريم حسن", 3.8m },
                    { 19, new DateTime(2004, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "student19@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ليلى علي", 3.9m },
                    { 20, new DateTime(2000, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "student20@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "أحمد إبراهيم", 4.0m },
                    { 21, new DateTime(2001, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "student21@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "محمد السيد", 2.0m },
                    { 22, new DateTime(2002, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "student22@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "منى محمود", 2.1m },
                    { 23, new DateTime(2003, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "student23@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "سارة فتحي", 2.2m },
                    { 24, new DateTime(2004, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "student24@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "يوسف حسن", 2.3m },
                    { 25, new DateTime(2000, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "student25@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "نور علي", 2.4m },
                    { 26, new DateTime(2001, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "student26@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "عمر إبراهيم", 2.5m },
                    { 27, new DateTime(2002, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "student27@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "هبة السيد", 2.6m },
                    { 28, new DateTime(2003, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "student28@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "كريم محمود", 2.7m },
                    { 29, new DateTime(2004, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "student29@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ليلى فتحي", 2.8m },
                    { 30, new DateTime(2000, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "student30@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "أحمد حسن", 2.9m },
                    { 31, new DateTime(2001, 8, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "student31@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "محمد علي", 3.0m },
                    { 32, new DateTime(2002, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "student32@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "منى إبراهيم", 3.1m },
                    { 33, new DateTime(2003, 10, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "student33@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "سارة السيد", 3.2m },
                    { 34, new DateTime(2004, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "student34@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "يوسف محمود", 3.3m },
                    { 35, new DateTime(2000, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "student35@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "نور فتحي", 3.4m },
                    { 36, new DateTime(2001, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "student36@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "عمر حسن", 3.5m },
                    { 37, new DateTime(2002, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "student37@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "هبة علي", 3.6m },
                    { 38, new DateTime(2003, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "student38@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "كريم إبراهيم", 3.7m },
                    { 39, new DateTime(2004, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "student39@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ليلى السيد", 3.8m },
                    { 40, new DateTime(2000, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "student40@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "أحمد محمود", 3.9m },
                    { 41, new DateTime(2001, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "student41@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "محمد فتحي", 4.0m },
                    { 42, new DateTime(2002, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "student42@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "منى حسن", 2.0m },
                    { 43, new DateTime(2003, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "student43@example.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "سارة علي", 2.1m },
                    { 44, new DateTime(2004, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "student44@example.com", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "يوسف إبراهيم", 2.2m },
                    { 45, new DateTime(2000, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "student45@example.com", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "نور السيد", 2.3m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name",
                table: "Departments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_DepartmentId",
                table: "Students",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_FullName",
                table: "Students",
                column: "FullName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
