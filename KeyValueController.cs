using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace RESTful
{
    [ApiController]
    [Route("1")]
    public class KeyValueController : ControllerBase
    {
        private readonly KeyValueRepository _repository;

        public KeyValueController(KeyValueRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{key}")]
        public async Task<ActionResult<KeyValue>> Get(string key)
        {
            var keyValue = await _repository.GetValueAsync(key);
            if (keyValue == null)
            {
                return NotFound(new { message = "Not found." });
            }

            return keyValue;
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            bool success = await _repository.DeleteValueAsync(key);
            if (success)
            {
                return Ok(new { message = "Value deleted successfully." });
            }
            else
            {
                return NotFound(new { message = "Value not found." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] KeyValue keyValue)
        {
            if (keyValue.Value == null)
            {
                return BadRequest(new { message = "Invalid value."});
            }

            await _repository.SaveValueAsync(keyValue.Key, keyValue.Value);
            return Ok(new { message = "Value saved successfully.", key = keyValue.Key, value = keyValue.Value });
        }
    }
}
