using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace PetsOverhaul.Commands
{
    /// <summary>
    /// Returns the name of the PetSlowID with the given integer for the argument.
    /// </summary>
    public class SlowIDCheck : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "petslowid";
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 1 && int.TryParse(args[0], out int result))
            {
                if (PetSlowID.Search.TryGetName(result, out var id))
                {
                    caller.Reply(id);
                    return;
                }
                caller.Reply("Can't find any.");
            }
            else 
            {
                caller.Reply("Give an integer argument to the command.");
            }
        }
    }
    /// <summary>
    /// Returns the ID of the PetSlowID with the given string for the argument.
    /// </summary>
    public class SlowStringCheck : ModCommand
    {
        public override bool IsCaseSensitive => true;
        public override CommandType Type => CommandType.Chat;
        public override string Command => "petslowstring";
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 1)
            {
                if (PetSlowID.Search.TryGetId(args[0], out var id))
                {
                    caller.Reply(id.ToString());
                    return;
                }
            }
                caller.Reply("Can't find any.");
        }
    }
}
