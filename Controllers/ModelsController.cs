using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using C__laba_2_console_traffic_police.DAL;
using C__laba_2_console_traffic_police.Models;
using System.Web;
using NuGet.Protocol.Core.Types;

namespace web_police.Controllers
{
    public class ModelsController : Controller
    {
        private readonly TrafficPoliceContext _context;

        public ModelsController(TrafficPoliceContext context)
        {
            _context = context;
        }

        // GET: Models
        public async Task<IActionResult> Index()
        {
              return _context.Models != null ? 
                          View(await _context.Models.ToListAsync()) :
                          Problem("Entity set 'TrafficPoliceContext.Models'  is null.");
        }

        // GET: Models/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Models == null)
            {
                return NotFound();
            }

            var model = await _context.Models
                .FirstOrDefaultAsync(m => m.ModelId == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Models/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Models/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModelId,ModelName,MarkId,imgPath")] Model model, List<IFormFile> imgPath)
        {
            foreach (var item in imgPath)
            {
                if (item.Length > 0)
                {
                    using (var Stream = new MemoryStream())
                    {
                        await item.CopyToAsync(Stream);
                        model.imgPath = Stream.ToArray();
                    }
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(model);
                 await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Models/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Models == null)
            {
                return NotFound();
            }

            var model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        public FileContentResult GetImage(int modelId)
        {
            Model model = _context.Models.FirstOrDefault(g => g.ModelId == modelId);

            if (model != null)
            {
                return File(model.imgPath, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        // POST: Models/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ModelId,ModelName,MarkId,imgPath")] Model model, IFormFile image)
        {
            if (id != model.ModelId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (image != null)
                    {
                        model.imgPath = new byte[image.Length];
                        image.OpenReadStream().Read(model.imgPath, 0, (int)image.Length);
                    }
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    TempData["message"] = string.Format("Изменения в модели \"{0}\" были сохранены", model.ModelName);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelExists(model.ModelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Models/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Models == null)
            {
                return NotFound();
            }

            var model = await _context.Models
                .FirstOrDefaultAsync(m => m.ModelId == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Models/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Models == null)
            {
                return Problem("Entity set 'TrafficPoliceContext.Models'  is null.");
            }
            var model = await _context.Models.FindAsync(id);
            if (model != null)
            {
                _context.Models.Remove(model);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModelExists(int? id)
        {
          return (_context.Models?.Any(e => e.ModelId == id)).GetValueOrDefault();
        }
    }
}
