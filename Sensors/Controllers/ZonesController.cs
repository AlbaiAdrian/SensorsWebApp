using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Entities;
using Sensors.Data;
using System.Security.Claims;
using AutoMapper;
using Sensors.Models.Zones;
using Sensors.Helpers;
using Sensors.Models.Groups;

namespace Sensors.Controllers;

public class ZonesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly string _userId;

    public ZonesController(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    // List all Zones for a specific Group
    public async Task<IActionResult> Index(int groupId)
    {
        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId && g.UserId == _userId);

        if (group == null)
        {
            return NotFound();
        }

        ViewBag.Group = group;
        var zones = await _context.Zones.Where(z => z.GroupId == groupId).ToListAsync();
        return View(zones);
    }

    // GET: Zones/Details/5
    public async Task<IActionResult> Details(int groupId, int? id)
    {
        if (id == null || _context.Zones == null)
        {
            return NotFound();
        }

        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId && g.UserId == _userId);

        if (group == null)
        {
            return NotFound();
        }

        ViewBag.Group = group;

        var zone = await _context.Zones.FirstOrDefaultAsync(z => z.GroupId == groupId && z.Id == id);
        if (zone == null)
        {
            return NotFound();
        }

        return View(_mapper.Map<DefaultInfoZoneDTO>(zone));
    }

    // GET: Zones/Create
    public async Task<IActionResult> Create(int groupId)
    {
        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId && g.UserId == _userId);

        if (group == null)
        {
            return NotFound();
        }

        ViewBag.Group = group;
        return View();
    }

    // POST: Zones/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int groupId, [Bind("Name,Description")] CreateZoneDTO createZoneDTO)
    {
        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId && g.UserId == _userId);

        if (group == null)
        {
            return NotFound();
        }

        ViewBag.Group = group;

        if (ZoneExistsWithTheSameName(groupId, 0, createZoneDTO.Name))
        {
            ModelState.AddModelError(String.Empty, "A zone with the same name already exists!");
            return View(createZoneDTO);
        }

        if (ModelState.IsValid)
        {
            try
            {
                Zone zone = _mapper.Map<Zone>(createZoneDTO);
                zone.GroupId = groupId;
                zone.ClientSecretKey = RandomStringGenerator.Generate();

                _context.Add(zone);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { groupId });
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the zone. Please try again.");
            }
        }

        return View(createZoneDTO);
    }

    // GET: Zones/Edit/5
    public async Task<IActionResult> Edit(int groupId, int? id)
    {
        if (id == null || _context.Zones == null)
        {
            return NotFound();
        }

        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId && g.UserId == _userId);

        if (group == null)
        {
            return NotFound();
        }

        ViewBag.Group = group;

        var zone = await _context.Zones.FirstOrDefaultAsync(z => z.GroupId == groupId && z.Id == id);
        if (zone == null)
        {
            return NotFound();
        }

        return View(_mapper.Map<EditZoneDTO>(zone));
    }

    // POST: Zones/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int groupId, int id, [Bind("Id,Name,Description")] EditZoneDTO editZoneDTO)
    {
        if (id != editZoneDTO.Id)
        {
            return NotFound();
        }

        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId && g.UserId == _userId);

        if (group == null)
        {
            return NotFound();
        }

        ViewBag.Group = group;

        var zone = await _context.Zones.FirstOrDefaultAsync(z => z.GroupId == groupId && z.Id == id);
        if (zone == null)
        {
            return NotFound();
        }

        if (ZoneExistsWithTheSameName(groupId, editZoneDTO.Id, editZoneDTO.Name))
        {
            ModelState.AddModelError(string.Empty, "A zone with the same name already exists!");
            return View(editZoneDTO);
        }

        if (ModelState.IsValid)
        {
            try
            {
                zone.Name = editZoneDTO.Name;
                zone.Description = editZoneDTO.Description;
                _context.Update(zone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { groupId });
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the zone. Please try again.");
            }
        }
        return View(editZoneDTO);
    }

    // GET: Zones/Delete/5
    public async Task<IActionResult> Delete(int groupId, int? id)
    {
        if (id == null || _context.Zones == null)
        {
            return NotFound();
        }

        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId && g.UserId == _userId);

        if (group == null)
        {
            return NotFound();
        }

        ViewBag.Group = group;

        var zone = await _context.Zones.FirstOrDefaultAsync(z => z.GroupId == groupId && z.Id == id);
        if (zone == null)
        {
            return NotFound();
        }

        return View(_mapper.Map<DefaultInfoZoneDTO>(zone));
    }

    // POST: Zones/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int groupId, int id)
    {
        if (_context.Zones == null)
        {
            return NotFound();
        }

        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId && g.UserId == _userId);

        if (group == null)
        {
            return NotFound();
        }

        ViewBag.Group = group;

        var zone = await _context.Zones.FirstOrDefaultAsync(z => z.GroupId == groupId && z.Id == id);
        if (zone == null)
        {
            return NotFound();
        }

        try
        {
            _context.Zones.Remove(zone);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { groupId });
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "An error occurred while saving the zone. Please try again.");
        }

        return View(_mapper.Map<DefaultInfoZoneDTO>(zone));
    }

    private bool ZoneExistsWithTheSameName(int groupId, int zoneId, string name)
    {
        bool zoneExists = _context.Zones.Any(z => z.GroupId == groupId && z.Id != zoneId && z.Name == name);
        return zoneExists;
    }
}
