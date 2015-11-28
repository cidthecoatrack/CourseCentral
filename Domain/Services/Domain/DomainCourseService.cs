using CourseCentral.Domain.Repositories;
using System;

namespace CourseCentral.Domain.Services.Domain
{
    public class DomainCourseService : CourseService
    {
        private CourseRepository courseRepository;
        private CourseTakenRepository courseTakenRepository;

        public DomainCourseService(CourseRepository courseRepository, CourseTakenRepository courseTakenRepository)
        {
            this.courseRepository = courseRepository;
            this.courseTakenRepository = courseTakenRepository;
        }

        public void Remove(Guid id)
        {
            var coursesTaken = courseTakenRepository.FindCourses(id);

            foreach (var courseTaken in coursesTaken)
                courseTakenRepository.Remove(courseTaken);

            courseRepository.Remove(id);
        }
    }
}
