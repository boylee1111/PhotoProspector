using System.IO;
using System.Web.Mvc;
using PhotoProspector.Services;
using PhotoProspector.ViewModels;

namespace PhotoProspector.Controllers
{
    public class CustomerSignUpController : Controller
    {
        private readonly IImageService imageService;
        private readonly IFileService fileService;
        private readonly IUserService userService;
        private readonly IMessageService messageService;
        private readonly ITrainingService trainingService;

        public CustomerSignUpController(
            IImageService imageService,
            IFileService fileService,
            IUserService userService,
            IMessageService messageService,
            ITrainingService trainingService)
        {
            this.imageService = imageService;
            this.fileService = fileService;
            this.userService = userService;
            this.messageService = messageService;
            this.trainingService = trainingService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new CustomerSignUpViewModel());
        }

        public ActionResult Index(CustomerSignUpViewModel customerSignUpViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(customerSignUpViewModel.photoPath.FileName);
                    var tempFolder = Server.MapPath("~/TempImg/");
                    if (!Directory.Exists(tempFolder))
                    {
                        Directory.CreateDirectory(tempFolder);
                    }
                    var savedFullPaht = tempFolder + fileService.filenameHashed(filename);
                    customerSignUpViewModel.photoPath.SaveAs(savedFullPaht);
                    if (userService.InsertUserToSQL(savedFullPaht, customerSignUpViewModel, Server.MapPath("~/ImageSource/"))) // change to customer
                    {
                        string trainpath = Server.MapPath("~/ImageSource/");
                        trainingService.StartTraining(trainpath);
                        return View(new CustomerSignUpViewModel(CustomerSignUpViewModel.SignUpStatus.SignUpSucceed));
                    }
                    else
                    {
                        return View(new CustomerSignUpViewModel(CustomerSignUpViewModel.SignUpStatus.SignUpFailed, "Sign up failed. Please try again later."));
                    }
                }
            }
            catch
            {
                return View(new CustomerSignUpViewModel(CustomerSignUpViewModel.SignUpStatus.SignUpFailed, "Sign up failed. Please try again later."));
            }

            return View(customerSignUpViewModel);
        }

        [HttpPost]
        public ActionResult SendInvitationCode(string email)
        {
            try
            {
                messageService.SendInvitationCode(email);
            }
            catch
            {
                return Json(new { success = false, errorMessage = "Send invitation code failed, please try later." });
            }
            return Json(new { success = true });
        }
    }
}