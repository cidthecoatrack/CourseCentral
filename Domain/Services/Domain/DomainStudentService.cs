using CourseCentral.Domain.Repositories;
using System;

namespace CourseCentral.Domain.Services.Domain
{
    public class DomainStudentService : StudentService
    {
        private StudentRepository studentRepository;
        private CourseTakenRepository courseTakenRepository;

        public DomainStudentService(StudentRepository studentRepository, CourseTakenRepository courseTakenRepository)
        {
            this.studentRepository = studentRepository;
            this.courseTakenRepository = courseTakenRepository;
        }

        public void Remove(Guid id)
        {
            var coursesTaken = courseTakenRepository.FindCourses(id);

            foreach (var courseTaken in coursesTaken)
                courseTakenRepository.Remove(courseTaken);

            studentRepository.Remove(id);
        }
    }
}
