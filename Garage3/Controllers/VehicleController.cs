using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Garage3.Data;
using Garage3.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Garage3.ViewModels;
using Microsoft.Extensions.Options;

namespace Garage3.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ParkingDbContext _context;

        private readonly GarageSettings _garageSettings;

        public VehicleController(ParkingDbContext context, IOptions<GarageSettings> garageSettings)// Constructor to inject GarageSettings dependency Xiahui
        {
            _context = context;
            _garageSettings = garageSettings.Value;
        }


        // Action method to generate a parking receipt Xiahui
        public IActionResult Receipt(int vehicleId, string registrationNumber, DateTime arrivalTime, DateTime departureTime, string vehicleType)
        {
            // Fetch cost per hour based on the vehicle type from GarageSettings
            decimal costPerHour = GetCostPerHourByVehicleType(vehicleType);

            // Create the receipt view model
            var receiptViewModel = new ReceiptViewModel(vehicleId, registrationNumber, arrivalTime, departureTime, costPerHour);

            // Return the view with the receipt view model
            return View(receiptViewModel);
        }

        // Private method to get the cost per hour based on the vehicle type
        private decimal GetCostPerHourByVehicleType(string vehicleType)
        {
            // Retrieve hourly rate based on the specified vehicle type from GarageSettings
            switch (vehicleType.ToLower())
            {
                case "car":
                    return _garageSettings.CarHourlyRate;
                case "motorcycle":
                    return _garageSettings.MotorcycleHourlyRate;
                case "truck":
                    return _garageSettings.TruckHourlyRate;
                case "bus":
                    return _garageSettings.BusHourlyRate;
                case "airplane":
                    return _garageSettings.AirplaneHourlyRate;
                default:
                    // Handle unknown vehicle types or set a default rate
                    return 0;
            }
        }

        // GET: Vehicle/Overview
        #region Old logic keeping as placeholder reference
        //public async Task<IActionResult> Overview(string sortOrder, string searchString)
        //{
        //    // Sorting parameters for the view
        //    ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewBag.TimeSortParam = sortOrder == "time" ? "time_desc" : "time";

        //    // Query to get vehicles and associated parking records where CheckOutTime is null
        //    var vehicles = from v in _context.Vehicles
        //                   join pr in _context.ParkingRecords on v.VehicleID equals pr.VehicleID into parkingRecords
        //                   from pr in parkingRecords.DefaultIfEmpty()
        //                   where pr.CheckOutTime == null
        //                   select new
        //                   {
        //                       Vehicle = v,
        //                       ParkTime = pr.ParkTime
        //                   };

        //    // Filter by search string
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        vehicles = vehicles.Where(v => v.Vehicle.RegistrationNumber.Contains(searchString));
        //    }

        //    // Sorting logic based on sortOrder parameter
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            vehicles = vehicles.OrderByDescending(v => v.Vehicle.RegistrationNumber);
        //            break;
        //        case "time":
        //            vehicles = vehicles.OrderBy(v => v.ParkTime);
        //            break;
        //        case "time_desc":
        //            vehicles = vehicles.OrderByDescending(v => v.ParkTime);
        //            break;
        //        default:
        //            vehicles = vehicles.OrderBy(v => v.Vehicle.RegistrationNumber);
        //            break;
        //    }

        //    /*commented out few lines here to avoid error as we dont have VehicleViewModel at the time 
        //      of creating VehiclesController, so kindly remove those comments 
        //     while creating views and viewmodel and do changes as per your needs*/

        //    // Todo: Create VehicleViewModel and use it instead of anonymous type

        //    var viewModel = vehicles.Select(v => new /*VehicleViewModel*/
        //    {
        //        VehicleID = v.Vehicle.VehicleID,
        //        RegistrationNumber = v.Vehicle.RegistrationNumber,
        //        //ArrivalTime = v.ParkTime ?? DateTime.MinValue   // Use DateTime.MinValue if ParkTime is null
        //    });

        //    var vehiclesList = await viewModel.ToListAsync();

        //    return View("Overview", vehiclesList);
        //}
        #endregion
        public async Task<IActionResult> Overview(string sortOrder, string searchString)
        {
            // Sorting parameters
            ViewData["RegistrationSortParm"] = String.IsNullOrEmpty(sortOrder) ? "registration_desc" : "";
            ViewData["TimeSortParm"] = sortOrder == "time_asc" ? "time_desc" : "time_asc";

            var vehiclesQuery = _context.Vehicles
                .Include(v => v.Owner) // Include Owner for OwnerFullName
                .Include(v => v.VehicleType) // Include VehicleType for VehicleType name
                .Where(v => v.ParkingRecords.Any(pr => pr.CheckOutTime == null))
                .Select(v => new VehicleOverviewViewModel
                {
                    VehicleID = v.VehicleID,
                    RegistrationNumber = v.RegistrationNumber,
                    VehicleType = v.VehicleType.TypeName,
                    OwnerFullName = v.Owner.FirstName + " " + v.Owner.LastName,
                    ParkTime = v.ParkingRecords.Where(pr => pr.CheckOutTime == null).Max(pr => pr.ParkTime),
                    FormattedParkingTime = "" // Placeholder, will be calculated later
                });

            // Filter by search string
            if (!String.IsNullOrEmpty(searchString))
            {
                vehiclesQuery = vehiclesQuery.Where(v => v.RegistrationNumber.Contains(searchString));
            }

            // Apply sorting logic
            switch (sortOrder)
            {
                case "registration_desc":
                    vehiclesQuery = vehiclesQuery.OrderByDescending(v => v.RegistrationNumber);
                    break;
                case "time_asc":
                    vehiclesQuery = vehiclesQuery.OrderBy(v => v.ParkTime);
                    break;
                case "time_desc":
                    vehiclesQuery = vehiclesQuery.OrderByDescending(v => v.ParkTime);
                    break;
                default:
                    vehiclesQuery = vehiclesQuery.OrderBy(v => v.RegistrationNumber);
                    break;
            }

            var vehicleList = await vehiclesQuery.ToListAsync();

            // Calculate formatted parking time for each vehicle
            foreach (var vehicle in vehicleList)
            {
                vehicle.FormattedParkingTime = GetFormattedParkingTime(vehicle.ParkTime);
            }

            return View(vehicleList);
        }

        private string GetFormattedParkingTime(DateTime parkTime)
        {
            var parkingDuration = DateTime.Now - parkTime;
            return parkingDuration.Days > 0
                ? $"est. {parkingDuration.Days} days"
                : $"est. {parkingDuration.Hours} hours";
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
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var vehicle = await _context.Vehicles
        //        .Include(v => v.Owner)
        //        .Include(v => v.VehicleType)
        //        .FirstOrDefaultAsync(m => m.VehicleID == id);

        //    if (vehicle == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(vehicle);
        //}
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var vehicle = await _context.Vehicles
        //        .Include(v => v.Owner)
        //        .Include(v => v.VehicleType)
        //        .FirstOrDefaultAsync(m => m.VehicleID == id);

        //    if (vehicle == null)
        //    {
        //        return NotFound();
        //    }

        //    var viewModel = new VehicleDetailedViewModel
        //    {
        //        Vehicle = vehicle,
        //        VehicleType = vehicle.VehicleType,
        //        ParkingRecord = vehicle.ParkingRecords.FirstOrDefault() 
        //                                                                // Populate other properties if needed
        //    };

        //    return View(viewModel);
        //}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Fetch the vehicle along with the related Owner, VehicleType, and ParkingRecords
            var vehicle = await _context.Vehicles
                .Include(v => v.Owner)
                .Include(v => v.VehicleType)
                .Include(v => v.ParkingRecords)
                .SingleOrDefaultAsync(m => m.VehicleID == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            // Assuming we want the latest parking record for display in the detailed view
            var latestParkingRecord = vehicle.ParkingRecords
                .OrderByDescending(pr => pr.ParkTime)
                .FirstOrDefault();

            // Create the ViewModel
            VehicleDetailedViewModel viewModel = new()
            {
                VehicleID = vehicle.VehicleID,
                RegistrationNumber = vehicle.RegistrationNumber,
                Color = vehicle.Color,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                NumberOfWheels = vehicle.NumberOfWheels,
                OwnerFirstName = vehicle.Owner?.FirstName!, // Use of null-conditional to avoid null reference
                OwnerLastName = vehicle.Owner?.LastName!,
                VehicleTypeName = vehicle.VehicleType?.TypeName!,
                ParkTime = latestParkingRecord?.ParkTime ?? DateTime.MinValue, // Use of null-coalescing to provide default value
                CheckOutTime = latestParkingRecord?.CheckOutTime,
                // Call your method to get formatted parking time, handling the case when there's no parking record
                FormattedParkingDuration = latestParkingRecord != null ? GetFormattedParkingTime(latestParkingRecord) : string.Empty              
            };

            // Pass the ViewModel to the view
            return View(viewModel);
        }
        private string GetFormattedParkingTime(ParkingRecord parkingRecord)
        {
            var duration = (parkingRecord.CheckOutTime ?? DateTime.Now) - parkingRecord.ParkTime;
            return duration.Days > 0
                ? $"{duration.Days} days, {duration.Hours} hours, {duration.Minutes} minutes"
                : $"{duration.Hours} hours, {duration.Minutes} minutes";
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
        //public IActionResult Park(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var vehicle = _context.Vehicles.Find(id);
        //    if (vehicle == null)
        //    {
        //        return NotFound();
        //    }

        //    // Note: This should be irrelevant I believe, the vehicle gets a timestamp when parked
        //    var parkingRecord = new ParkingRecord { VehicleID = vehicle.VehicleID, ParkTime = DateTime.Now };
        //    return View(parkingRecord);
        //}

        // GET: Vehicle/Park/5
        public IActionResult Park(int? id)
        {
            // If no id is provided, prepare the ViewModel with all vehicles and members for selection.
            if (id == null)
            {
                var vehicles = _context.Vehicles.Select(v => new { v.VehicleID, v.RegistrationNumber }).ToList();
                var members = _context.Members.Select(m => new { m.MemberID, MemberName = m.FirstName + " " + m.LastName }).ToList();

                var model = new ParkViewModel
                {
                    ParkTime = DateTime.Now,
                    Vehicles = new SelectList(vehicles, "VehicleID", "RegistrationNumber"),
                    Members = new SelectList(members, "MemberID", "MemberName")
                };

                return View(model);
            }

            // If an id is provided, select the specific vehicle and its owner for parking.
            var vehicle = _context.Vehicles
                .Include(v => v.Owner)
                .SingleOrDefault(v => v.VehicleID == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            // Assuming the member is the owner of the vehicle, fetch the MemberID from the vehicle's OwnerID
            var modelById = new ParkViewModel
            {
                VehicleId = vehicle.VehicleID,
                MemberId = vehicle.OwnerID, // OwnerID from Vehicles table is used for MemberID
                ParkTime = DateTime.Now,
                // Populate the dropdowns even when a specific vehicle is selected
                // to allow changing the selection if needed
                Vehicles = new SelectList(_context.Vehicles, "VehicleID", "RegistrationNumber", vehicle.VehicleID),
                Members = new SelectList(_context.Members, "MemberID", "FirstName", vehicle.OwnerID) // Include LastName if needed
            };

            return View(modelById);
        }


        // POST: Vehicle/Park/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Park(int id, [Bind("VehicleID,MemberID,ParkTime")] ParkingRecord parkingRecord)
        //{
        //    if (id != parkingRecord.VehicleID)
        //    {
        //        return NotFound();
        //    }

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _context.Add(parkingRecord);
        //            await _context.SaveChangesAsync();
        //            TempData["Message"] = "Vehicle parked successfully!";
        //            return RedirectToAction(nameof(Overview));
        //        }
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        // Log the error
        //        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
        //    }

        //    return View(parkingRecord);
        //}

        // POST: Vehicle/Park/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Park(ParkViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var parkingRecord = new ParkingRecord
            {
                VehicleID = model.VehicleId,
                MemberID = model.MemberId, // Use MemberId from the ViewModel
                ParkTime = model.ParkTime
            };

            try
            {
                _context.ParkingRecords.Add(parkingRecord);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Vehicle parked successfully!";
                return RedirectToAction(nameof(Overview)); // Make sure the 'Overview' action exists
            }
            catch (DbUpdateException ex)
            {
                // Log the error (consider using a logging framework)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, see your system administrator.");
                return View(model);
            }
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
