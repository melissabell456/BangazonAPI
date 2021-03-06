using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Data;
using BangazonAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//Written by: Matt Augsburger
namespace BangazonAPI.Controllers
{
    // Class to PUT/POST/GET/DELETE product types to the Bangazon API.
    [Route("[controller]")]
    [EnableCors("AllowOnlyBangazonians")]
    public class ProductTypeController : Controller
    {
        //Sets up an empty variable _context that will  be a reference of our BangazonAPIContext class
        private BangazonAPIContext _context;
        // Constructor method to create an instance of context to communicate with our database.
        public ProductTypeController(BangazonAPIContext ctx)
        {
            _context = ctx;
        }

        // GET url/ProductType
        // Returns List of ProductTypes if any exist
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<object> productTypes = from productType in _context.ProductType select productType;

            if (productTypes == null)
            {
                return NotFound();
            }

            return Ok(productTypes);

        }

        // GET url/ProductYpe/{id}
        // Returns a Specific ProductType correlating to id

        [HttpGet("{id}", Name = "GetSingleProductType")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ProductType productType = _context.ProductType.Single(m => m.ProductTypeID == id);

                if (productType == null)
                {
                    return NotFound();
                }
                
                return Ok(productType);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound(ex);
            }
        }

        // POST url/ProductType
        // Creates a ProductType in the database
        // Requires an Object:
        // {
        //     "Name": "Example"
        // }

        [HttpPost]
        public IActionResult Post([FromBody] ProductType newProductType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ProductType.Add(newProductType);
            
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ProductTypeExists(newProductType.ProductTypeID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetSingleProductType", new { id = newProductType.ProductTypeID }, newProductType);
        }

        private bool ProductTypeExists(int productTypeID)
        {
          return _context.ProductType.Count(e => e.ProductTypeID == productTypeID) > 0;
        }
        // PUT url/ProductType/{id}
        // Edits a Specific ProductType from the database correlating to the id
        // Requires an Object
        // {
        //     "ProductTypeID": 1,
        //     "Name": "Example"

        // }
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ProductType modifiedProductType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modifiedProductType.ProductTypeID)
            {
                return BadRequest();
            }

            _context.Entry(modifiedProductType).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }
        // DELETE url/ProductType/{id}
        // Removes a specific ProductType from the database
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductType productType = _context.ProductType.Single(m => m.ProductTypeID == id);
            if (productType == null)
            {
                return NotFound();
            }

            _context.ProductType.Remove(productType);
            _context.SaveChanges();

            return Ok(productType);
        }

    }
}