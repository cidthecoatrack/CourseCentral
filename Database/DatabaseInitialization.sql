CREATE DATABASE CourseCentral

CREATE LOGIN CourseCentralUser
WITH PASSWORD = 'courseCentralPassword',
	DEFAULT_DATABASE = CourseCentral
	
CREATE USER CourseCentralUser FOR LOGIN CourseCentralUser
EXEC sp_addrolemember N'db_owner', N'CourseCentralUser'