USE CourseCentral

CREATE TABLE Students
(
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	FirstName NVARCHAR(1000) NOT NULL,
	MiddleName NVARCHAR(1000) NOT NULL DEFAULT '',
	LastName NVARCHAR(1000) NOT NULL,
	Suffix NVARCHAR(1000) NOT NULL DEFAULT '',
	DateOfBirth DATE NOT NULL
)

CREATE TABLE Courses
(
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	Name NVARCHAR (1000) NOT NULL,
	Department NVARCHAR(4) NOT NULL,
	Number INT NOT NULL,
	Section CHARACTER(1) NOT NULL DEFAULT 'A',
	Professor NVARCHAR(1000) NOT NULL,
	Year INT NOT NULL,
	Semester NVARCHAR(6) NOT NULL,
	CONSTRAINT checkDepartmentCasing CHECK (Department = UPPER(Department)),
	CONSTRAINT checkSectionCasing CHECK (Section = UPPER(Section)),
	CONSTRAINT courseNumber UNIQUE (Department, Number, Section, Year, Semester),
	CONSTRAINT checkSemester CHECK (Semester = 'FALL' OR Semester = 'SPRING' OR Semester = 'SUMMER'),
	CONSTRAINT checkSemesterCasing CHECK (Semester = UPPER(Semester))
)

CREATE TABLE CoursesTaken
(
	Student UNIQUEIDENTIFIER NOT NULL,
	Course UNIQUEIDENTIFIER NOT NULL,
	Grade Int NOT NULL,
	FOREIGN KEY (Student) REFERENCES Students(Id),
	FOREIGN KEY (Course) REFERENCES Courses(Id),
	CONSTRAINT checkGrade CHECK (0 <= Grade),
	CONSTRAINT courseTaken UNIQUE (Student, Course)
)