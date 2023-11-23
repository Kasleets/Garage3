using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Garage3.Data;
using Garage3.Models.Entities;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Garage3.Controllers
{
    public class MemberController : Controller
    {
        private readonly ParkingDbContext _context;

        public MemberController(ParkingDbContext context)
        {
            _context = context;
        }

        // GET: Member/Overview
        public async Task<IActionResult> Overview(string sortOrder, string searchString)
        {
            // Set up sorting parameters
            ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            // Query to retrieve members and their vehicle count
            var members = from m in _context.Members
                          select new
                          {
                              Member = m,
                              VehicleCount = m.Vehicles.Count
                          };

            // Apply search filter if searchString is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                members = members.Where(m => m.Member.FirstName.Contains(searchString) || m.Member.LastName.Contains(searchString));
            }

            // Apply sorting based on sortOrder parameter
            switch (sortOrder)
            {
                case "name_desc":
                    // Sort by first name in descending order
                    members = members.OrderByDescending(m => m.Member.FirstName);
                    break;
                case "name_asc":
                    // Sort by first name in ascending order
                    members = members.OrderBy(m => m.Member.FirstName);
                    break;
                case "name2_desc":
                    // Sort by the first two characters of first name in descending order,
                    // then by the entire first name in descending order
                    members = members.OrderByDescending(m => m.Member.FirstName.Substring(0, 2)).ThenByDescending(m => m.Member.FirstName);
                    break;
                case "name2_asc":
                    // Sort by the first two characters of first name in ascending order,
                    // then by the entire first name in ascending order
                    members = members.OrderBy(m => m.Member.FirstName.Substring(0, 2)).ThenBy(m => m.Member.FirstName);
                    break;
                default:
                    // Default sorting: Sort by first name in ascending order
                    members = members.OrderBy(m => m.Member.FirstName);
                    break;
            }

            // Create a view model with selected member properties
            var viewModel = members.Select(m => new
            {
                MemberID = m.Member.MemberID,
                FirstName = m.Member.FirstName,
                LastName = m.Member.LastName,
                VehicleCount = m.VehicleCount
            });

            
            var membersList = await viewModel.ToListAsync();

            // Return the Overview view with the sorted and filtered members list
            return View("Overview", membersList);
        }


        // GET: Member/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonalNumber,FirstName,LastName,Age")] Member member)
        {
            try
            {
                // Validate social security number format
                if (!IsValidSocialSecurityNumber(member.PersonalNumber))
                {
                    ModelState.AddModelError("PersonalNumber", "Invalid social security number format.");
                }

                // Validate first name is not the same as last name
                if (member.FirstName.Equals(member.LastName, System.StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("LastName", "Last name must be different from first name.");
                }

                // Validate age for parking
                if (member.Age < 18)
                {
                    ModelState.AddModelError("Age", "Members must be over 18 years old.");
                }

                // Check if a member with the same social security number already exists
                var existingMember = await _context.Members.FirstOrDefaultAsync(m => m.PersonalNumber == member.PersonalNumber);
                if (existingMember != null)
                {
                    ModelState.AddModelError("PersonalNumber", "A member with this social security number already exists.");
                }

                // Check if the model is valid before saving
                if (ModelState.IsValid)
                {
                    _context.Add(member);
                    await _context.SaveChangesAsync();

                    // Create a new account for the member
                    var account = new Account { MemberID = member.MemberID };
                    _context.Accounts.Add(account);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Member registered successfully!";
                    //return RedirectToAction(nameof(Overview));
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(member);
        }

        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberID,PersonalNumber,FirstName,LastName,Age")] Member member)
        {
            if (id != member.MemberID)
            {
                return NotFound();
            }

            try
            {
                // Validate social security number format
                if (!IsValidSocialSecurityNumber(member.PersonalNumber))
                {
                    ModelState.AddModelError("PersonalNumber", "Invalid social security number format.");
                }

                // Validate first name is not the same as last name
                if (member.FirstName.Equals(member.LastName, System.StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("LastName", "Last name must be different from first name.");
                }

                // Validate age for parking
                if (member.Age < 18)
                {
                    ModelState.AddModelError("Age", "Members must be over 18 years old.");
                }

                if (ModelState.IsValid)
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Member edited successfully!";
                    return RedirectToAction(nameof(Overview));
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(member);
        }

        // GET: Member/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Vehicles)
                .FirstOrDefaultAsync(m => m.MemberID == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Member/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberID == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var member = await _context.Members.FindAsync(id);
                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Member deleted successfully!";
                return RedirectToAction(nameof(Overview));
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                TempData["ErrorMessage"] = "Unable to delete the member. Please try again.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }
        }
        #region Legacy code
        //private bool IsValidSocialSecurityNumber(string personalNumber)
        //{
        //    const int validLength = 13;// Remember to Re-execute this 

        //    if (personalNumber.Length != validLength)
        //    {
        //        return false;
        //    }

        //    // Validate the format using a regular expression
        //    string pattern = @"^\d{8}-\d{4}$"; // User is forced to enter -
        //    Regex regex = new Regex(pattern);

        //    return regex.IsMatch(personalNumber);
        //}
        #endregion
        private bool IsValidSocialSecurityNumber(string personalNumber)
        {
            // Remove any hyphens and check if the remaining characters are all digits
            var digitsOnly = personalNumber.Replace("-", "");
            if (!digitsOnly.All(char.IsDigit) || digitsOnly.Length != 12)
            {
                return false;
            }

            // Insert a hyphen after the 8th digit
            personalNumber = digitsOnly.Insert(8, "-");

            // Validate the format using a regular expression
            string pattern = @"^\d{8}-\d{4}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(personalNumber);
        }
    }
}
