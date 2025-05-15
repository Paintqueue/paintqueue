using HW4NoteKeeperEx2.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PainterQueueApi.Infrastructure.Database;
using PainterQueueApi.Infrastructure.Interfaces;
using PainterQueueApi.Models;
using System.Text;

namespace PainterQueueApi.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing rules.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RuleController : BaseController<Rule>
    {
        private readonly PaintQueueDbContext _context;

        public RuleController(PaintQueueDbContext context, ICustomTelemetryClient<Rule> tracker)
            : base(tracker)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all rules from the data store.
        /// </summary>
        /// <remarks>
        /// Returns a list of all rules currently available in the database.
        /// </remarks>
        /// <response code="200">Returns a list of rules</response>
        /// <response code="500">Returns a ProblemDetails response if an internal server error occurs</response>
        /// <returns>A list of rules</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RuleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var rules = await _context.Rules.ToListAsync();
                var dtos = rules.Select(r => r.ToRuleDto()).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return InternalServerErrorExceptionCustomResponse(ex);
            }

        }


        /// <summary>
        /// Retrieves a rule by its unique identifier.
        /// </summary>
        /// <remarks>
        /// Returns the rule matching the provided <paramref name="id"/>. If the rule does not exist, a 404 response is returned.
        /// </remarks>
        /// <param name="id">The unique identifier of the rule to retrieve.</param>
        /// <response code="200">Returns the requested rule</response>
        /// <response code="400">Returned if the ID is empty or invalid</response>
        /// <response code="404">Returned if no rule is found with the specified ID</response>
        /// <response code="500">Returns a ProblemDetails response if an internal server error occurs</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(RuleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequestCustomResponse("Invalid ID", "ID cannot be empty");

                var rule = await _context.Rules.FindAsync(id);

                if (rule == null)
                    return NotFoundCustomResponse("Rule not found", $"No rule found with ID {id}");

                return Ok(rule.ToRuleDto());
            }
            catch (Exception ex)
            {
                return InternalServerErrorExceptionCustomResponse(new Rule() { Id = id }, ex);
            }
        }

        /// <summary>
        /// Creates a new rule in the data store.
        /// </summary>
        /// <remarks>
        /// Accepts a rule object and creates a new entry in the database. Automatically sets the creation and update timestamps.
        /// </remarks>
        /// <param name="dto">The rule to create</param>
        /// <response code="201">Returns the newly created rule</response>
        /// <response code="400">Returned if the request body is invalid</response>
        /// <response code="500">Returns a ProblemDetails response if an internal server error occurs</response>

        [HttpPost]
        [ProducesResponseType(typeof(RuleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] RuleDto dto)
        {
            try
            { 
                dto.Id = Guid.NewGuid();

                if (!ModelState.IsValid)
                {
                    var sb = new StringBuilder();
                    var additionalData = new Dictionary<string, object>()
                    {
                        { "ModelState", ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()) }
                    };

                    sb.AppendLine("One or more fields are invalid. Errors:");
                    foreach (var kvp in ModelState)
                    {
                        sb.AppendLine($"{kvp.Key}: {string.Join(", ", kvp.Value.Errors.Select(e => e.ErrorMessage))}");
                    }

                    return BadRequestCustomResponse("Invalid rule", sb.ToString(), dto.ToRule(), additionalData);
                }

                var rule = dto.ToRule();
                rule.Created = DateTimeOffset.UtcNow;
                rule.Updated = DateTimeOffset.UtcNow;

                _context.Rules.Add(rule);
                await _context.SaveChangesAsync();
                
                return Ok(rule.ToRuleDto());
            }
            catch (Exception ex)
            {
                return InternalServerErrorExceptionCustomResponse(dto.ToRule(), ex);
            }
        }

        /// <summary>
        /// Updates an existing rule in the data store.
        /// </summary>
        /// <remarks>
        /// Updates a rule's details based on its unique ID. Returns 204 if successful, 404 if the rule does not exist, or 400 if the ID is mismatched.
        /// </remarks>
        /// <param name="id">The ID of the rule to update</param>
        /// <param name="dto">The updated rule data</param>
        /// <response code="204">Rule updated successfully</response>
        /// <response code="400">Returned if the ID is missing or mismatched with the payload</response>
        /// <response code="404">Returned if no rule is found with the specified ID</response>
        /// <response code="500">Returns a ProblemDetails response if an internal server error occurs</response>

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] RuleDto dto)
        {
            if (id != dto.Id)
                return BadRequestCustomResponse("Mismatched ID", "URL ID does not match payload ID", dto.ToRule());

            var rule = await _context.Rules.FindAsync(id);
            
            if (rule == null)
                return NotFoundCustomResponse("Rule not found", $"No rule found with ID {id}", dto.ToRule());

            rule.Name = dto.Name;
            rule.Description = dto.Description;
            rule.InternalDescription = dto.InternalDescription;
            rule.Page = dto.Page;
            rule.Updated = DateTimeOffset.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                
                return NoContentLogCustomResponse("Rule updated", $"Rule with ID {id} updated", dto.ToRule());
            }
            catch (Exception ex)
            {
                return InternalServerErrorExceptionCustomResponse(dto.ToRule(), ex);
            }
        }

        /// <summary>
        /// Deletes a rule from the data store.
        /// </summary>
        /// <remarks>
        /// Removes the rule with the specified ID. If no matching rule is found, a 404 response is returned.
        /// </remarks>
        /// <param name="id">The ID of the rule to delete</param>
        /// <response code="204">Rule deleted successfully</response>
        /// <response code="404">Returned if no rule is found with the specified ID</response>
        /// <response code="500">Returns a ProblemDetails response if an internal server error occurs</response>

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var rule = await _context.Rules.FindAsync(id);
            if (rule == null)
                return NotFoundCustomResponse("Rule not found", $"No rule found with ID {id}");

            try
            {
                _context.Rules.Remove(rule);
                await _context.SaveChangesAsync();
                return NoContentLogCustomResponse("Rule deleted", $"Rule with ID {id} deleted");
            }
            catch (Exception ex)
            {
                return InternalServerErrorExceptionCustomResponse(ex);
            }
        }
    }
}
