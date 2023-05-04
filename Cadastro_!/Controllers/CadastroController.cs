using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cadastro__.Data;
using Cadastro__.Models;

namespace Cadastro__.Controllers
{
    public class CadastroController : Controller
    {
        private readonly aplicationDatabase _context;

        public CadastroController(aplicationDatabase context)
        {
            _context = context;
        }

        // GET: Cadastro
        public async Task<IActionResult> Index()
        {
              return _context.Clientes != null ? 
                          View(await _context.Clientes.ToListAsync()) :
                          Problem("Entity set 'aplicationDatabase.Clientes'  is null.");
        }

        // GET: Cadastro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var clienteModel = await _context.Clientes
                .FirstOrDefaultAsync(m => m.id == id);
            if (clienteModel == null)
            {
                return NotFound();
            }

            return View(clienteModel);
        }

        // GET: Cadastro/Create
        public IActionResult Create()
        {
            return View();
        }

        //Checar a inscrição estadual
        public bool VerificadorIsento(string inscricaoEstadual)
        {
            if (inscricaoEstadual == null)
            {
                return false;
            }

            return inscricaoEstadual != "ISENTO";
        }

        // laciate ogni speranza, voi ch'entrate
        // POST: Cadastro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,password,name,email,telefone,cadData,tipoPessoa,cpfcnpj,inscEstadual,cadStatus,genero,dataNasc")] clienteModel clienteModel, string confirmPassword)
        {
            // Verificando email e CPF/CNPJ utilizando anyasync para ler o necessário
            bool isEmailExists = await _context.Clientes.AnyAsync(c => c.email == clienteModel.email);
            if (isEmailExists)
            {
                ModelState.AddModelError("email", "O e-mail já está vinculado a outro Comprador.");
            }

            bool isCpfCnpjExists = await _context.Clientes.AnyAsync(c => c.cpfcnpj == clienteModel.cpfcnpj);
            if (isCpfCnpjExists)
            {
                ModelState.AddModelError("cpfcnpj", "Este CPF/CNPJ já está cadastrado para outro Cliente.");
            }

            if (VerificadorIsento(clienteModel.inscEstadual))
            {
                bool isInscricaoEstadualExists = await _context.Clientes.AnyAsync(c => c.inscEstadual == clienteModel.inscEstadual && c.inscEstadual != "ISENTO");
                if (isInscricaoEstadualExists)
                {
                    ModelState.AddModelError("inscEstadual", "Esta Inscrição Estadual já está cadastrada para outro Cliente.");
                }
            }

            if (clienteModel.password != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "A senha e a confirmação de senha não correspondem.");
            }

            if (ModelState.IsValid)
            {
                if (clienteModel != null)
                {
                    _context.AddRange(clienteModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(clienteModel);
        }


        // GET: Cadastro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var clienteModel = await _context.Clientes.FindAsync(id);
            if (clienteModel == null)
            {
                return NotFound();
            }
            return View(clienteModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,password,name,email,telefone,cadData,tipoPessoa,cpfcnpj,inscEstadual,cadStatus,genero,dataNasc")] clienteModel clienteModel, string confirmPassword)
        {
            if (id != clienteModel.id)
            {
                return NotFound();
            }

            // Verificadores marotos
            bool isEmailExists = await _context.Clientes.AnyAsync(c => c.email == clienteModel.email && c.id != clienteModel.id);
            if (isEmailExists)
            {
                ModelState.AddModelError("email", "Este e-mail já está cadastrado para outro Cliente.");
            }

            bool isCpfCnpjExists = await _context.Clientes.AnyAsync(c => c.cpfcnpj == clienteModel.cpfcnpj && c.id != clienteModel.id);
            if (isCpfCnpjExists)
            {
                ModelState.AddModelError("cpfcnpj", "Este CPF/CNPJ já está cadastrado para outro Cliente.");
            }

            if (VerificadorIsento(clienteModel.inscEstadual))
            {
                bool isInscricaoEstadualExists = await _context.Clientes.AnyAsync(c => c.inscEstadual == clienteModel.inscEstadual && c.inscEstadual != "ISENTO" && c.id != clienteModel.id);
                if (isInscricaoEstadualExists)
                {
                    ModelState.AddModelError("inscEstadual", "Esta Inscrição Estadual já está cadastrada para outro Cliente.");
                }
            }

            if (!string.Equals(clienteModel.password, confirmPassword))
            {
                ModelState.AddModelError("confirmPassword", "A senha e a confirmação de senha não correspondem.");
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clienteModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!clienteModelExists(clienteModel.id))
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

            return View(clienteModel);
        }



        // GET: Cadastro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var clienteModel = await _context.Clientes
                .FirstOrDefaultAsync(m => m.id == id);
            if (clienteModel == null)
            {
                return NotFound();
            }

            return View(clienteModel);
        }

        // POST: Cadastro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'aplicationDatabase.Clientes'  is null.");
            }
            var clienteModel = await _context.Clientes.FindAsync(id);
            if (clienteModel != null)
            {
                _context.Clientes.Remove(clienteModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool clienteModelExists(int id)
        {
          return (_context.Clientes?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
