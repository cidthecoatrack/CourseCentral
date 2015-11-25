USE CourseCentral

INSERT INTO Students (Id, FirstName, MiddleName, LastName, Suffix, DateOfBirth)
VALUES ('ABF60760-13EB-4BE5-A029-F1B7CF62B514', 'Karl', 'Matthew', 'Speer', 'the Awesome', '1989-10-29')

INSERT INTO Students (Id, FirstName, LastName, DateOfBirth)
VALUES ('E3FD0B4E-F681-4074-800F-D22838F6FFA9', 'Ferris', 'Bueller', '1985-06-09')

INSERT INTO Courses (Id, Name, Department, Number, Section, Professor, Year, Semester)
VALUES ('0A127D6F-913F-49AA-AABB-1E712E5939C5', 'Database Construction and Maintenance', 'CSCI', '2669', 'A', 'Robert Martin', 2015, 'FALL')

INSERT INTO Courses (Id, Name, Department, Number, Section, Professor, Year, Semester)
VALUES ('8f8c4be1-a2d2-44b8-ba2e-06a4716c8578', 'Database Construction and Maintenance', 'CSCI', '2669', 'A', 'Robert Martin', 2014, 'FALL')

INSERT INTO Courses (Id, Name, Department, Number, Section, Professor, Year, Semester)
VALUES ('0f013aa9-7dcf-41a5-b1f5-4e785af734b3', 'Database Construction and Maintenance', 'CSCI', '2669', 'A', 'Robert Martin', 2013, 'FALL')

INSERT INTO Courses (Id, Name, Department, Number, Section, Professor, Year, Semester)
VALUES ('2949CF66-3C9C-42A3-A820-4E53FD6BED3D', 'Database Construction and Maintenance', 'CSCI', '2669', 'B', 'Joel Spolsky', 2015, 'FALL')

INSERT INTO Courses (Id, Name, Department, Number, Professor, Year, Semester)
VALUES ('28D64EBD-44BF-49E5-8709-49A6F291E3DF', 'Introduction to Model-View-Controller', 'CSCI', '1337', 'Scott Hanselmann', 2015, 'SPRING')

INSERT INTO Courses (Id, Name, Department, Number, Professor, Year, Semester)
VALUES ('E0936A90-9257-4CF5-B253-A05DF0B07E23', 'Magical Realism', 'ENGL', '3008', 'Jim Peterson', 2011, 'SPRING')

INSERT INTO Courses (Id, Name, Department, Number, Professor, Year, Semester)
VALUES ('D6A31382-D70F-407F-A58E-894F4B63344E', 'Science Fiction: Is It Just Fantasy With Steel Instead Of Trees?', 'ENGL', '4242', 'Orson Scott Card', 2014, 'SUMMER')

INSERT INTO CoursesTaken (Student, Course, Grade)
VALUES ('ABF60760-13EB-4BE5-A029-F1B7CF62B514', 'E0936A90-9257-4CF5-B253-A05DF0B07E23', 86)

INSERT INTO CoursesTaken (Student, Course, Grade)
VALUES ('ABF60760-13EB-4BE5-A029-F1B7CF62B514', '0A127D6F-913F-49AA-AABB-1E712E5939C5', 92)

INSERT INTO CoursesTaken (Student, Course, Grade)
VALUES ('ABF60760-13EB-4BE5-A029-F1B7CF62B514', '28D64EBD-44BF-49E5-8709-49A6F291E3DF', 100)

INSERT INTO CoursesTaken (Student, Course, Grade)
VALUES ('ABF60760-13EB-4BE5-A029-F1B7CF62B514', 'D6A31382-D70F-407F-A58E-894F4B63344E', 102)

INSERT INTO CoursesTaken (Student, Course, Grade)
VALUES ('E3FD0B4E-F681-4074-800F-D22838F6FFA9', '0f013aa9-7dcf-41a5-b1f5-4e785af734b3', 54)

INSERT INTO CoursesTaken (Student, Course, Grade)
VALUES ('E3FD0B4E-F681-4074-800F-D22838F6FFA9', '8f8c4be1-a2d2-44b8-ba2e-06a4716c8578', 62)

INSERT INTO CoursesTaken (Student, Course, Grade)
VALUES ('E3FD0B4E-F681-4074-800F-D22838F6FFA9', '2949CF66-3C9C-42A3-A820-4E53FD6BED3D', 48)

INSERT INTO CoursesTaken (Student, Course, Grade)
VALUES ('E3FD0B4E-F681-4074-800F-D22838F6FFA9', '28D64EBD-44BF-49E5-8709-49A6F291E3DF', 44)