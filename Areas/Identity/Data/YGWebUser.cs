using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using YGWeb.Models;

namespace YGWeb.Areas.Identity.Data;

// Add profile data for application users by adding properties to the YGWebUser class
public class YGWebUser : IdentityUser
{
    public List<Deck> savedDecks { get; set; }
}

