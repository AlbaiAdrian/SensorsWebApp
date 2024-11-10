using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities;
using Sensors.Data;
using System.Security.Claims;
using Sensors.Models.Groups;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Sensors.Controllers;

[Authorize]
public class GroupsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly string _userId;

    public GroupsController(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    // GET: Groups
    public async Task<IActionResult> Index()
    {
        var groupZoneCounts = await _context.Groups.Select(g => new
        {
            Group = g,
            ZoneCount = _context.Zones.Count(z => z.GroupId == g.Id)
        }).Where(g => g.Group.UserId == _userId).ToListAsync();

        var listGroupDTOs = groupZoneCounts.Select(g => new ListGroupDTO() { Id = g.Group.Id, Name = g.Group.Name, Description = g.Group.Description, Zones = g.ZoneCount });
        return View(listGroupDTOs);
    }

    // GET: Groups/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Groups == null)
        {
            return NotFound();
        }

        var @group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id && g.UserId == _userId);
        if (@group == null)
        {
            return NotFound();
        }
        DefaultInfoGroupDTO defaultInfoGroupDTO = _mapper.Map<DefaultInfoGroupDTO>(@group);

        return View(defaultInfoGroupDTO);
    }

    // GET: Groups/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Groups/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description")] CreateGroupDTO groupDTO)
    {
        if (GroupExistsWithTheSameName(0, groupDTO.Name))
        {
            ModelState.AddModelError(String.Empty, "A group with the same name already exists!");
            return View(groupDTO);
        }

        if (ModelState.IsValid)
        {
            try
            {
                Group @group = _mapper.Map<Group>(groupDTO);
                @group.UserId = _userId;

                _context.Add(@group);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the group. Please try again.");
            }
        }

        return View(groupDTO);
    }

    // GET: Groups/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Groups == null)
        {
            return NotFound();
        }

        var @group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id && g.UserId == _userId);
        if (@group == null)
        {
            return NotFound();
        }

        DefaultInfoGroupDTO defaultInfoGroupDTO = _mapper.Map<DefaultInfoGroupDTO>(@group);

        return View(defaultInfoGroupDTO);
    }

    // POST: Groups/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Name,Description, Id")] DefaultInfoGroupDTO defaultInfoGroupDTO)
    {
        if (id != defaultInfoGroupDTO.Id)
        {
            return NotFound();
        }

        if (GroupExistsWithTheSameName(id, defaultInfoGroupDTO.Name))
        {
            ModelState.AddModelError(String.Empty, "A group with the same name already exists!");
            return View(defaultInfoGroupDTO);
        }

        if (ModelState.IsValid)
        {
            try
            {
                Group @group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id && g.UserId == _userId);
                if (null == @group)
                {
                    return NotFound();
                }

                @group.Name = defaultInfoGroupDTO.Name;
                @group.Description = defaultInfoGroupDTO.Description;

                _context.Update(@group);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError(String.Empty, "Update failed, please reload and try again!");
            }
            return RedirectToAction(nameof(Index));
        }

        return View(defaultInfoGroupDTO);
    }

    // GET: Groups/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Groups == null)
        {
            return NotFound();
        }

        var @group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id && g.UserId == _userId);
        if (@group == null)
        {
            return NotFound();
        }

        DefaultInfoGroupDTO defaultInfoGroupDTO = _mapper.Map<DefaultInfoGroupDTO>(@group);

        return View(defaultInfoGroupDTO);
    }

    // POST: Groups/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Groups == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Groups'  is null.");
        }
        var @group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id && g.UserId == _userId);
        if (@group != null)
        {
            _context.Groups.Remove(@group);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GroupExistsWithTheSameName(int id, string name)
    {
        bool groupExists = _context.Groups.Any(g => g.UserId == _userId && g.Id != id && g.Name == name);
        return groupExists;
    }
}