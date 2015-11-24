CREATE TABLE [dbo].[CoursesTaken]
(
    [Student] UNIQUEIDENTIFIER NOT NULL, 
    [Course] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_CoursesTaken_Students] FOREIGN KEY ([Student]) REFERENCES [Students]([Id]), 
    CONSTRAINT [FK_CoursesTaken_Courses] FOREIGN KEY ([Course]) REFERENCES [Courses]([Id])
)
