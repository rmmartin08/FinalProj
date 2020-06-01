using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProj.DATA
{
    [MetadataType(typeof(CourseMetadata))]
    public partial class Course { }

    public class CourseMetadata
    {
        [Display(Name = "Course")]
        [Required(ErrorMessage = "*Required")]
        [StringLength(200, ErrorMessage = "Must be 200 Characters or Less")]
        public string CourseName { get; set; }

        [Display(Name = "Description")]
        [StringLength(200, ErrorMessage = "Must be 200 Characters or Less")]
        [UIHint("MulitlineText")]
        public string CourseDescription { get; set; }

        [Display(Name = "Active?")]
        [Required(ErrorMessage = "*Required")]
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(CourseCompletionMetadata))]
    public partial class CourseCompletion { }

    public class CourseCompletionMetadata
    {
        [Display(Name = "User")]
        [Required(ErrorMessage = "*Required")]
        [StringLength(128, ErrorMessage = "Must be 128 Characters or Less")]
        public string UserId { get; set; }


        [Display(Name = "Course")]
        [Required(ErrorMessage = "*Required")]
        public int CourseId { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Required(ErrorMessage = "*Required")]
        public System.DateTime DateCompleted { get; set; }
    }

    [MetadataType(typeof(LessonViewMetadata))]
    public partial class LessonView { }

    public class LessonViewMetadata
    {
        [Display(Name = "User")]
        [Required(ErrorMessage = "*Required")]
        [StringLength(128, ErrorMessage = "Must be 128 Characters or Less")]
        public string UserId { get; set; }

        [Display(Name = "Lesson")]
        [Required(ErrorMessage = "*Required")]
        public int LessonId { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Required(ErrorMessage = "*Required")]
        public System.DateTime DateViewed { get; set; }

    }

    [MetadataType(typeof(LessonMetadata))]
    public partial class Lesson { }

    public class LessonMetadata
    {
        [Display(Name = "Title")]
        [Required(ErrorMessage = "*Required")]
        [StringLength(200, ErrorMessage = "Must be 200 Characters or Less")]
        public string LessonTitle { get; set; }

        [Display(Name = "Course")]
        [Required(ErrorMessage = "*Required")]
        public int CourseId { get; set; }

        [Display(Name = "Intro")]
        [StringLength(300, ErrorMessage = "Must be 300 Characters or Less")]
        public string Introduction { get; set; }

        [Display(Name = "Video URL")]
        [StringLength(250, ErrorMessage = "Must be 250 Characters or Less")]
        public string VideoURL { get; set; }

        [Display(Name = "Video URL")]
        [StringLength(250, ErrorMessage = "Must be 250 Characters or Less")]
        public string PdfFilename { get; set; }

        [Display(Name = "Active?")]
        [Required(ErrorMessage = "*Required")]
        public bool isActive { get; set; }

    }

    [MetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail {
        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }

    public class UserDetailMetadata
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "*Required")]
        [StringLength(200, ErrorMessage = "Must be 200 Characters or Less")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "*Required")]
        [StringLength(200, ErrorMessage = "Must be 200 Characters or Less")]
        public string LastName { get; set; }
    }

}