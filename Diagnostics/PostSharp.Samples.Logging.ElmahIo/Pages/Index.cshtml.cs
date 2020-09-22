using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Serilog;

namespace PostSharp.Samples.Logging.ElmahIo.Pages
{
  public class IndexModel : PageModel
  {
    [BindProperty]
    public string Result
    {
      get;
      set;
    }

    [BindProperty] 
    public string NumberOfKgs
    {
      get;
      set;
    }

    public void OnPost()
    {
      this.WriteInterestingFacts();
    }

    public void OnGet()
    {

      this.WriteInterestingFacts();
    }

    private void WriteInterestingFacts()
    {
      try
      {
        float kgs = this.GetKilograms(this.NumberOfKgs);
        this.Result =
          "You weigh " + kgs + " kilograms.\n" +
          "You would sell for " + WorthInGold(kgs) + " if you were made of gold.\n" +
          "You would explode as " + Explosion(kgs) + " atomic bombs if you were turned into pure energy.\n" +
          "You would fall for " + EiffelFall(kgs) + " seconds if you fell from the Eiffel Tower.";
      }
      catch (Exception)
      {
        this.Result = "I can't tell you anything about your body.";
      }
    }

    private string EiffelFall(in float kgs) // Ha ha, your mass doesn't actually matter ^^
    {
      int distance = 300; // m
      // distance = acceleration * time squared
      // time = square root of (distance / acceleration)
      float time = MathF.Sqrt((float) distance / 9.81f);
      return time.ToString("F2");
    }

    private string Explosion(in float kgs)
    {
      // e = m * c * c
      BigInteger c = 299792458;
      BigInteger cc = c * c;
      BigInteger eTimes1000 = (cc) * new BigInteger((int) (kgs * 1000)); // reasonable accuracy
      BigInteger e = eTimes1000 / 1000;
      BigInteger atomicEnergy = new BigInteger(100) * 1000 * 1000 * 1000 * 1000;
      BigInteger bombs = e / atomicEnergy;
      return bombs.ToString();
    }

    private string WorthInGold(in float kgs)
    {
      float costOfKgOfGold = 61612; // USD as of September 22, 2020
      return "$" + (kgs * costOfKgOfGold);
    }

    private float GetKilograms(string numberOfStars)
    {
      return float.Parse(numberOfStars, this.GetUserCulture());
    }

    private CultureInfo GetUserCulture()
    {
      var locale = this.Request.HttpContext.Features.Get<IRequestCultureFeature>();
      return locale.RequestCulture.Culture;
    }
  }
}