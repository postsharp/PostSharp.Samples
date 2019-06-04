using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MicroserviceExample.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ValuesController : ControllerBase
  {
    private readonly ConcurrentDictionary<int, string> values = new ConcurrentDictionary<int, string>();

    public ValuesController()
    {
      this.values.TryAdd(1, "default");
    }

    // GET api/values
    [HttpGet]
    public ActionResult<IEnumerable<KeyValuePair<int, string>>> Get()
    {
      return this.values;
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public ActionResult<string> Get(int id)
    {
      if (this.values.TryGetValue(id, out var value))
      {
        return value;
      }
      else
      {
        return this.NotFound();
      }

    }


    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
      if (value.Length > 5)
      {
        throw new ArgumentOutOfRangeException(nameof(value));
      }

      this.values[id] = value;
    }

    // POST api/values/5
    [HttpPost("{id}")]
    public void Post(int id, [FromBody] string value)
    {
      if (value.Length > 5)
      {
        throw new ArgumentOutOfRangeException(nameof(value));
      }

      if (!this.values.TryAdd(id, value))
      {
        this.Conflict();
      }
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      if (!this.values.TryRemove(id, out _))
      {
        this.NotFound();
      }
    }
  }
}
