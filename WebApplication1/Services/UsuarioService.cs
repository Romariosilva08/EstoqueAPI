//using System.Linq;
//using MinhaAPIEstoque.Data;
//using MinhaAPIEstoque.Models;

//namespace MinhaAPIEstoque.Services
//{
//    public class UsuariosService
//    {
//        private readonly MeuDbContext _dbContext;

//        public UsuariosService(MeuDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public Usuarios CreateUser(Usuarios user)
//        {
//            // Hash the password using BCrypt
//            user.Senha = BCrypt.Net.BCrypt.HashPassword(user.Senha);

//            // Save the user to the database
//            _dbContext.Usuarios.Add(user);
//            _dbContext.SaveChanges();

//            return user;
//        }

//        public Usuarios GetUserByEmail(string email)
//        {
//            // Retrieve the user from the database by email
//            return _dbContext.Usuarios.FirstOrDefault(u => u.Email == email);
//        }

//        public Usuarios UpdateUser(Usuarios user)
//        {
//            // Check if the user exists in the database
//            var existingUser = _dbContext.Usuarios.Find(user.Id);
//            if (existingUser == null)
//            {
//                return null;
//            }

//            // Update the user's properties
//            existingUser.Nome = user.Nome;
//            existingUser.Email = user.Email;

//            // If the password was changed, hash it
//            if (!string.IsNullOrEmpty(user.Senha))
//            {
//                existingUser.Senha = BCrypt.Net.BCrypt.HashPassword(user.Senha);
//            }

//            // Save the changes to the database
//            _dbContext.SaveChanges();

//            return existingUser;
//        }

//        public bool DeleteUser(int id)
//        {
//            // Check if the user exists in the database
//            var user = _dbContext.Usuarios.Find(id);
//            if (user == null)
//            {
//                return false;
//            }

//            // Delete the user from the database
//            _dbContext.Usuarios.Remove(user);
//            _dbContext.SaveChanges();

//            return true;
//        }
//    }
////}