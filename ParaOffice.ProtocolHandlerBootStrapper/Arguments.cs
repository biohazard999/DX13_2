using System.Collections;
using System.Text.RegularExpressions;

namespace ParaOffice.Win.ProtocolHandlerBootStrapper
{

  /// <summary>
  /// parses the commandline arguments
  /// </summary>
  public class Arguments : Hashtable
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="Arguments"/> class.
    /// </summary>
    /// <param name="Args">The args.</param>
    public Arguments(string[] Args)
    {
      Regex regExSplitter = new Regex(@"^-{1,2}|^/|=|:",
          RegexOptions.IgnoreCase | RegexOptions.Compiled);

      Regex regExCleaner = new Regex(@"^['""]?(.*?)['""]?$",
          RegexOptions.IgnoreCase | RegexOptions.Compiled);

      string parameter = null;
      string[] parts;

      foreach (string Txt in Args)
      {
        parts = regExSplitter.Split(Txt, 3);

        switch (parts.Length)
        {
          case 1:
            if (parameter != null)
            {
              if (!this.ContainsKey(parameter))
              {
                parts[0] =
                    regExCleaner.Replace(parts[0], "$1");

                this.Add(parameter, parts[0]);
              }
              parameter = null;
            }
            break;

          case 2:
            if (parameter != null)
            {
              if (!this.ContainsKey(parameter))
                this.Add(parameter, "true");
            }
            parameter = parts[1];
            break;
          case 3:
            if (parameter != null)
            {
              if (!this.ContainsKey(parameter))
                this.Add(parameter, "true");
            }
            parameter = parts[1];

            if (!this.ContainsKey(parameter))
            {
              parts[2] = regExCleaner.Replace(parts[2], "$1");
              this.Add(parameter, parts[2]);
            }

            parameter = null;
            break;
        }
      }

      if (parameter != null)
      {
        if (!this.ContainsKey(parameter))
          this.Add(parameter, "true");
      }
    }




  }
}
