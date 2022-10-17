using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using Sandwich2Go.Data;
using Sandwich2Go.Models;

namespace Sandwich2Go.Controllers
{
    public class PedidoProvsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidoProvsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PedidoProvs
        public async Task<IActionResult> Index(string SearchString)
        {
            if (!String.IsNullOrEmpty(SearchString))
            {
                var pedprov = _context.PedidoProv.
                    Where(s => s.Nombre.Contains(SearchString)).
                    OrderBy(m => m.Nombre);
                return View(await pedprov.ToListAsync());

            }
            else
            {
                return View(await _context.PedidoProv.ToListAsync());
            }
            
        }

        // GET: PedidoProvs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoProv = await _context.PedidoProv
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidoProv == null)
            {
                return NotFound();
            }

            return View(pedidoProv);
        }

        // GET: PedidoProvs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PedidoProvs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,FechaPedido")] PedidoProv pedidoProv)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedidoProv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pedidoProv);
        }

        // GET: PedidoProvs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoProv = await _context.PedidoProv.FindAsync(id);
            if (pedidoProv == null)
            {
                return NotFound();
            }
            return View(pedidoProv);
        }

        // POST: PedidoProvs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,FechaPedido")] PedidoProv pedidoProv)
        {
            if (id != pedidoProv.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidoProv);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoProvExists(pedidoProv.Id))
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
            return View(pedidoProv);
        }

        // GET: PedidoProvs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoProv = await _context.PedidoProv
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidoProv == null)
            {
                return NotFound();
            }

            return View(pedidoProv);
        }

        // POST: PedidoProvs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedidoProv = await _context.PedidoProv.FindAsync(id);
            _context.PedidoProv.Remove(pedidoProv);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoProvExists(int id)
        {
            return _context.PedidoProv.Any(e => e.Id == id);
        }
    }
}
