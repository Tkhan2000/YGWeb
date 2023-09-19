using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace YGWeb.Areas.Identity.Data;

// Add profile data for application users by adding properties to the YGWebUser class
public class YGWebUser : IdentityUser
{
    // Create a list of decks. Each deck can be represented as a list of card id's (or ints).
    // Therefore, a list of user deck can be represented as a list of list of ints.
    // public List<List<int>> Decks { get; set;}
}

