using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Garage3.Data;
using Garage3.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage3.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ParkingDbContext _context;

        public VehicleController(ParkingDbContext context)
        {
            _context = context;
        }

        // GET: Vehicle/Overview
        public async Task<IActionResult> Overview(string sortOrder, string searchString)
        {
            // Sorting parameters for the view
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TimeSortParam = sortOrder == "time" ? "time_desc" : "time";

            // Query to get vehicles and associated parking records where CheckOutTime is null
            var vehicles = from v in _context.Vehicles
                           join pr in _context.ParkingRecords on v.VehicleID equals pr.VehicleID into parkingRecords
                           from pr in parkingRecords.DefaultIfEmpty()
                           where pr.CheckOutTime == null
                           select new
                           {
                               Vehicle = v,
                               ParkTime = pr.ParkTime
                           };

            // Filter by search string
            if (!String.IsNullOrEmpty(searchString))
            {
                vehicles = vehicles.Where(v => v.Vehicle.RegistrationNumber.Contains(searchString));
            }

            // Sorting logic based on sortOrder parameter
            switch (sortOrder)
            {
                case "name_desc":
                    vehicles = vehicles.OrderByDescending(v => v.Vehicle.RegistrationNumber);
                    break;
                case "time":
                    vehicles = vehicles.OrderBy(v => v.ParkTime);
                    break;
                case "time_desc":
                    vehicles = vehicles.OrderByDescending(v => v.ParkTime);
                    break;
                default:
                    vehicles = vehicles.OrderBy(v => v.Vehicle.RegistrationNumber);
                    break;
            }

            /*commented out few lines here to avoid error as we dont have VehicleViewModel at the time 
              of creating VehiclesController, so kindly remove those comments 
             while creating views and viewmodel and do changes as per your needs*/

            var viewModel = vehicles.Select(v => new /*VehicleViewModel*/
            {
                VehicleID = v.Vehicle.VehicleID,
                RegistrationNumber = v.Vehicle.RegistrationNumber,
                //ArrivalTime = v.ParkTime ?? DateTime.MinValue   // Use DateTime.MinValue if ParkTime is null
            });

            var vehiclesList = await viewModel.ToListAsync();

            return View("Overview", vehiclesList);
        }


        // GET: Vehicle/Add
        public IActionResult Add()
        {
            PopulateDropDownLists();
            return View();
        }

        // POST: Vehicle/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("OwnerID,RegistrationNumber,VehicleTypeID,Color,Brand,Model,NumberOfWheels")] Vehicle vehicle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(vehicle); //add new vehicle to DB
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Vehicle added successfully!";
                    return RedirectToAction(nameof(Overview));
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            PopulateDropDownLists(vehicle.VehicleTypeID);
            return View(vehicle);
        }

        // GET: Vehicle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            PopulateDropDownLists(vehicle.VehicleTypeID);
            return View(vehicle);
        }

        // POST: Vehicle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VehicleID,OwnerID,RegistrationNumber,VehicleTypeID,Color,Brand,Model,NumberOfWheels")] Vehicle vehicle)
        {
            if (id != vehicle.VehicleID)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(vehicle); //update edited vehicle into DB
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Vehicle edited successfully!";
                    return RedirectToAction(nameof(Overview));
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            PopulateDropDownLists(vehicle.VehicleTypeID);
            return View(vehicle);
        }

        // GET: Vehicle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.Owner)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.VehicleID == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.Owner)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.VehicleID == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var vehicle = await _context.Vehicles.FindAsync(id); // Find & remove vehicle from DB
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Vehicle deleted successfully!";
                return RedirectToAction(nameof(Overview));
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                TempData["ErrorMessage"] = "Unable to delete the vehicle. Please try again.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }
        }

        // GET: Vehicle/Park/5
        public IActionResult Park(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = _context.Vehicles.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            var parkingRecord = new ParkingRecord { VehicleID = vehicle.VehicleID, ParkTime = DateTime.Now };
            return View(parkingRecord);
        }

        // POST: Vehicle/Park/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Park(int id, [Bind("VehicleID,MemberID,ParkTime")] ParkingRecord parkingRecord)
        {
            if (id != parkingRecord.VehicleID)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(parkingRecord);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Vehicle parked successfully!";
                    return RedirectToAction(nameof(Overview));
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(parkingRecord);
        }

        // GET: Vehicle/Unpark/5
        public async Task<IActionResult> Unpark(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingRecord = await _context.ParkingRecords
                .Include(pr => pr.Vehicle)
                .Include(pr => pr.Member)
                .FirstOrDefaultAsync(m => m.VehicleID == id && m.CheckOutTime == null);

            if (parkingRecord == null)
            {
                TempData["Message"] = "Vehicle is not currently parked.";
                return RedirectToAction(nameof(Overview));
            }

            return View(parkingRecord);
        }

        // POST: Vehicle/Unpark/5
        [HttpPost, ActionName("Unpark")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnparkConfirmed(int id)
        {
            var parkingRecord = await _context.ParkingRecords.FindAsync(id);

            try
            {
                if (ModelState.IsValid)
                {
                    parkingRecord.CheckOutTime = DateTime.Now;
                    _context.Update(parkingRecord);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Vehicle unparked successfully!";
                    return RedirectToAction(nameof(Overview));
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(parkingRecord);
        }

        private void PopulateDropDownLists(object selectedType = null)
        {
            // Query to get vehicle types for dropdown
            var typesQuery = from t in _context.VehicleTypes
                             orderby t.TypeName
                             select t;
            // Populate the ViewBag with the dropdown data
            ViewBag.VehicleTypeID = new SelectList(typesQuery.AsNoTracking(), "VehicleTypeID", "TypeName", selectedType);
        }
    }
}
