using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Entities;
using Sensors.Data;
using AutoMapper;
using Sensors.Helpers;
using Sensors.Models.Sensors;
using System.Text.RegularExpressions;
using Sensors.Models.Zones;

namespace Sensors.Controllers
{
    public class SensorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SensorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Sensors
        public async Task<IActionResult> Index(int zoneId)
        {
            Zone zone = await _context.Zones.FirstOrDefaultAsync(z => z.Id == zoneId);
            if (zone == null)
            {
                return NotFound();
            }
            ViewBag.Zone = zone;

            var zones = await _context.Sensor.Where(s => s.ZoneId == zoneId).ToListAsync();

            return View(zones);
        }

        // GET: Sensors/Details/5
        public async Task<IActionResult> Details(int zoneId, int? id)
        {
            if (id == null || _context.Sensor == null)
            {
                return NotFound();
            }

            var zone = await _context.Zones.FirstOrDefaultAsync(z => z.Id == zoneId);

            if (zone == null)
            {
                return NotFound();
            }

            ViewBag.Zone = zone;

            var sensor = await _context.Sensor.FirstOrDefaultAsync(m => m.Id == id);

            if (sensor == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<DefaultInfoSensorDTO>(sensor));
        }

        // GET: Sensors/Create
        public IActionResult Create(int zoneId)
        {
            var zone = _context.Zones.FirstOrDefault(z => z.Id == zoneId);

            if (zone == null)
            {
                return NotFound();
            }

            ViewBag.Zone = zone;
            return View();
        }

        // POST: Sensors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int zoneId, [Bind("Name,Description")] CreateSensorDTO createSensorDTO)
        {
            var zone = await _context.Zones.FirstOrDefaultAsync(z => z.Id == zoneId);

            if (zone == null)
            {
                return NotFound();
            }
            ViewBag.Zone = zone;

            if (SensorExistsWithTheSameName(zoneId, 0, createSensorDTO.Name))
            {
                ModelState.AddModelError(string.Empty, "A sensor with the same name already exists!");
                return View(createSensorDTO);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Sensor sensor = _mapper.Map<Sensor>(createSensorDTO);
                    sensor.ZoneId = zoneId;
                    sensor.ClientSecretKey = RandomStringGenerator.Generate();

                    _context.Add(sensor);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index), new { zoneId });
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the sensor. Please try again.");
                }
            }

            return View(createSensorDTO);
        }

        // GET: Sensors/Edit/5
        public async Task<IActionResult> Edit(int zoneId, int? id)
        {
            if (id == null || _context.Sensor == null)
            {
                return NotFound();
            }

            var zone = await _context.Zones.FirstOrDefaultAsync(z => z.Id == zoneId);

            if (zone == null)
            {
                return NotFound();
            }

            ViewBag.Zone = zone;

            var sensor = await _context.Sensor.FindAsync(id);
            if (sensor == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<EditSensorDTO>(sensor));
        }

        // POST: Sensors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int zoneId, int id, [Bind("Name,Description,Id")] EditSensorDTO editSensorDTO)
        {
            if (id != editSensorDTO.Id)
            {
                return NotFound();
            }

            var zone = await _context.Zones.FirstOrDefaultAsync(z => z.Id == zoneId);

            if (zone == null)
            {
                return NotFound();
            }

            ViewBag.Zone = zone;

            Sensor sensor = await _context.Sensors.FirstOrDefaultAsync(s => s.ZoneId == zoneId && s.Id == id);
            if (sensor == null)
            {
                return NotFound();
            }

            if (SensorExistsWithTheSameName(zoneId, id, editSensorDTO.Name))
            {
                ModelState.AddModelError(string.Empty, "A sensor with the same name already exists!");
                return View(editSensorDTO);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    sensor.Name = editSensorDTO.Name;
                    sensor.Description = editSensorDTO.Description;
                    _context.Update(sensor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { zoneId });
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the sensor. Please try again.");
                }
            }
            return View(editSensorDTO);
        }

        // GET: Sensors/Delete/5
        public async Task<IActionResult> Delete(int zoneId, int? id)
        {
            if (id == null || _context.Sensor == null)
            {
                return NotFound();
            }

            var zone = await _context.Zones.FirstOrDefaultAsync(z => z.Id == zoneId);
            if (zone == null)
            {
                return NotFound();
            }

            ViewBag.Zone = zone;

            var sensor = await _context.Sensor.FirstOrDefaultAsync(s => s.ZoneId == zoneId && s.Id == id);
            if (sensor == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<DefaultInfoSensorDTO>(sensor));
        }

        // POST: Sensors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int zoneId, int id)
        {
            if (_context.Sensor == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sensor'  is null.");
            }

            var zone = await _context.Zones.FirstOrDefaultAsync(z => z.Id == zoneId);
            if (zone == null)
            {
                return NotFound();
            }

            var sensor = await _context.Sensor.FirstOrDefaultAsync(s => s.ZoneId == zoneId && s.Id == id);
            if (sensor == null)
            {
                return NotFound();
            }
            try
            {
                _context.Sensor.Remove(sensor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { zoneId });
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the sensor. Please try again.");
            }

            return View(_mapper.Map<DefaultInfoZoneDTO>(sensor));
        }


        private bool SensorExistsWithTheSameName(int zoneId, int sensorId, string name)
        {
            bool sensorExists = _context.Sensors.Any(s => s.ZoneId == zoneId && s.Id != sensorId && s.Name == name);
            return sensorExists;
        }
    }
}
