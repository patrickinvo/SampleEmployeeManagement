using EmployeeManagement.Models;
using EmployeeManagement.Services;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            //await Task.CompletedTask;
            return View();
        }

        // GET: Employees/GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            var viewModels = employees.Select(e => new EmployeeViewModel
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Department = e.Department,
                Position = e.Position,
                Status = e.Status.ToString(),
                ProfileImage = e.ProfileImage
            });

            return Json(viewModels);
        }

        // POST: Employees/Filter
        [HttpPost]
        public async Task<IActionResult> Filter([FromBody] EmployeeFilterRequest request)
        {
            var statuses = request.Statuses?.Select(s => Enum.Parse<EmployeeStatus>(s)).ToArray();

            var employees = await _employeeService.FilterEmployeesAsync(
                request.SearchTerm ?? string.Empty,
                request.Departments ?? Array.Empty<string>(),
                statuses ?? Array.Empty<EmployeeStatus>());

            var viewModels = employees.Select(e => new EmployeeViewModel
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Department = e.Department,
                Position = e.Position,
                Status = e.Status.ToString(),
                ProfileImage = e.ProfileImage
            });

            return Json(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var viewModel = new EmployeeViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Department = employee.Department,
                Position = employee.Position,
                Status = employee.Status.ToString(),
                ProfileImage = employee.ProfileImage
            };

            return Json(viewModel);
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] EmployeeCreateEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = new Employee
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Email = viewModel.Email,
                Department = viewModel.Department,
                Position = viewModel.Position,
                Status = Enum.Parse<EmployeeStatus>(viewModel.Status),
                ProfileImage = viewModel.ProfileImage ?? "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=40&h=40&fit=crop&crop=face"
            };

            await _employeeService.CreateEmployeeAsync(employee);
            return Json(new { success = true, message = "Employee created successfully" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] EmployeeCreateEditViewModel viewModel)
        {
            if (id != viewModel.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.FirstName = viewModel.FirstName;
            employee.LastName = viewModel.LastName;
            employee.Email = viewModel.Email;
            employee.Department = viewModel.Department;
            employee.Position = viewModel.Position;
            employee.Status = Enum.Parse<EmployeeStatus>(viewModel.Status);
            employee.ProfileImage = viewModel.ProfileImage ?? employee.ProfileImage;

            await _employeeService.UpdateEmployeeAsync(employee);
            return Json(new { success = true, message = "Employee updated successfully" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _employeeService.DeleteEmployeeAsync(id);
            return Json(new { success = true, message = "Employee deleted successfully" });
        }

        [HttpGet]
        public IActionResult GetDepartments()
        {
            return Json(Department.AllDepartments);
        }

        [HttpGet]
        public IActionResult GetStatuses()
        {
            var statuses = Enum.GetNames(typeof(EmployeeStatus));
            return Json(statuses);
        }
    }

    public class EmployeeFilterRequest
    {
        public string? SearchTerm { get; set; }
        public string[]? Departments { get; set; }
        public string[]? Statuses { get; set; }
    }
}