
using OOP_Lab_1.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OOP_Lab_1.Core.Interfaces
{
    public interface IDatabaseService
    {
        /// <summary>
        /// Adds a new user to the database using a raw SQL query.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <returns>True if the user was added successfully; otherwise, false.</returns>
        Task<bool> AddUserAsync(User user, string bankId);

        /// <summary>
        /// Checks if a user with the specified email already exists in the database using a raw SQL query.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the user exists; otherwise, false.</returns>
        Task<bool> UserExistsByEmailAsync(string email, string bankId);

        /// <summary>
        /// Retrieves all users from the database using a raw SQL query.
        /// </summary>
        /// <returns>A list of users.</returns>
        Task<List<User>> GetAllUsersAsync(string bankId);

        /// <summary>
        /// Returns a user with the specified email using a raw SQL query.
        /// </summary>
        Task<User> GetUserByEmail(string email, string bankId);

        /// <summary>
        /// Updates the user in the database using a raw SQL query.
        /// </summary>
        Task<bool> UpdateUserAsync(User user, string bankId);
        

    }
}